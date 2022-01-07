using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers;
public class PurgeCommandHandler : CommandHandlerBase
{
    protected override string CommandName { get; set; } = "purge";

    protected override void MakeWork(string parameters)
    {
        if (Program.fileCabinetService is not FileCabinetFilesystemService)
        {
            Console.WriteLine("This command is available only for filesystem storage mode.");
            return;
        }

        (int deletedNum, int beforeNum) = ((FileCabinetFilesystemService)Program.fileCabinetService).Purge();

        Console.WriteLine($"Data file processing is completed: {deletedNum} of {beforeNum} records were purged.");
    }
}