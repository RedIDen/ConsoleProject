namespace FileCabinetApp.CommandHandlers;

/// <summary>
/// The storage command handler.
/// </summary>
internal class StorageCommandHandler : ServiceCommandHandlerBase
{
    private readonly Action<string> setStorageString;

    /// <summary>
    /// Initializes a new instance of the <see cref="StorageCommandHandler"/> class.
    /// </summary>
    /// <param name="service">Transfer helper.</param>
    /// <param name="setStorageString">Delegate setting storage string.</param>
    public StorageCommandHandler(FileCabinetTrasferHelper service, Action<string> setStorageString)
        : base(service)
    {
        this.setStorageString = setStorageString;
    }

    /// <summary>
    /// Gets the list of command names (only full or full and short).
    /// </summary>
    /// <value>
    /// The list of command names (strings).
    /// </value>
    protected override string[] CommandNames { get; } = { "--storage", "-s" };

    /// <summary>
    /// Changes the storage type.
    /// </summary>
    /// <param name="parameters">Command parameters.</param>
    protected override void MakeWork(string parameters)
    {
        var temp = this.transferHelper.Service is IServiceDecorator decorator ? decorator.GetLast() : this.transferHelper.Service;

        if (parameters == "memory")
        {
            if (temp is FileCabinetMemoryService)
            {
                Console.WriteLine("This storage is already in use.");
                return;
            }

            ((FileCabinetFilesystemService)temp).Close();
            this.setStorageString("Using memory storage.");
            this.transferHelper.SetLast(new FileCabinetMemoryService(temp.Validator));
        }
        else if (parameters == "file")
        {
            if (temp is FileCabinetFilesystemService)
            {
                Console.WriteLine("This storage is already in use.");
                return;
            }

            this.setStorageString("Using filesystem storage.");
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