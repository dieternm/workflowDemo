using System;
using Workflow.Tests.Domain;
using Workflow.Tests.Domain.Workflow;

namespace WorkflowDemo
{
    internal class Program
    {
        private static Bestellung _participant = new Bestellung();
        private static BestellungWorkflow _workflow = new BestellungWorkflow();

        static void Main(string[] args)
        {
            PrintState();
            ExecuteOperation(BestellungWorkflowOperation.speichern);
            PrintState();
            GetOperationsOnly();
            IsOperationAllowed(BestellungWorkflowOperation.speichern);
            //WholeProcess();
        }

        private static void IsOperationAllowed(BestellungWorkflowOperation operation)
        {
            Console.Write($"operation = '{operation}'");
            var isOperationAllowed = _workflow.IsOperationAllowed(_participant, operation);
            Console.WriteLine($"; isOperationAllowed = '{isOperationAllowed}'");
        }

        private static void GetOperationsOnly()
        {
            var operations = _workflow.GetPossibleOperations(_participant);
            Console.WriteLine("Possible WorkflowOperations:");
            foreach (var operation in operations)
            {
                Console.Write("  ");
                Console.WriteLine(operation);
            }
        }

        private static void WholeProcess()
        {
            PrintAndExecute(BestellungWorkflowOperation.speichern);
            PrintAndExecute(BestellungWorkflowOperation.versenden);
            PrintAndExecute(BestellungWorkflowOperation.speichern);
            PrintAndExecute(BestellungWorkflowOperation.beenden);
        }

        private static void PrintAndExecute(BestellungWorkflowOperation operation)
        {
            PrintPossibleWorkflowOperations();
            ExecuteOperation(operation);
            PrintState();
            Console.WriteLine();
        }

        private static void ExecuteOperation(BestellungWorkflowOperation operation)
        {
            Console.Write($"operation = '{operation}'");
            var wasOperationSuccessful = _workflow.ExecuteOperation(_participant, operation);
            Console.WriteLine($"; sucessful = '{wasOperationSuccessful}'");
        }

        private static void PrintState()
        {
            Console.WriteLine($"actual state = '{_participant.State}'");
        }

        private static void PrintPossibleWorkflowOperations()
        {
            Console.WriteLine("Possible WorkflowOperations:");
            var operations = _workflow.GetPossibleOperations(_participant);
            foreach (var operation in operations)
            {
                Console.WriteLine($"  {operation}");
            }
        }
    }
}
