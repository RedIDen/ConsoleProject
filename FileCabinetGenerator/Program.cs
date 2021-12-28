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

    /// <summary>
    /// The main method of the program.
    /// </summary>
    /// <param name="args">Extra arguments to run the application.</param>
    //public static void Main(string[] args)
    //{
    //    WriteGreeting();
    //    do
    //    {
    //        Console.Write("> ");
    //        var inputs = Console.ReadLine().Split(new char[] { ' ', '=' }, 2);
    //        const int commandIndex = 0;
    //        var command = inputs[commandIndex];

    //        if (string.IsNullOrEmpty(command))
    //        {
    //            Console.WriteLine(Program.HintMessage);
    //            continue;
    //        }

    //        var index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(command, StringComparison.InvariantCultureIgnoreCase));
    //        if (index >= 0)
    //        {
    //            const int parametersIndex = 1;
    //            var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
    //            commands[index].Item2(parameters);
    //        }
    //        else
    //        {
    //            PrintMissedCommandInfo(command);
    //        }
    //    }
    //    while (isRunning);
    //}
}