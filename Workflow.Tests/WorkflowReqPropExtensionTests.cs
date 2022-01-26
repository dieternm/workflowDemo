using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Workflow.Tests.Domain;
using Workflow.Tests.Domain.Workflow;

namespace Workflow.Tests
{
    [TestClass]
    public class WorkflowReqPropExtensionTests
    {
        private Bestellung _bestellung = new();
        private BestellungWorkflow _sut = new();

        [TestMethod]
        public void ExecuteOperation_RequiredPropertyNotSet_NotExecuted()
        {
            _bestellung.State = BestellungWorkflowState.Angelegt;
            _bestellung.Empfänger = null;

            var wasOperationExecuted = _sut.ExecuteOperation(_bestellung, BestellungWorkflowOperation.versenden);

            wasOperationExecuted.Should().BeFalse();
        }

        [TestMethod]
        public void ExecuteOperation_RequiredPropertySet_Executed()
        {
            _bestellung.State = BestellungWorkflowState.Angelegt;
            _bestellung.Empfänger = "UnitTest";

            var wasOperationExecuted = _sut.ExecuteOperation(_bestellung, BestellungWorkflowOperation.versenden);

            wasOperationExecuted.Should().BeTrue();
        }
    }
}
