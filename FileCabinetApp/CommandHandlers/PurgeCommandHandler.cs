using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers;
public class PurgeCommandHandler : CommandHandlerBase
{
    protected override string CommandName { get; set; } = "purge";

    public PurgeCommandHandler(IFileCabinetService fileCabinetService)
    {
        this.fileCabinetService = fileCabinetService;
    }

    protected override void MakeWork(string parameters)
    {
        if (this.fileCabinetService is not FileCabinetFilesystemService)
        {
            Console.WriteLine("This command is available only for filesystem storage mode.");
            return;
        }

        (int deletedNum, int beforeNum) = ((FileCabinetFilesystemService)this.fileCabinetService).Purge();

        Console.WriteLine($"Data file processing is completed: {deletedNum} of {beforeNum} records were purged.");
    }
}