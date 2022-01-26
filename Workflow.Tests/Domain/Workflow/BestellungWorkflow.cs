namespace Workflow.Tests.Domain.Workflow
{
    public enum BestellungWorkflowState
    {
        Start,
        Angelegt,
        InVersand,
        Ende,
        Storniert
    }

    public enum BestellungWorkflowOperation
    {
        speichern,
        versenden,
        stornieren,
        beenden
    }

    public partial class BestellungWorkflow : Workflow<Bestellung, BestellungWorkflowState, BestellungWorkflowOperation>
    {
        protected override BestellungWorkflowState? GetNextState(BestellungWorkflowState state, BestellungWorkflowOperation operation)
        {
            return operation switch
            {
                BestellungWorkflowOperation.speichern => BestellungWorkflowState.Angelegt,
                BestellungWorkflowOperation.versenden => BestellungWorkflowState.InVersand,
                BestellungWorkflowOperation.beenden => BestellungWorkflowState.Ende,
                BestellungWorkflowOperation.stornieren => BestellungWorkflowState.Storniert,
                _ => null,
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
                    yield return BestellungWorkflowOperation.versenden;
                    break;
                case BestellungWorkflowState.InVersand:
                    yield return BestellungWorkflowOperation.beenden;
                    break;
                default:
                    yield break;
            }
        }
    }
}
