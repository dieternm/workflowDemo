using System.Reflection;

namespace Workflow
{
    public abstract class Workflow<TWorkflowParticipant, TWorkflowState, TWorkflowOperation>
        where TWorkflowParticipant : IWorkflowParticipant<TWorkflowState>
        where TWorkflowState : struct, Enum
        where TWorkflowOperation : Enum
    {
        public IEnumerable<TWorkflowOperation> GetPossibleOperations(TWorkflowParticipant participant)
        {
            var possibleOperationsByState = GetOperations(participant.State);
            var possibleOperationsByRequiredProperties = GetPossibleOperationsByProperties(participant, possibleOperationsByState);
            return possibleOperationsByRequiredProperties;
        }

        public bool ExecuteOperation(TWorkflowParticipant participant, TWorkflowOperation operation, Object? parameters = null)
        {
            if (IsOperationAllowed(participant, operation))
            {
                (var nextState, var actionToInvoke) = GetNextStateAndActionToInvoke(participant.State, operation);
                actionToInvoke.Invoke(participant, parameters);
                participant.State = nextState!.Value;
                return true;
            }
            return false;
        }

        public bool IsOperationAllowed(TWorkflowParticipant participant, TWorkflowOperation operation)
        {
            var possibleOperations = GetPossibleOperations(participant);
            return possibleOperations.Contains(operation);
        }

        protected abstract (TWorkflowState? nextState, Func<TWorkflowParticipant, Object?, Task> actionToInvoke) GetNextStateAndActionToInvoke(TWorkflowState state, TWorkflowOperation operation);

        protected abstract IEnumerable<TWorkflowOperation> GetOperations(TWorkflowState state);

        protected virtual IEnumerable<PropertyInfo> GetRequiredProperties(TWorkflowParticipant participant, TWorkflowState state) => Enumerable.Empty<PropertyInfo>();
        
        protected Task NoOpAction(TWorkflowParticipant b, object? arguments)
        {
            return Task.CompletedTask;
        }

        private IEnumerable<TWorkflowOperation> GetPossibleOperationsByProperties(TWorkflowParticipant participant, IEnumerable<TWorkflowOperation> allowedOperations)
        {
            foreach (var allowedOperation in allowedOperations)
            {
                (var nextState, var _) = GetNextStateAndActionToInvoke(participant.State, allowedOperation);
                if (nextState.HasValue && ValidateRequiredProperties(participant, nextState.Value))
                {
                    yield return allowedOperation;
                }
            }
        }

        private bool ValidateRequiredProperties(TWorkflowParticipant participant, TWorkflowState state)
        {
            var requiredProperties = GetRequiredProperties(participant, state)
                .ToList()
                .AsReadOnly();
            if (requiredProperties.Any())
            {
                return !requiredProperties.Any(item => item.GetValue(participant) == null);
            }
            return true;
        }
    }
}