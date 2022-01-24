#pragma warning disable CS8602

namespace FileCabinetApp.FileCabinet;

/// <summary>
/// The File Cabinet Memory Service class.
/// </summary>
internal class FileCabinetMemoryService : FileCabinetServiceBase
{
    private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
    private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
    private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
    private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();
    private readonly Dictionary<decimal, List<FileCabinetRecord>> balanceDictionary = new Dictionary<decimal, List<FileCabinetRecord>>();
    private readonly Dictionary<short, List<FileCabinetRecord>> workExperienceDictionary = new Dictionary<short, List<FileCabinetRecord>>();
    private readonly Dictionary<char, List<FileCabinetRecord>> favLetterDictionary = new Dictionary<char, List<FileCabinetRecord>>();

    private readonly Dictionary<string, IEnumerable<FileCabinetRecord>> memoizedConditions = new Dictionary<string, IEnumerable<FileCabinetRecord>>();

    /// <summary>
    /// Initializes a new instance of the <see cref="FileCabinetMemoryService"/> class.
    /// </summary>
    /// <param name="recordValidator">Record validator.</param>
    public FileCabinetMemoryService(IRecordValidator recordValidator)
    {
        this.Validator = recordValidator;
    }

    /// <summary>
    /// Gets or sets the record validator.
    /// </summary>
    /// <value>The object of the class realizing the IRecordValidator interface.</value>
    public override IRecordValidator Validator { get; set; }

    /// <summary>
    /// Adds the record to the list.
    /// </summary>
    /// <param name="record">The record to add to the list.</param>
    /// <returns>Returns the id of the new record.</returns>
    public override int CreateRecord(FileCabinetRecord record)
    {
        if (record.Id == 0)
        {
            record.Id = this.list.Count == 0 ? 1 : this.list.Max(x => x.Id) + 1;
        }

        this.AddToDictionaries(record);

        this.list.Add(record);

        return record.Id;
    }

    /// <summary>
    /// Edits the existing record.
    /// </summary>
    /// <param name="newRecordData">The new record data.</param>
    /// <param name="index">The index of the record to edit.</param>
    public override void EditRecord(FileCabinetRecord newRecordData, int index)
    {
        var record = this.list[index];
        this.DeleteFromDictionaries(this.list[index]);

        if (!string.IsNullOrEmpty(newRecordData.FirstName))
        {
            record.FirstName = newRecordData.FirstName;
        }

        if (!string.IsNullOrEmpty(newRecordData.LastName))
        {
            record.LastName = newRecordData.LastName;
        }

        if (newRecordData.DateOfBirth != new DateTime(0))
        {
            record.DateOfBirth = newRecordData.DateOfBirth;
        }

        if (newRecordData.Balance != -1)
        {
            record.Balance = newRecordData.Balance;
        }

        if (newRecordData.WorkExperience != -1)
        {
            record.WorkExperience = newRecordData.WorkExperience;
        }

        if (newRecordData.FavLetter != '\0')
        {
            record.FavLetter = newRecordData.FavLetter;
        }

        this.AddToDictionaries(record);
        this.memoizedConditions.Clear();
    }

    /// <summary>
    /// Returnd the index of the record with the recieved id.
    /// </summary>
    /// <param name="id">The record's id.</param>
    /// <returns>The index in the list of rhe record with the recieved id.</returns>
    public override int FindRecordIndexById(int id) => this.list.FindIndex(e => e.Id == id);

    /// <summary>
    /// Returns the readonly collection of all records.
    /// </summary>
    /// <returns>The readonly collection of all records.</returns>
    public override IEnumerable<FileCabinetRecord> GetRecords() => this.list;

    /// <summary>
    /// Returns the stats (the number of records in the list).
    /// </summary>
    /// <returns>The number of records in the list and the number of deleted records.</returns>
    public override (int, int) GetStat() => (this.list.Count, 0);

    /// <summary>
    /// Returns the list of the records with recieved first name.
    /// </summary>
    /// <param name="firstName">First name.</param>
    /// <returns>The list of the records with recieved first name.</returns>
    public override IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
    {
        var list = this.firstNameDictionary.GetValueOrDefault(firstName.ToLower(CultureInfo.InvariantCulture));
        return list == null ? Array.Empty<FileCabinetRecord>() : new MemoryEnumerator<FileCabinetRecord>(list);
    }

