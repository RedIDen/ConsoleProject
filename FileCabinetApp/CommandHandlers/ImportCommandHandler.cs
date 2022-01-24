namespace FileCabinetApp.CommandHandlers;

/// <summary>
/// The import command handler.
/// </summary>
internal class ImportCommandHandler : ServiceCommandHandlerBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ImportCommandHandler"/> class.
    /// </summary>
    /// <param name="service">Transfer helper.</param>
    public ImportCommandHandler(FileCabinetTrasferHelper service)
        : base(service)
    {
    }

    /// <summary>
    /// Gets the list of command names (only full or full and short).
    /// </summary>
    /// <value>
    /// The list of command names (strings).
    /// </value>
    protected override string[] CommandNames { get; } = { "import" };

    /// <summary>
    /// Imports the records data from file.
    /// </summary>
    /// <param name="parameters">Command parameters.</param>
    protected override void MakeWork(string parameters)
    {
        var parametersArray = parameters.Trim().Split();

        if (parametersArray.Length != 2)
        {
            Console.WriteLine(WrongSyntaxError);
            return;
        }

        string fileType = parametersArray[0];
        string fileName = parametersArray[1];

        if (!fileType.Equals("csv", StringComparison.OrdinalIgnoreCase) &&
            !fileType.Equals("xml", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Wrong file type!");
            return;
        }

        if (File.Exists(fileName))
        {
            try
            {
                var fileStream = new FileStream(fileName, FileMode.Open);

                var snapshot = new FileCabinetServiceSnapshot();

                if (fileType.Equals("csv", StringComparison.OrdinalIgnoreCase))
                {
                    snapshot.LoadFromCsv(fileStream);
                }
                else
                {
                    snapshot.LoadFromXml(fileStream);
                }

                this.transferHelper.Service.Restore(snapshot);

                Console.WriteLine($"All records are imported from {fileName}.");
                fileStream.Close();
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine($"Import failed: can't open file {fileName}.");
                return;
            }
        }
        else
        {
            Console.Write($"This file doesn't exist!");
        }
    }
}
