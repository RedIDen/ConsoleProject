using System.Globalization;

namespace FileCabinetApp.CommandHandlers;
public class ValidationRulesCommandHandler : CommandHandlerBase
{
    protected override string CommandName { get; set; } = "--validation-rules";
    private string ShortCommandName { get; set; } = "-v";

    public override void Handle(AppCommandRequest appCommandRequest)
    {
        if (string.Equals(appCommandRequest.Command, this.CommandName, StringComparison.InvariantCultureIgnoreCase) ||
            string.Equals(appCommandRequest.Command, this.ShortCommandName, StringComparison.InvariantCultureIgnoreCase))
        {
            this.MakeWork(appCommandRequest.Parameters);
        }
        else if (this.nextHandler != null)
        {
            this.nextHandler.Handle(appCommandRequest);
        }
        else
        {
            Console.WriteLine($"There is no '{appCommandRequest.Command}' command.");
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Shows the list of all commands and their descriptions.
    /// </summary>
    /// <param name="parameters">Extra parameteres for the method.</param>
    protected override void MakeWork(string parameters)
    {
        if (parameters.Equals("default", StringComparison.InvariantCultureIgnoreCase))
        {
            Program.fileCabinetService.Validator = new DefaultValidator();
            Program.validationRulesMessage = "Using default validation rules.";
        }
        else if (parameters.Equals("custom", StringComparison.InvariantCultureIgnoreCase))
        {
            Program.fileCabinetService.Validator = new CustomValidator();
            Program.validationRulesMessage = "Using custom validation rules.";
        }
        else
        {
            Console.WriteLine("Wrong parameters!");
            return;
        }

        Program.WriteGreeting();
    }
}