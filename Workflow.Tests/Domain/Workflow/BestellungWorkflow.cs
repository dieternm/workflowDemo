using System.Reflection;
using Workflow.Tests.Logic;
using Workflow.Tests.Utils;

namespace Workflow.Tests.Domain.Workflow
{
    public partial class BestellungWorkflow : Workflow<Bestellung, BestellungWorkflowState, BestellungWorkflowOperation>
    {
        protected override (BestellungWorkflowState? nextState, Func<Bestellung, Object?, Task> actionToInvoke) GetNextStateAndActionToInvoke(BestellungWorkflowState state, BestellungWorkflowOperation operation)
        {
            return operation switch
            {
                BestellungWorkflowOperation.speichern when state == BestellungWorkflowState.Start => (BestellungWorkflowState.Angelegt, BestellungLogic.SetEmpfaenger),
                BestellungWorkflowOperation.speichern => (state, NoOpAction),
                BestellungWorkflowOperation.versenden => (BestellungWorkflowState.InVersand, NoOpAction),
                BestellungWorkflowOperation.beenden => (BestellungWorkflowState.Ende, NoOpAction),
                BestellungWorkflowOperation.stornieren => (BestellungWorkflowState.Storniert, NoOpAction),
                BestellungWorkflowOperation.pruefen => (BestellungWorkflowState.Geprueft, NoOpAction),
                _ => (null, NoOpAction),
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
    }
}
