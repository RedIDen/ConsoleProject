namespace FileCabinetApp.CommandHandlers;

/// <summary>
/// The export command handler.
/// </summary>
internal class ExportCommandHandler : ServiceCommandHandlerBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExportCommandHandler"/> class.
    /// </summary>
    /// <param name="service">Trasfer helper.</param>
    public ExportCommandHandler(FileCabinetTrasferHelper service)
        : base(service)
    {
    }

    /// <summary>
    /// Gets the list of command names (only full or full and short).
    /// </summary>
    /// <value>
    /// The list of command names (strings).
    /// </value>
    protected override string[] CommandNames { get; } = { "export" };

    /// <summary>
    /// Exports the records data to file.
    /// </summary>
    /// <param name="parameters">Command parameters.</param>
    protected override void MakeWork(string parameters)
    {
        var parametersArray = parameters.Trim().Split();

        if (parametersArray.Length != 2)
        {
            Console.WriteLine("Wrong command syntax!");
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
            Console.Write($"File exists - rewrite {fileName}? [Y/n] ");
            do
            {
                ConsoleKey key = Console.ReadKey().Key;
                if (key == ConsoleKey.Y)
                {
                    Console.WriteLine();
                    break;
                }

                if (key == ConsoleKey.N)
                {
                    Console.WriteLine();
                    return;
                }
            }
            while (true);
        }

        try
        {
            var streamWriter = new StreamWriter(fileName);
            var fileCabinetServiceSnapshot = this.transferHelper.Service.MakeSnapshot();

            if (fileType.Equals("csv", StringComparison.OrdinalIgnoreCase))
            {
                fileCabinetServiceSnapshot.SaveToCsv(streamWriter);
            }
            else
            {
                fileCabinetServiceSnapshot.SaveToXml(streamWriter);
            }

            Console.WriteLine($"All records are exported to {fileName}.");
            streamWriter.Close();
        }
        catch (DirectoryNotFoundException)
        {
            Console.WriteLine($"Export failed: can't open file {fileName}.");
            return;
        }
    }
}