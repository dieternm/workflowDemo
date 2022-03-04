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
                WorkflowOperation.pruefen
            })]
        [DataRow(WorkflowState.Storniert, new WorkflowOperation[] { })]
        public void GetPossibleOperations(WorkflowState state, WorkflowOperation[] expectedOperations)
        {
            _bestellung.State = state;

            var operations = _sut.GetPossibleOperations(_bestellung);

            operations.Should().BeEquivalentTo(expectedOperations);
        }

        [DataTestMethod]
        [DataRow(WorkflowState.Start, WorkflowOperation.speichern, WorkflowState.Angelegt)]
        [DataRow(WorkflowState.Angelegt, WorkflowOperation.speichern, WorkflowState.Angelegt)]
        public void ExecuteOperation_NextState(WorkflowState state,  WorkflowOperation operation, WorkflowState expectedState)
        {
            _bestellung.State = state;

            var wasOperationSuccessful = _sut.ExecuteOperation(_bestellung, operation);

            wasOperationSuccessful.Should().BeTrue();
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
    
        [TestMethod]
        public void ExecuteOperation_SpeichernWhenStart_SetEmpfaengerInvoked()
        {
            _bestellung.State = WorkflowState.Start;
            var argument = "TestValue";
            _sut.ExecuteOperation(_bestellung, WorkflowOperation.speichern, argument);
            _bestellung.Empfaenger.Should().Be(argument);
        }

        [TestMethod]
        public void ExecuteOperation_SpeichernWhenAngelegt_SetEmpfaenger_NotInvoked()
        {
            _bestellung.State = WorkflowState.Angelegt;
            _sut.ExecuteOperation(_bestellung, WorkflowOperation.speichern);
            _bestellung.Empfaenger.Should().BeNull();
        }
    }
}