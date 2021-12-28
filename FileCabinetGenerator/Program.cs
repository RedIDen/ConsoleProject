#pragma warning disable CS8602
#pragma warning disable CS8618

using FileCabinetApp;

namespace FileCabinetGeneretor;

/// <summary>
/// The main class of the program.
/// </summary>
public static class Program
{
    private static int startId;
    private static int recordsAmount;
    private static string fileName;
    private static string format;

    private const string testCommand = "-t csv -o a.csv -a 1000 -i 1";

    private static readonly string[] firstNames = {
        "Bob", "Fred", "Leon", "Martha", "Egor",
        "Denis", "Oleg", "George", "Konstantin", "Vladislav",
        "Anastasia", "Vadim", "Bred", "Mikola", "Natalie",
        "Olga", "Victor", "Kate", "Irina", "Julia",
        "Leonardo", "Ryan", "John", "Korey", "Lena",
    };

    private static readonly string[] lastNames = {
        "Smith", "Vasilko", "Bebro", "Brown", "Dark",
        "Whire", "Green", "Namazko", "Electro", "Zvezdochet",
        "Gorlyshko", "Bondarchuk", "Nevermind", "Starkiller", "Goodnight",
        "Taker", "Partymaker", "Blanchet", "DiCaprio", "Pitt",
        "Gosling", "Kobain", "Bob", "Magistral", "Kenobi",
    };

    /// <summary>
    /// The main method of the program.
    /// </summary>
    /// <param name="args">Extra arguments to run the application.</param>
    public static void Main(string[] args)
    {
        do
        {
            Console.Write("> ");
            //var inputs = Console.ReadLine().Split(new char[] { ' ', '=' });
            var inputs = Program.testCommand.Split(new char[] { ' ', '=' });

            if (inputs[0] == "exit")
            {
                Console.WriteLine("Exiting an application...");
                break;
            }

            if (inputs.Length != 8)
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

            switch (Program.format.ToLower())
            {
                case "xml":
                    Program.WriteXml();
                    break;

                case "csv":
                    Program.WriteCsv();
                    break;
            }
        }
        while (false);

        Console.ReadKey();
    }

    private static void WriteCsv()
    {
        Random random = new Random();
        using (var streamWriter = new StreamWriter(Program.fileName))
        {
            var csvWriter = new FileCabinetRecordCsvWriter(streamWriter);

            int endId = Program.startId + Program.recordsAmount;
            for (int i = Program.startId; i < endId; i++)
            {
                csvWriter.Write(new FileCabinetRecord
                {
                    Id = i,
                    FirstName = Program.firstNames[random.Next(Program.firstNames.Length)],
                    LastName = Program.lastNames[random.Next(Program.lastNames.Length)],
                    DateOfBirth = new DateTime(1950, 1, 1).AddDays(random.Next(20_000)),
                    Balance = random.Next(10),
                    WorkExperience = (short)random.Next(5),
                    FavLetter = (char)('a' + random.Next(26)),
                });
            }

            csvWriter.Close();
        }

        Console.WriteLine($"{Program.recordsAmount} records were written to {Program.fileName}");
    }

    private static void WriteXml()
    {
        throw new NotImplementedException();
    }

    private static void StartId(string parameter) => Program.startId = int.Parse(parameter);

    private static void RecordsAmount(string parameter) => Program.recordsAmount = int.Parse(parameter);

    private static void Output(string parameter) => Program.fileName = parameter;

    private static void OutputType(string parameter) => Program.format = parameter;
}