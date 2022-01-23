namespace FileCabinetApp.CommandHandlers;
public class InsertCommandHandler : ServiceCommandHandlerBase
{
    public InsertCommandHandler(FileCabinetTrasferHelper service)
        : base(service)
    {
    }

    protected override string[] CommandNames { get; } = { "insert" };

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

        int valuesWordPosition = parametersList.FindIndex(x => x.Equals(valuesWord, StringComparison.InvariantCultureIgnoreCase));

        if (valuesWordPosition == -1 || parametersList.Count != (valuesWordPosition * 2) + 1)
        {
            Console.WriteLine(WrongSyntaxError);
            return;
        }

        int range = valuesWordPosition + 1;

        var record = new FileCabinetRecord();

        for (int i = 0; i < valuesWordPosition; i++)
        {
            (bool, string) convertResult = parametersList[i].ToLower() switch
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
                Console.WriteLine($"Error: {convertResult.Item2}.");
                return;
            }
        }

        if (record.FirstName == null || record.LastName == null)
        {
            string oneOfNamesIsNullError = $"Error: first name and last name can not be null.";
            Console.WriteLine(oneOfNamesIsNullError);
            return;
        }

        var validationResult = this.service.Service.Validator.Validate(record);

        int id = this.service.Service.CreateRecord(record);

        Console.WriteLine($"Record #{id} is created.");
    }
}
