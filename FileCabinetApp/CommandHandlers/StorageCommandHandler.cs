using System.Globalization;

namespace FileCabinetApp.CommandHandlers;
public class StorageCommandHandler : CommandHandlerBase
{
    protected override string CommandName { get; set; } = "--storage";

    private string ShortCommandName { get; set; } = "-s";

    public StorageCommandHandler(IFileCabinetService fileCabinetService)
    {
        this.fileCabinetService = fileCabinetService;
    }

    public override void Handle(AppCommandRequest appCommandRequest)
    {
        if (string.Equals(appCommandRequest.Command, this.CommandName, StringComparison.InvariantCultureIgnoreCase) ||
            string.Equals(appCommandRequest.Command, this.ShortCommandName, StringComparison.InvariantCultureIgnoreCase))
        {
            this.MakeWork(appCommandRequest.Parameters);
        }
        else if (this.nextHandler != null)
        {
            this.nextHandler.Handle(appCommandRequest);
        }
        else
        {
            Console.WriteLine($"There is no '{appCommandRequest.Command}' command.");
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Shows the list of all commands and their descriptions.
    /// </summary>
    /// <param name="parameters">Extra parameteres for the method.</param>
    protected override void MakeWork(string parameters)
    {
        if (parameters == "memory")
        {
            if (this.fileCabinetService is FileCabinetMemoryService)
            {
                Console.WriteLine("This storage is already in use.");
                return;
            }

            ((FileCabinetFilesystemService)this.fileCabinetService).Close();
            Program.storageTypeMessage = "Using memory storage.";
            this.fileCabinetService = new FileCabinetMemoryService(this.fileCabinetService.Validator);
        }
        else if (parameters == "file")
        {
            if (this.fileCabinetService is FileCabinetFilesystemService)
            {
                Console.WriteLine("This storage is already in use.");
                return;
            }

            Program.storageTypeMessage = "Using filesystem storage.";
            this.fileCabinetService = new FileCabinetFilesystemService(this.fileCabinetService.Validator, File.Open(FileCabinetFilesystemService.FILENAME, FileMode.OpenOrCreate));
        }
        else
        {
            Console.WriteLine("Wrong parameters!");
            return;
        }

        Program.WriteGreeting();
    }
}