using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers;
public class PurgeCommandHandler : ServiceCommandHandlerBase
{
    public PurgeCommandHandler(FileCabinetServiceTransferHelper fileCabinetServiceTransferHelper)
        : base(fileCabinetServiceTransferHelper)
    {
    }

    protected override string CommandName { get; set; } = "purge";

    protected override void MakeWork(string parameters)
    {
        if (this.fileCabinetServiceTransferHelper.fileCabinetService is not FileCabinetFilesystemService)
        {
            Console.WriteLine("This command is available only for filesystem storage mode.");
            return;
        }

        (int deletedNum, int beforeNum) = ((FileCabinetFilesystemService)this.fileCabinetServiceTransferHelper.fileCabinetService).Purge();

        Console.WriteLine($"Data file processing is completed: {deletedNum} of {beforeNum} records were purged.");
    }
}