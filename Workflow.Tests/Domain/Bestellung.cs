using Workflow.Tests.Domain.Workflow;

namespace Workflow.Tests.Domain
{
    public class Bestellung : IWorkflowParticipant<BestellungWorkflowState>
    {
        public BestellungWorkflowState State { get; set; }
        public string? Empfaenger { get; set; }
    }
}
