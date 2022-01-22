using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public abstract class FileCabinetServiceBase : IFileCabinetService
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

        public abstract IRecordValidator Validator { get; set; }

        public abstract int CreateRecord(FileCabinetRecord record);

        public abstract void EditRecord(FileCabinetRecord record, int index);

        public abstract IEnumerable<FileCabinetRecord> GetRecords();

        public abstract (int, int) GetStat();

        public abstract FileCabinetServiceSnapshot MakeSnapshot();

        public abstract (int, int) Purge();

        public abstract void RemoveRecord(int id);

        public abstract void Restore(FileCabinetServiceSnapshot snapshot);

        public abstract int FindRecordIndexById(int id);

        public abstract IEnumerable<FileCabinetRecord> FindByFirstName(string firstName);

        public abstract IEnumerable<FileCabinetRecord> FindByLastName(string lastName);

        public abstract IEnumerable<FileCabinetRecord> FindByDateOfBirth(string date);

        public abstract IEnumerable<FileCabinetRecord> FindByBalance(string data);

        public abstract IEnumerable<FileCabinetRecord> FindByWorkExperience(string data);

        public abstract IEnumerable<FileCabinetRecord> FindByFavLetter(string data);

        public abstract IEnumerable<FileCabinetRecord> FindById(string data);

        public virtual IEnumerable<FileCabinetRecord> Find(string parameters)
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
                    case IdWord:
                    case FirstNameWord:
                    case LastNameWord:
                    case DateOfBirthWord:
                    case BalanceWord:
                    case WorkExperienceWord:
                    case FavLetterWord:
                        polandNotation.Add(string.Concat(lowerParameter, '=', parametersList[++i]));
                        break;
                    default:
                        return Array.Empty<FileCabinetRecord>();
                }
            }

            while (operations.Count != 0)
            {
                polandNotation.Add(operations.Pop());
            }

            var results = new Stack<IEnumerable<FileCabinetRecord>>();
            results.Push(this.FindRecords(polandNotation[0].Split('=')));

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
                        results.Push(this.FindRecords(polandNotation[i].Split('=')));
                        break;
                }
            }

            var result = results.Pop().OrderBy(x => x.Id);

            return result;
        }

        protected IEnumerable<FileCabinetRecord> FindRecords(string[] conditions)
        {
            return conditions[0] switch
            {
                IdWord => this.FindById(conditions[1]),
                FirstNameWord => this.FindByFirstName(conditions[1]),
                LastNameWord => this.FindByLastName(conditions[1]),
                DateOfBirthWord => this.FindByDateOfBirth(conditions[1]),
                BalanceWord => this.FindByBalance(conditions[1]),
                WorkExperienceWord => this.FindByWorkExperience(conditions[1]),
                FavLetterWord => this.FindByFavLetter(conditions[1]),
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
}
