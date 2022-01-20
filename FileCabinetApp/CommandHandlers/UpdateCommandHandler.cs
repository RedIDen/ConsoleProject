using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers;
public class UpdateCommandHandler : ServiceCommandWithWhereSyntaxHandlerBase
{
    public UpdateCommandHandler(FileCabinetTrasferHelper service)
        : base(service)
    {
    }

    protected override string CommandName { get; set; } = "update";

    protected override void MakeWork(string parameters)
    {
        var parametersAndPredicates = parameters.Split("where");

        char[] symbols = { ',', ' ', '(', ')', '\'', '\"', '=' };
        var parametersList = parametersAndPredicates[0].Split(symbols, StringSplitOptions.RemoveEmptyEntries).ToList();
        var record = new FileCabinetRecord() { DateOfBirth = new DateTime(0), Balance = -1, WorkExperience = -1, FavLetter = '\0'};

        for (int i = 1; i < parametersList.Count; i++)
        {
            var convertResult = parametersList[i].ToLower() switch
            {
                FirstNameWord => this.TryInsertFirstName(record, parametersList[++i]),
                LastNameWord => this.TryInsertLastName(record, parametersList[++i]),
                DateOfBirthWord => this.TryInsertDateOfBirth(record, parametersList[++i]),
                BalanceWord => this.TryInsertBalance(record, parametersList[++i]),
                WorkExperienceWord => this.TryInsertWorkExperience(record, parametersList[++i]),
                FavLetterWord => this.TryInsertFavLetter(record, parametersList[++i]),
                _ => (false, WrongSyntaxError),
            };

            if (!convertResult.Item1)
            {
                Console.WriteLine($"Error: {convertResult.Item2}.");
                return;
            }
        }

        var list = this.ParseWhereParameters(parametersAndPredicates[1]);
        var ids = new List<int>();

        int count = 0;
        foreach (var foundRecord in list)
        {
            ids.Add(foundRecord.Id);
            count++;
            this.service.Service.EditRecord(record, this.service.Service.FindRecordIndexById(foundRecord.Id));
        }

        if (count == 0)
        {
            Console.Write("No records found.");
        }
        else if (count == 1)
        {
            Console.Write($"Record #{ids[0]} is updated.");
        }
        else
        {
            Console.Write("Records ");
            foreach (var id in ids)
            {
                Console.Write('#');
                Console.Write(id);
                Console.Write(' ');
            }

            Console.WriteLine("are updated.");
        }
    }
}
