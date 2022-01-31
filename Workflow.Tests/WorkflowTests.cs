using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Workflow.Tests.Domain;
using Workflow.Tests.Domain.Workflow;
using WorkflowState = Workflow.Tests.Domain.Workflow.BestellungWorkflowState;
using WorkflowOperation = Workflow.Tests.Domain.Workflow.BestellungWorkflowOperation;

namespace Workflow.Tests
{
    [TestClass]
    public class WorkflowTests
    {
        private Bestellung _bestellung = new();
        private BestellungWorkflow _sut = new();

        [DataTestMethod]
        [DataRow(WorkflowState.Angelegt, new[]
            {
                WorkflowOperation.speichern,
                WorkflowOperation.stornieren,
                WorkflowOperation.versenden
            })]
        [DataRow(WorkflowState.Storniert, new WorkflowOperation[] { })]
        public void GetOperations(WorkflowState state, WorkflowOperation[] expectedOperations)
        {
            _bestellung.State = state;

            var operations = _sut.GetOperations(_bestellung);

            operations.Should().BeEquivalentTo(expectedOperations);
        }

        [DataTestMethod]
        [DataRow(WorkflowState.Start, WorkflowOperation.speichern, WorkflowState.Angelegt)]
        [DataRow(WorkflowState.Angelegt, WorkflowOperation.speichern, WorkflowState.Angelegt)]
        public void ExecuteOperation(WorkflowState state,  WorkflowOperation operation, WorkflowState expectedState)
        {
            _bestellung.State = state;

            var wasOperaionSuccessfull = _sut.ExecuteOperation(_bestellung, operation);

            wasOperaionSuccessfull.Should().BeTrue();
            _bestellung.State.Should().Be(expectedState);
        }

        [DataTestMethod]
        [DataRow(WorkflowState.Start, WorkflowOperation.speichern, true)]
        [DataRow(WorkflowState.Start, WorkflowOperation.beenden, false)]
        [DataRow(WorkflowState.Storniert, WorkflowOperation.beenden, false)]
        public void IsOperationAllowed(WorkflowState state, WorkflowOperation operation, bool expectedIsOperationAllowed)
        {
            _bestellung.State = state;

            var isOperationAllowed = _sut.IsOperationAllowed(_bestellung, operation);

            isOperationAllowed.Should().Be(expectedIsOperationAllowed);
        }
    }
}