using Workflow.Tests.Domain;

namespace Workflow.Tests.Logic
{
    public class BestellungLogic
    {
        public static Task SetEmpfaenger(Bestellung b, object? arguments)
        {
            var typedArgument = arguments as string;
            b.Empfaenger = typedArgument;
            return Task.CompletedTask;
        }
    }
}
