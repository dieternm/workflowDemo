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
    }
}
