using System.Reflection;

namespace Workflow
{
    public abstract partial class Workflow<TWorkflowParticipant, TWorkflowState, TWorkflowOperation>
        where TWorkflowParticipant : IWorkflowParticipant<TWorkflowState>
        where TWorkflowState : struct, Enum
        where TWorkflowOperation : Enum
    {
        protected virtual IEnumerable<PropertyInfo> GetRequiredProperties(TWorkflowParticipant participant) => Enumerable.Empty<PropertyInfo>();

        private bool ValidateRequiredProperties(TWorkflowParticipant participant)
        {
            var requiredProperties = GetRequiredProperties(participant)
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