using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Workflow.Tests.Domain;
using Workflow.Tests.Domain.Workflow;
using WorkflowState = Workflow.Tests.Domain.Workflow.BestellungWorkflowState;
using WorkflowOperation = Workflow.Tests.Domain.Workflow.BestellungWorkflowOperation;

namespace Workflow.Tests
{
    [TestClass]
    public class WorkflowRequiredPropertiesTests
    {
        private Bestellung _bestellung = new();
        private BestellungWorkflow _sut = new();

        [TestMethod]
        [DataRow("UnitTest", true)]
        [DataRow(null, false)]
        public void ExecuteOperation(string empfaenger, bool wasExecuted)
        {
            _bestellung.State = WorkflowState.Geprueft;
            _bestellung.Empfaenger = empfaenger;

            var operations = _sut.GetPossibleOperations(_bestellung);
            var wasOperationExecuted = _sut.ExecuteOperation(_bestellung, BestellungWorkflowOperation.versenden);

            wasOperationExecuted.Should().Be(wasExecuted);
        }

        [DataTestMethod]
        [DataRow("UnitTest", new WorkflowOperation[] { WorkflowOperation.versenden })]
        [DataRow(null, new WorkflowOperation[] { })]
        public void GetPossibleOperations(string empfaenger, WorkflowOperation[] expectedOperations)
        {
            _bestellung.State = WorkflowState.Geprueft;
            _bestellung.Empfaenger = empfaenger;

            var operations = _sut.GetPossibleOperations(_bestellung);

            operations.Should().BeEquivalentTo(expectedOperations);
        }
    }
}
