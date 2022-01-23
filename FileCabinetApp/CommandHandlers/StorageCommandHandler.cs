namespace FileCabinetApp.CommandHandlers;

public class StorageCommandHandler : ServiceCommandHandlerBase
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
        var temp = this.service.Service is IServiceDecorator ? ((IServiceDecorator)this.service.Service).GetLast() : this.service.Service;

        if (parameters == "memory")
        {
            if (temp is FileCabinetMemoryService)
            {
                Console.WriteLine("This storage is already in use.");
                return;
            }

            ((FileCabinetFilesystemService)temp).Close();
            Program.storageTypeMessage = "Using memory storage.";
            this.service.SetLast(new FileCabinetMemoryService(temp.Validator));
        }
        else if (parameters == "file")
        {
            if (temp is FileCabinetFilesystemService)
            {
                Console.WriteLine("This storage is already in use.");
                return;
            }

            Program.storageTypeMessage = "Using filesystem storage.";
            this.service.SetLast(new FileCabinetFilesystemService(temp.Validator, File.Open(FileCabinetFilesystemService.FILENAME, FileMode.OpenOrCreate)));
        }
        else
        {
            Console.WriteLine("Wrong parameters!");
            return;
        }

        Program.WriteGreeting();
    }
}