using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

#pragma warning disable CS8602
#pragma warning disable CS8604

namespace FileCabinetApp;

/// <summary>
/// The main class of the program.
/// </summary>
public static class Program
{
    private static int startId;
    private static int recordsAmount;
    private static string fileName;
    private static string format;

    /// <summary>
    /// The main method of the program.
    /// </summary>
    /// <param name="args">Extra arguments to run the application.</param>
    public static void Main(string[] args)
    {
        do
        {
            Console.Write("> ");
            var inputs = Console.ReadLine().Split(new char[] { ' ', '=' });

            if (inputs[0] == "exit")
            {
                Console.WriteLine("Exiting an application...");
                break;
            }

            if (inputs.Length / 2 != 0)
            {
                Console.WriteLine("Wrong command syntax!");
                continue;
            }

            for (int i = 0; i < inputs.Length; i += 2)
            {
                switch (inputs[i].ToLower())
                {
                    case "--output-type":
                    case "-t":
                        Program.OutputType(inputs[i + 1]);
                        break;
                    case "--output":
                    case "-o":
                        Program.Output(inputs[i + 1]);
                        break;
                    case "--records-amount":
                    case "-a":
                        Program.RecordsAmount(inputs[i + 1]);
                        break;
                    case "--start-id":
                    case "-i":
                        Program.StartId(inputs[i + 1]);
                        break;
                }
            }
        }
        while (true);
    }

    private static void StartId(string parameter) => Program.startId = int.Parse(parameter);

    private static void RecordsAmount(string parameter) => Program.recordsAmount = int.Parse(parameter);

    private static void Output(string parameter) => Program.fileName = parameter;

    private static void OutputType(string parameter) => Program.format = parameter;
}