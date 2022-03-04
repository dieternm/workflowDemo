using System.Reflection;
using Workflow.Tests.Utils;

namespace Workflow.Tests.Domain.Workflow
{
    public partial class BestellungWorkflow : Workflow<Bestellung, BestellungWorkflowState, BestellungWorkflowOperation>
    {
        protected override BestellungWorkflowState? GetNextState(BestellungWorkflowState state, BestellungWorkflowOperation operation)
        {
            return operation switch
            {
                BestellungWorkflowOperation.speichern when state == BestellungWorkflowState.Start => BestellungWorkflowState.Angelegt,
                BestellungWorkflowOperation.speichern => state,
                BestellungWorkflowOperation.versenden => BestellungWorkflowState.InVersand,
                BestellungWorkflowOperation.beenden => BestellungWorkflowState.Ende,
                BestellungWorkflowOperation.stornieren => BestellungWorkflowState.Storniert,
                BestellungWorkflowOperation.pruefen => BestellungWorkflowState.Geprueft,
                _ => null
            };
        }

        protected override IEnumerable<BestellungWorkflowOperation> GetOperations(BestellungWorkflowState state)
        {
            switch (state)
            {
                case BestellungWorkflowState.Start:
                    yield return BestellungWorkflowOperation.speichern;
                    break;
                case BestellungWorkflowState.Angelegt:
                    yield return BestellungWorkflowOperation.speichern;
                    yield return BestellungWorkflowOperation.stornieren;
                    yield return BestellungWorkflowOperation.pruefen;
                    break;
                case BestellungWorkflowState.InVersand:
                    yield return BestellungWorkflowOperation.beenden;
                    break;
                case BestellungWorkflowState.Geprueft:
                    yield return BestellungWorkflowOperation.versenden;
                    break;
                default:
                    yield break;
            }
        }

        protected override IEnumerable<PropertyInfo> GetRequiredProperties(Bestellung participant, BestellungWorkflowState state)
        {
            switch (state)
            {
                case BestellungWorkflowState.InVersand:
                    yield return PropertyHelper.GetPropertyInfo((Bestellung b) => b.Empfaenger);
                    break;
                case BestellungWorkflowState.Storniert:
                default:
                    yield break;
            }
        }

        protected override Func<Bestellung, object?, Task> GetActionToInvoke(BestellungWorkflowState state, BestellungWorkflowOperation operation)
        {
            return operation switch
            {
                BestellungWorkflowOperation.speichern when state == BestellungWorkflowState.Start => BestellungLogic.SetEmpfaenger,
                _ => base.GetActionToInvoke(state, operation)
            };
        }

        public class BestellungLogic
        {
            public static Task SetEmpfaenger(Bestellung b, object? arguments)
            {
                var typedArgument = arguments as string;
                b.Empfaenger = typedArgument;
                return Task.CompletedTask;
            }
        }
    }
}
