namespace FileCabinetApp.CommandHandlers;

internal class StorageCommandHandler : ServiceCommandHandlerBase
{
    public StorageCommandHandler(FileCabinetTrasferHelper service)
        : base(service)
    {
    }

    protected override string[] CommandNames { get; } = { "--storage", "-s" };

    /// <summary>
    /// Shows the list of all commands and their descriptions.
    /// </summary>
    /// <param name="parameters">Extra parameteres for the method.</param>
    protected override void MakeWork(string parameters)
    {
        var temp = this.transferHelper.Service is IServiceDecorator ? ((IServiceDecorator)this.transferHelper.Service).GetLast() : this.transferHelper.Service;

        if (parameters == "memory")
        {
            if (temp is FileCabinetMemoryService)
            {
                Console.WriteLine("This storage is already in use.");
                return;
            }

            ((FileCabinetFilesystemService)temp).Close();
            Program.storageTypeMessage = "Using memory storage.";
            this.transferHelper.SetLast(new FileCabinetMemoryService(temp.Validator));
        }
        else if (parameters == "file")
        {
            if (temp is FileCabinetFilesystemService)
            {
                Console.WriteLine("This storage is already in use.");
                return;
            }

            Program.storageTypeMessage = "Using filesystem storage.";
            this.transferHelper.SetLast(new FileCabinetFilesystemService(temp.Validator, File.Open(FileCabinetFilesystemService.FILENAME, FileMode.OpenOrCreate)));
        }
        else
        {
            Console.WriteLine("Wrong parameters!");
            return;
        }

        Program.WriteGreeting();
    }
}