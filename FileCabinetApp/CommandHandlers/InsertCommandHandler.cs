using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers;
public class InsertCommandHandler : ServiceCommandHandlerBase
{
    public InsertCommandHandler(FileCabinetTrasferHelper service)
        : base(service)
    {
    }

    protected override string CommandName { get; set; } = "insert";

    protected override void MakeWork(string parameters)
    {
        char[] symbols = { ',', ' ', '(', ')', '\'', '\"' };
        var parametersList = parameters.Split(symbols, StringSplitOptions.RemoveEmptyEntries).ToList();

        const string wrondSyntaxError = "Wrong command syntax!";
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
            Console.WriteLine(wrondSyntaxError);
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
                _ => (false, wrondSyntaxError),
            };

            if (!convertResult.Item1)
            {
                Console.WriteLine($"Error: {convertResult.Item2}.");
                return;
            }
        }

        if (record.FirstName == null || record.LastName == null)
        {
            string oneOfNamesIsNullError = $"Error: first name and last name cannot be null.";
            Console.WriteLine(oneOfNamesIsNullError);
            return;
        }

        var validationResult = this.service.Service.Validator.Validate(record);

        int id = this.service.Service.CreateRecord(record);

        Console.WriteLine($"Record #{id} is created.");
    }

    private (bool, string) TryInsertId(FileCabinetRecord record, string value)
    {
        var result = this.IntConverter(value);
        if (result.Item1)
        {
            record.Id = result.Item3;
        }

        if (this.service.Service.FindRecordIndexById(result.Item3) != -1)
        {
            string thisIdAlreadyExistsError = "this id is already taken";
            return (false, thisIdAlreadyExistsError);
        }

        return (result.Item1, result.Item2);
    }

    private (bool, string) TryInsertFirstName(FileCabinetRecord record, string value)
    {
        var result = this.StringConverter(value);
        if (result.Item1)
        {
            record.FirstName = result.Item3;
        }

        return (result.Item1, result.Item2);
    }

    private (bool, string) TryInsertLastName(FileCabinetRecord record, string value)
    {
        var result = this.StringConverter(value);
        if (result.Item1)
        {
            record.LastName = result.Item3;
        }

        return (result.Item1, result.Item2);
    }

    private (bool, string) TryInsertDateOfBirth(FileCabinetRecord record, string value)
    {
        var result = this.DateConverter(value);
        if (result.Item1)
        {
            record.DateOfBirth = result.Item3;
        }

        return (result.Item1, result.Item2);
    }

    private (bool, string) TryInsertBalance(FileCabinetRecord record, string value)
    {
        var result = this.DecimalConverter(value);
        if (result.Item1)
        {
            record.Balance = result.Item3;
        }

        return (result.Item1, result.Item2);
    }

    private (bool, string) TryInsertWorkExperience(FileCabinetRecord record, string value)
    {
        var result = this.ShortConverter(value);
        if (result.Item1)
        {
            record.WorkExperience = result.Item3;
        }

        return (result.Item1, result.Item2);
    }

    private (bool, string) TryInsertFavLetter(FileCabinetRecord record, string value)
    {
        var result = this.CharConverter(value);
        if (result.Item1)
        {
            record.FavLetter = result.Item3;
        }

        return (result.Item1, result.Item2);
    }
}
