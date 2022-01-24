namespace FileCabinetApp.CommandHandlers;

/// <summary>
/// The validation rules command handler.
/// </summary>
internal class ValidationRulesCommandHandler : ServiceCommandHandlerBase
{
    private readonly Dictionary<string, CompositeValidator> validators;

    private readonly Action<string> setValidationString;

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationRulesCommandHandler"/> class.
    /// </summary>
    /// <param name="service">Transfer helper.</param>
    /// <param name="validators">The dictionary of validators and their names.</param>
    /// <param name="setValidationString">Delegate setting validation string.</param>
    public ValidationRulesCommandHandler(FileCabinetTrasferHelper service, Dictionary<string, CompositeValidator> validators, Action<string> setValidationString)
        : base(service)
    {
        this.validators = validators;
        this.setValidationString = setValidationString;
    }

    /// <summary>
    /// Gets the list of command names (only full or full and short).
    /// </summary>
    /// <value>
    /// The list of command names (strings).
    /// </value>
    protected override string[] CommandNames { get; } = { "--validation-rules", "-v" };

    /// <summary>
    /// Changes the validator.
    /// </summary>
    /// <param name="parameters">Command parameters.</param>
    protected override void MakeWork(string parameters)
    {
        var parametersToLower = parameters.ToLower(CultureInfo.InvariantCulture);
        var validator = this.validators.GetValueOrDefault(parametersToLower);
        if (validator != null)
        {
            this.transferHelper.Service.Validator = validator;
            this.setValidationString($"Using {parametersToLower} validation rules.");
        }
        else
        {
            Console.WriteLine("Wrong parameters!");
            return;
        }

        Program.WriteGreeting();
    }
}