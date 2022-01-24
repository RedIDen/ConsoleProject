namespace FileCabinetApp.CommandHandlers;

/// <summary>
/// The insert command hanlder.
/// </summary>
internal class InsertCommandHandler : ServiceCommandHandlerBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InsertCommandHandler"/> class.
    /// </summary>
    /// <param name="service">Transfer helper.</param>
    public InsertCommandHandler(FileCabinetTrasferHelper service)
        : base(service)
    {
    }

    /// <summary>
    /// Gets the list of command names (only full or full and short).
    /// </summary>
    /// <value>
    /// The list of command names (strings).
    /// </value>
    protected override string[] CommandNames { get; } = { "insert" };

    /// <summary>
    /// Inserts the new record with specified data.
    /// </summary>
    /// <param name="parameters">Command parameters.</param>
    protected override void MakeWork(string parameters)
    {
        char[] symbols = { ',', ' ', '(', ')', '\'', '\"' };
        var parametersList = parameters.Split(symbols, StringSplitOptions.RemoveEmptyEntries).ToList();

        const string valuesWord = "values";
        const string idWord = "id";
        const string firstNameWord = "firstname";
        const string lastNameWord = "lastname";
        const string dateOfBirthWord = "dateofbirth";
        const string balanceWord = "balance";
        const string workExperienceWord = "workexperience";
        const string favLetterWord = "favletter";

        int valuesWordPosition = parametersList.FindIndex(x => x.Equals(valuesWord, StringComparison.OrdinalIgnoreCase));

        if (valuesWordPosition == -1 || parametersList.Count != (valuesWordPosition * 2) + 1)
        {
            Console.WriteLine(WrongSyntaxError);
            return;
        }

        int range = valuesWordPosition + 1;

        var record = new FileCabinetRecord();

        for (int i = 0; i < valuesWordPosition; i++)
        {
            (bool, string) convertResult = parametersList[i].ToLower(CultureInfo.InvariantCulture) switch
            {
                idWord => this.TryInsertId(record, parametersList[i + range]),
                firstNameWord => this.TryInsertFirstName(record, parametersList[i + range]),
                lastNameWord => this.TryInsertLastName(record, parametersList[i + range]),
                dateOfBirthWord => this.TryInsertDateOfBirth(record, parametersList[i + range]),
                balanceWord => this.TryInsertBalance(record, parametersList[i + range]),
                workExperienceWord => this.TryInsertWorkExperience(record, parametersList[i + range]),
                favLetterWord => this.TryInsertFavLetter(record, parametersList[i + range]),
                _ => (false, WrongSyntaxError),
            };

            if (!convertResult.Item1)
            {
                var convertError = $"Error: {convertResult.Item2}.";
                Console.WriteLine(convertError);
                return;
            }
        }

        var validationResult = this.transferHelper.Service.Validator.Validate(record);

        if (validationResult.Item1)
        {
            int id = this.transferHelper.Service.CreateRecord(record);
            Console.WriteLine($"Record #{id} is created.");
        }
        else
        {
            var validationError = $"Error: {validationResult.Item2}.";
            Console.WriteLine(validationError);
        }
    }
}
