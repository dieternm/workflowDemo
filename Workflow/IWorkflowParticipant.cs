namespace Workflow
{
    public interface IWorkflowParticipant<TWorkflowState>
        where TWorkflowState : Enum
    {
        TWorkflowState State { get; internal set; }
    }
}