using System.Reflection;
using Workflow.Tests.Utils;

namespace Workflow.Tests.Domain.Workflow
{
    public partial class BestellungWorkflow : Workflow<Bestellung, BestellungWorkflowState, BestellungWorkflowOperation>
    {
        protected override IEnumerable<PropertyInfo> GetRequiredProperties(Bestellung participant)
        {
            switch (participant.State)

            {
                case BestellungWorkflowState.InVersand:
                    yield return PropertyHelper.GetPropertyInfo((Bestellung Bestellung) => Bestellung.Empfänger);
                    break;
                case BestellungWorkflowState.Storniert:
                default:
                    yield break;
            }
        }
    }
}
