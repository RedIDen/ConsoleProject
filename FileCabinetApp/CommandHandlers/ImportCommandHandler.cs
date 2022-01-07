using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers;
public class ImportCommandHandler : CommandHandlerBase
{
    protected override string CommandName { get; set; } = "import";

    public ImportCommandHandler(IFileCabinetService fileCabinetService)
    {
        this.fileCabinetService = fileCabinetService;
    }

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
            try
            {
                var fileStream = new FileStream(fileName, FileMode.Open);

                var snapshot = new FileCabinetServiceSnapshot();

                if (fileType.Equals("csv", StringComparison.InvariantCultureIgnoreCase))
                {
                    snapshot.LoadFromCsv(fileStream);
                }
                else
                {
                    snapshot.LoadFromXml(fileStream);
                }

                this.fileCabinetService.Restore(snapshot);

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