    /// <summary>
    /// Returns the list of the records with recieved last name.
    /// </summary>
    /// <param name="lastName">Last name.</param>
    /// <returns>The list of the records with recieved last name.</returns>
    public override IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
    {
        var list = this.lastNameDictionary.GetValueOrDefault(lastName.ToLower(CultureInfo.InvariantCulture));
        return list == null ? Array.Empty<FileCabinetRecord>() : new MemoryEnumerator<FileCabinetRecord>(list);
    }

    /// <summary>
    /// Returns the list of the records with recieved date of birth.
    /// </summary>
    /// <param name="data">Date of birth.</param>
    /// <returns>The list of the records with recieved date of birth.</returns>
    public override IEnumerable<FileCabinetRecord> FindByDateOfBirth(string data)
    {
        var parseResult = DateTime.TryParse(
            data,
            CultureInfo.CreateSpecificCulture("en-US"),
            DateTimeStyles.None,
            out DateTime dateOfBirth);

        if (parseResult)
        {
            var list = this.dateOfBirthDictionary.GetValueOrDefault(dateOfBirth);
            return list == null ? Array.Empty<FileCabinetRecord>() : new MemoryEnumerator<FileCabinetRecord>(list);
        }

        return Array.Empty<FileCabinetRecord>();
    }

    /// <summary>
    /// Returns the list of the records with recieved balance.
    /// </summary>
    /// <param name="data">Balance.</param>
    /// <returns>The list of the records with recieved balance.</returns>
    public override IEnumerable<FileCabinetRecord> FindByBalance(string data)
    {
        var parseResult = decimal.TryParse(
            data,
            out decimal balance);

        if (parseResult)
        {
            var list = this.balanceDictionary.GetValueOrDefault(balance);
            return list == null ? Array.Empty<FileCabinetRecord>() : new MemoryEnumerator<FileCabinetRecord>(list);
        }

        return Array.Empty<FileCabinetRecord>();
    }

    /// <summary>
    /// Returns the list of the records with recieved work experience.
    /// </summary>
    /// <param name="data">Work experience.</param>
    /// <returns>The list of the records with recieved work experience.</returns>
    public override IEnumerable<FileCabinetRecord> FindByWorkExperience(string data)
    {
        var parseResult = short.TryParse(
            data,
            out short workExperience);

        if (parseResult)
        {
            var list = this.workExperienceDictionary.GetValueOrDefault(workExperience);
            return list == null ? Array.Empty<FileCabinetRecord>() : new MemoryEnumerator<FileCabinetRecord>(list);
        }

        return Array.Empty<FileCabinetRecord>();
    }

    /// <summary>
    /// Returns the list of the records with recieved favorite letter.
    /// </summary>
    /// <param name="data">Favorite letter.</param>
    /// <returns>The list of the records with recieved favorite letter.</returns>
    public override IEnumerable<FileCabinetRecord> FindByFavLetter(string data)
    {
        var parseResult = char.TryParse(
            data,
            out char favLetter);

        if (parseResult)
        {
            var list = this.favLetterDictionary.GetValueOrDefault(favLetter);
            return list == null ? Array.Empty<FileCabinetRecord>() : new MemoryEnumerator<FileCabinetRecord>(list);
        }

        return Array.Empty<FileCabinetRecord>();
    }

    /// <summary>
    /// Returns the list of the records with recieved id.
    /// </summary>
    /// <param name="data">Id.</param>
    /// <returns>The list of the records with recieved id.</returns>
    public override IEnumerable<FileCabinetRecord> FindById(string data)
    {
        return int.TryParse(data, out int id) ? new MemoryEnumerator<FileCabinetRecord>(this.list.Where(x => x.Id == id)) : Array.Empty<FileCabinetRecord>();
    }

    /// <summary>
    /// Creates the snapshot of the record list.
    /// </summary>
    /// <returns>The new snapshot object.</returns>
    public override FileCabinetServiceSnapshot MakeSnapshot() => new FileCabinetServiceSnapshot(this.list);

    /// <summary>
    /// Resores the list from the snapshot.
    /// </summary>
    /// <param name="snapshot">THe snapshot to restore from.</param>
    public override void Restore(FileCabinetServiceSnapshot snapshot)
    {
        var importList = snapshot.GetRecords();

        foreach (var record in importList)
        {
            (bool result, string message) = this.Validator.Validate(record);

            if (result)
            {
                int index = this.FindRecordIndexById(record.Id);
                if (index == -1)
                {
                    this.list.Add(record);
                    this.AddToDictionaries(record);
                }
                else
                {
                    this.EditRecord(record, index);
                }
            }
            else
            {
                Console.WriteLine($"Can't read record #{record.Id}");
                Console.WriteLine(message);
            }
        }
    }

