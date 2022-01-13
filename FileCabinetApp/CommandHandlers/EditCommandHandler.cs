using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers;
public class EditCommandHandler : ServiceCommandHandlerBase
{
    public EditCommandHandler(FileCabinetServiceTransferHelper fileCabinetServiceTransferHelper)
        : base(fileCabinetServiceTransferHelper)
    {
    }

    protected override string CommandName { get; set; } = "edit";

    protected override void MakeWork(string parameters)
    {
        string firstName;
        string lastName;
        DateTime dateOfBirth;
        short workExperience;
        decimal balance;
        char favLetter;

        if (!int.TryParse(parameters, out int id))
        {
            Console.WriteLine("Wrong command syntax!");
            return;
        }

        int index = this.fileCabinetServiceTransferHelper.fileCabinetService.FindRecordIndexById(id);

        if (index == -1)
        {
            Console.WriteLine($"#{id} record is not found.");
            return;
        }

        this.ReadDataForRecord(out firstName, out lastName, out dateOfBirth, out workExperience, out balance, out favLetter);

        this.fileCabinetServiceTransferHelper.fileCabinetService.EditRecord(
            new FileCabinetRecord()
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                WorkExperience = workExperience,
                Balance = balance,
                FavLetter = favLetter,
            },
            index);

        Console.WriteLine($"Record #{id} is edited.");
    }
}
