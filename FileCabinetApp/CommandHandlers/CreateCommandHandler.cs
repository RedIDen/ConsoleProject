using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers;
public class CreateCommandHandler : CommandHandlerBase
{
    protected override string CommandName { get; set; } = "create";

    public CreateCommandHandler(IFileCabinetService fileCabinetService)
    {
        this.fileCabinetService = fileCabinetService;
    }

    protected override void MakeWork(string parameters)
    {
        string firstName;
        string lastName;
        DateTime dateOfBirth;
        short workExperience;
        decimal balance;
        char favLetter;

        this.ReadDataForRecord(out firstName, out lastName, out dateOfBirth, out workExperience, out balance, out favLetter);

        int id = this.fileCabinetService.CreateRecord(
            new FileCabinetRecord()
            {
                Id = 0,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                WorkExperience = workExperience,
                Balance = balance,
                FavLetter = favLetter,
            });

        Console.WriteLine($"Record #{id} is created.");
    }
}
