using System.Reflection;
using Workflow.Tests.Utils;

namespace Workflow.Tests.Domain.Workflow
{
    public partial class BestellungWorkflow
    {
        protected override IEnumerable<PropertyInfo> GetRequiredProperties(Bestellung participant)
        {
            switch (participant.State)
            {
                case BestellungWorkflowState.InVersand:
                    yield return PropertyHelper.GetPropertyInfo((Bestellung b) => b.Empfänger);
                    break;
                case BestellungWorkflowState.Storniert:
                default:
                    yield break;
            }
        }
    }
}
