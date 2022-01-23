namespace FileCabinetApp.CommandHandlers;
internal class ExportCommandHandler : ServiceCommandHandlerBase
{
    public ExportCommandHandler(FileCabinetTrasferHelper service)
        : base(service)
    {
    }

    protected override string[] CommandNames { get; } = { "export" };

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

        if (!fileType.Equals("csv", StringComparison.InvariantCultureIgnoreCase) &&
            !fileType.Equals("xml", StringComparison.InvariantCultureIgnoreCase))
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

            if (fileType.Equals("csv", StringComparison.InvariantCultureIgnoreCase))
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