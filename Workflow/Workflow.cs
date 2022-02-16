namespace Workflow
{
    public abstract partial class Workflow<TWorkflowParticipant, TWorkflowState, TWorkflowOperation>
        where TWorkflowParticipant : IWorkflowParticipant<TWorkflowState>
        where TWorkflowState : struct, Enum
        where TWorkflowOperation : Enum
    {
        protected abstract TWorkflowState? GetNextState(TWorkflowState state, TWorkflowOperation operation);

        protected abstract IEnumerable<TWorkflowOperation> GetOperations(TWorkflowState state);

        public IEnumerable<TWorkflowOperation> GetOperations(TWorkflowParticipant participant)
        {
            return GetOperations(participant.State);
        }

        public bool IsOperationAllowed(TWorkflowParticipant participant, TWorkflowOperation operation)
        {
            var allowedOperations = GetOperations(participant.State);
            return allowedOperations.Contains(operation);
        }

        public bool ExecuteOperation(TWorkflowParticipant participant, TWorkflowOperation operation)
        {
            if (IsOperationAllowed(participant, operation))
            {
                participant.State = GetNextState(participant.State, operation)!.Value;
                // for ReqPropExtension
                return ValidateRequiredProperties(participant);
            }
            return false;
        }
    }
}