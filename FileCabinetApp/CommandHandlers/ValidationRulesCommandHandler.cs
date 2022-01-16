using System.Globalization;

namespace FileCabinetApp.CommandHandlers;
public class ValidationRulesCommandHandler : ServiceCommandHandlerBase
{
    private Dictionary<string, CompositeValidator> validators;

    public ValidationRulesCommandHandler(FileCabinetTrasferHelper service, Dictionary<string, CompositeValidator> validators)
        : base(service)
    {
        this.validators = validators;
    }

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
        var parametersToLower = parameters.ToLower();
        var validator = this.validators.GetValueOrDefault(parametersToLower);
        if (validator != null)
        {
            this.service.Service.Validator = validator;
            Program.validationRulesMessage = $"Using {parametersToLower} validation rules.";
        }
        else
        {
            Console.WriteLine("Wrong parameters!");
            return;
        }

        Program.WriteGreeting();
    }
}