    /// <summary>
    /// Removes the existing record.
    /// </summary>
    /// <param name="index">The id of record to remove.</param>
    public override void RemoveRecord(int index)
    {
        var record = this.list[index];
        this.DeleteFromDictionaries(record);
        this.list.Remove(record);
        this.memoizedConditions.Clear();
    }

    /// <summary>
    /// For this class does nothing.
    /// </summary>
    /// <returns>Zero, the number of all the records before purge.</returns>
    public override (int, int) Purge()
    {
        return this.GetStat();
    }

    /// <summary>
    /// Returns the list of the records with recieved complex SQL-like 'where' condition.
    /// </summary>
    /// <param name="parameters">SQL-like 'where' condition.</param>
    /// <returns>The list of the records with recieved complex SQL-like 'where' condition.</returns>
    public override IEnumerable<FileCabinetRecord> Find(string parameters)
    {
        var found = this.memoizedConditions.GetValueOrDefault(parameters);
        if (found is not null)
        {
            return found;
        }

        var result = base.Find(parameters);

        this.memoizedConditions.Add(parameters, result);

        return result;
    }

    /// <summary>
    /// Adds new record to dictionaries.
    /// </summary>
    /// <param name="record">The record to add.</param>
    private void AddToDictionaries(FileCabinetRecord record)
    {
        string lowerFirstName = record.FirstName.ToLower(CultureInfo.InvariantCulture);

        if (this.firstNameDictionary.ContainsKey(lowerFirstName))
        {
            this.firstNameDictionary.GetValueOrDefault(lowerFirstName).Add(record);
        }
        else
        {
            var list = new List<FileCabinetRecord>();
            list.Add(record);
            this.firstNameDictionary.Add(lowerFirstName, list);
        }

        string lowerLastName = record.LastName.ToLower(CultureInfo.InvariantCulture);

        if (this.lastNameDictionary.ContainsKey(lowerLastName))
        {
            this.lastNameDictionary.GetValueOrDefault(lowerLastName).Add(record);
        }
        else
        {
            var list = new List<FileCabinetRecord>();
            list.Add(record);
            this.lastNameDictionary.Add(lowerLastName, list);
        }

        if (this.dateOfBirthDictionary.ContainsKey(record.DateOfBirth))
        {
            this.dateOfBirthDictionary.GetValueOrDefault(record.DateOfBirth).Add(record);
        }
        else
        {
            var list = new List<FileCabinetRecord>();
            list.Add(record);
            this.dateOfBirthDictionary.Add(record.DateOfBirth, list);
        }

        if (this.balanceDictionary.ContainsKey(record.Balance))
        {
            this.balanceDictionary.GetValueOrDefault(record.Balance).Add(record);
        }
        else
        {
            var list = new List<FileCabinetRecord>();
            list.Add(record);
            this.balanceDictionary.Add(record.Balance, list);
        }

        if (this.workExperienceDictionary.ContainsKey(record.WorkExperience))
        {
            this.workExperienceDictionary.GetValueOrDefault(record.WorkExperience).Add(record);
        }
        else
        {
            var list = new List<FileCabinetRecord>();
            list.Add(record);
            this.workExperienceDictionary.Add(record.WorkExperience, list);
        }

        if (this.favLetterDictionary.ContainsKey(record.FavLetter))
        {
            this.favLetterDictionary.GetValueOrDefault(record.FavLetter).Add(record);
        }
        else
        {
            var list = new List<FileCabinetRecord>();
            list.Add(record);
            this.favLetterDictionary.Add(record.FavLetter, list);
        }
    }

    /// <summary>
    /// Deletes the record to dicionaries.
    /// </summary>
    /// <param name="record">The record to delete.</param>
    private void DeleteFromDictionaries(FileCabinetRecord record)
    {
        string lowerFirstName = record.FirstName.ToLower(CultureInfo.InvariantCulture);
        this.firstNameDictionary.GetValueOrDefault(lowerFirstName).Remove(record);

        string lowerLastName = record.LastName.ToLower(CultureInfo.InvariantCulture);
        this.lastNameDictionary.GetValueOrDefault(lowerLastName).Remove(record);

        this.dateOfBirthDictionary.GetValueOrDefault(record.DateOfBirth).Remove(record);

        this.balanceDictionary.GetValueOrDefault(record.Balance).Remove(record);

        this.workExperienceDictionary.GetValueOrDefault(record.WorkExperience).Remove(record);

        this.favLetterDictionary.GetValueOrDefault(record.FavLetter).Remove(record);
    }
}