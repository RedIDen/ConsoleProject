#pragma warning disable CA1822

namespace FileCabinetApp;

/// <summary>
/// The file cabinet service base.
/// </summary>
internal abstract class FileCabinetServiceBase : IFileCabinetService
{
    /// <summary>
    /// id.
    /// </summary>
    protected const string IdWord = "id";

    /// <summary>
    /// firstname.
    /// </summary>
    protected const string FirstNameWord = "firstname";

    /// <summary>
    /// lastname.
    /// </summary>
    protected const string LastNameWord = "lastname";

    /// <summary>
    /// dateofbirth.
    /// </summary>
    protected const string DateOfBirthWord = "dateofbirth";

    /// <summary>
    /// balance.
    /// </summary>
    protected const string BalanceWord = "balance";

    /// <summary>
    /// workexperience.
    /// </summary>
    protected const string WorkExperienceWord = "workexperience";

    /// <summary>
    /// favletter.
    /// </summary>
    protected const string FavLetterWord = "favletter";

    /// <summary>
    /// (.
    /// </summary>
    protected const string OpeningBrace = "(";

    /// <summary>
    /// ).
    /// </summary>
    protected const string ClosingBrace = ")";

    /// <summary>
    /// and.
    /// </summary>
    protected const string AndWord = "and";

    /// <summary>
    /// or.
    /// </summary>
    protected const string OrWord = "or";

    /// <summary>
    /// Gets or sets record validator.
    /// </summary>
    /// <value>
    /// IRecordValidator object.
    /// </value>
    public abstract IRecordValidator Validator { get; set; }

    /// <summary>
    /// Adds the record to the list.
    /// </summary>
    /// <param name="record">The record to add to the list.</param>
    /// <returns>Returns the id of the new record.</returns>
    public abstract int CreateRecord(FileCabinetRecord record);

    /// <summary>
    /// Edits the existing record.
    /// </summary>
    /// <param name="newRecordData">The new record data.</param>
    /// <param name="index">The index of the record to edit.</param>
    public abstract void EditRecord(FileCabinetRecord newRecordData, int index);

    /// <summary>
    /// Returns the readonly collection of all records.
    /// </summary>
    /// <returns>The readonly collection of all records.</returns>
    public abstract IEnumerable<FileCabinetRecord> GetRecords();

    /// <summary>
    /// Returns the stats (the number of records in the list).
    /// </summary>
    /// <returns>The number of records in the list and the number of deleted records.</returns>
    public abstract (int, int) GetStat();

    /// <summary>
    /// Creates the snapshot of the record list.
    /// </summary>
    /// <returns>The new snapshot object.</returns>
    public abstract FileCabinetServiceSnapshot MakeSnapshot();

    /// <summary>
    /// For this class does nothing.
    /// </summary>
    /// <returns>Zero, the number of all the records before purge.</returns>
    public abstract (int, int) Purge();

    /// <summary>
    /// Removes the existing record.
    /// </summary>
    /// <param name="index">The id of record to remove.</param>
    public abstract void RemoveRecord(int index);

    /// <summary>
    /// Resores the list from the snapshot.
    /// </summary>
    /// <param name="snapshot">THe snapshot to restore from.</param>
    public abstract void Restore(FileCabinetServiceSnapshot snapshot);

    /// <summary>
    /// Returnd the index of the record with the recieved id.
    /// </summary>
    /// <param name="id">The record's id.</param>
    /// <returns>The index in the list of rhe record with the recieved id.</returns>
    public abstract int FindRecordIndexById(int id);

    /// <summary>
    /// Returns the list of the records with recieved first name.
    /// </summary>
    /// <param name="firstName">First name.</param>
    /// <returns>The list of the records with recieved first name.</returns>
    public abstract IEnumerable<FileCabinetRecord> FindByFirstName(string firstName);

    /// <summary>
    /// Returns the list of the records with recieved last name.
    /// </summary>
    /// <param name="lastName">Last name.</param>
    /// <returns>The list of the records with recieved last name.</returns>
    public abstract IEnumerable<FileCabinetRecord> FindByLastName(string lastName);

    /// <summary>
    /// Returns the list of the records with recieved date of birth.
    /// </summary>
    /// <param name="date">Date of birth.</param>
    /// <returns>The list of the records with recieved date of birth.</returns>
    public abstract IEnumerable<FileCabinetRecord> FindByDateOfBirth(string date);

    /// <summary>
    /// Returns the list of the records with recieved balance.
    /// </summary>
    /// <param name="data">Balance.</param>
    /// <returns>The list of the records with recieved balance.</returns>
    public abstract IEnumerable<FileCabinetRecord> FindByBalance(string data);

    /// <summary>
    /// Returns the list of the records with recieved work experience.
    /// </summary>
    /// <param name="data">Work experience.</param>
    /// <returns>The list of the records with recieved work experience.</returns>
    public abstract IEnumerable<FileCabinetRecord> FindByWorkExperience(string data);

    /// <summary>
    /// Returns the list of the records with recieved favorite letter.
    /// </summary>
    /// <param name="data">Favorite letter.</param>
    /// <returns>The list of the records with recieved favorite letter.</returns>
    public abstract IEnumerable<FileCabinetRecord> FindByFavLetter(string data);

    /// <summary>
    /// Returns the list of the records with recieved id.
    /// </summary>
    /// <param name="data">Id.</param>
    /// <returns>The list of the records with recieved id.</returns>
    public abstract IEnumerable<FileCabinetRecord> FindById(string data);

    /// <summary>
    /// Returns the list of the records with recieved complex SQL-like 'where' condition.
    /// </summary>
    /// <param name="parameters">SQL-like 'where' condition.</param>
    /// <returns>The list of the records with recieved complex SQL-like 'where' condition.</returns>
    public virtual IEnumerable<FileCabinetRecord> Find(string parameters)
    {
        if (string.IsNullOrEmpty(parameters))
        {
            return Array.Empty<FileCabinetRecord>();
        }

        char[] symbols = { ',', ' ', '\'', '\"', '=' };
        var parametersList = parameters.Split(symbols, StringSplitOptions.RemoveEmptyEntries).ToList();

        var operations = new Stack<string>();
        var polandNotation = new List<string>();

        for (int i = 0; i < parametersList.Count; i++)
        {
            var lowerParameter = parametersList[i].ToLower(CultureInfo.InvariantCulture);
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
                    if (++i < parametersList.Count)
                    {
                        polandNotation.Add(string.Concat(lowerParameter, '=', parametersList[i]));
                        break;
                    }
                    else
                    {
                        return Array.Empty<FileCabinetRecord>();
                    }

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
                    if (!results.Any())
                    {
                        return Array.Empty<FileCabinetRecord>();
                    }

                    second = results.Pop();
                    results.Push(first.Intersect(second, comparer));
                    break;
                case OrWord:
                    first = results.Pop();
                    if (!results.Any())
                    {
                        return Array.Empty<FileCabinetRecord>();
                    }

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

    /// <summary>
    /// Returns the list of the records with recieved part of SQL-like 'where' condition.
    /// </summary>
    /// <param name="conditions">Parameter name and parameter value.</param>
    /// <returns>The list of the records with recieved part of SQL-like 'where' condition.</returns>
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

    /// <summary>
    /// Adds operation to the stack of operations for Poland notation.
    /// </summary>
    /// <param name="operation">Operation.</param>
    /// <param name="operations">Stack of operations.</param>
    /// <param name="polandNotation">List of operations in Poland notation.</param>
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