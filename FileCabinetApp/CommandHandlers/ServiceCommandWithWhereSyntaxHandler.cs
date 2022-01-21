using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers;
public abstract class ServiceCommandWithWhereSyntaxHandlerBase : ServiceCommandHandlerBase
{
    protected const string IdWord = "id";
    protected const string FirstNameWord = "firstname";
    protected const string LastNameWord = "lastname";
    protected const string DateOfBirthWord = "dateofbirth";
    protected const string BalanceWord = "balance";
    protected const string WorkExperienceWord = "workexperience";
    protected const string FavLetterWord = "favletter";
    protected const string OpeningBrace = "(";
    protected const string ClosingBrace = ")";
    protected const string AndWord = "and";
    protected const string OrWord = "or";

    public ServiceCommandWithWhereSyntaxHandlerBase(FileCabinetTrasferHelper service)
        : base(service)
    {
    }

    protected IEnumerable<FileCabinetRecord> ParseWhereParameters(string parameters)
    {
        char[] symbols = { ',', ' ', '\'', '\"', '=' };
        var parametersList = parameters.Split(symbols, StringSplitOptions.RemoveEmptyEntries).ToList();

        var operations = new Stack<string>();
        var polandNotation = new List<string>();

        for (int i = 0; i < parametersList.Count; i++)
        {
            var lowerParameter = parametersList[i].ToLower();
            switch (lowerParameter)
            {
                case AndWord:
                case OrWord:
                case OpeningBrace:
                case ClosingBrace:
                    this.AddOperation(lowerParameter, operations, polandNotation);
                    break;

                default:
                    polandNotation.Add(string.Concat(lowerParameter, '=', parametersList[++i]));
                    break;
            }
        }

        while (operations.Count != 0)
        {
            polandNotation.Add(operations.Pop());
        }

        var results = new Stack<IEnumerable<FileCabinetRecord>>();
        results.Push(this.GetRecords(polandNotation[0].Split('=')));

        IEnumerable<FileCabinetRecord> first, second;
        var comparer = new FileCabinetRecordsComparer();

        for (int i = 1; i < polandNotation.Count; i++)
        {
            switch (polandNotation[i])
            {
                case AndWord:
                    first = results.Pop();
                    second = results.Pop();
                    results.Push(first.Intersect(second, comparer));
                    break;
                case OrWord:
                    first = results.Pop();
                    second = results.Pop();
                    results.Push(first.Concat(second).Distinct(comparer));
                    break;
                default:
                    results.Push(this.GetRecords(polandNotation[i].Split('=')));
                    break;
            }
        }

        return results.Pop().OrderBy(x => x.Id);
    }

    protected IEnumerable<FileCabinetRecord> GetRecords(string[] conditions)
    {
        return conditions[0] switch
        {
            IdWord => this.service.Service.FindById(conditions[1]),
            FirstNameWord => this.service.Service.FindByFirstName(conditions[1]),
            LastNameWord => this.service.Service.FindByLastName(conditions[1]),
            DateOfBirthWord => this.service.Service.FindByDateOfBirth(conditions[1]),
            BalanceWord => this.service.Service.FindByBalance(conditions[1]),
            WorkExperienceWord => this.service.Service.FindByWorkExperience(conditions[1]),
            FavLetterWord => this.service.Service.FindByFavLetter(conditions[1]),
            _ => Array.Empty<FileCabinetRecord>(),
        };
    }

    protected void AddOperation(string operation, Stack<string> operations, List<string> polandNotation)
    {
        switch (operation)
        {
            case AndWord:
                while (operations.Count != 0)
                {
                    string temp = operations.Pop();
                    if (temp == AndWord)
                    {
                        polandNotation.Add(temp);
                    }
                    else
                    {
                        operations.Push(temp);
                        break;
                    }
                }

                operations.Push(operation);
                break;
            case OrWord:
                while (operations.Count != 0)
                {
                    string temp = operations.Pop();
                    if (temp == AndWord || temp == OrWord)
                    {
                        polandNotation.Add(temp);
                    }
                    else
                    {
                        operations.Push(temp);
                        break;
                    }
                }

                operations.Push(operation);
                break;
            case ClosingBrace:
                while (operations.Count != 0)
                {
                    string temp = operations.Pop();
                    if (temp != OpeningBrace)
                    {
                        polandNotation.Add(temp);
                    }
                    else
                    {
                        break;
                    }
                }

                break;
            case OpeningBrace:
                operations.Push(operation);
                break;
        }
    }
}