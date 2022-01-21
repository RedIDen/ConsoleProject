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

    protected override string[] CommandNames { get; } = { "--validation-rules", "-v" };

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