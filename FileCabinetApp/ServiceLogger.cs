#pragma warning disable CA1822

namespace FileCabinetApp;

/// <summary>
/// The service logger.
/// </summary>
internal class ServiceLogger : FileCabinetServiceBase, IServiceDecorator
{
    /// <summary>
    /// The file to save logs to.
    /// </summary>
    public const string FileName = "log.txt";

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceLogger"/> class.
    /// </summary>
    /// <param name="service">Service.</param>
    public ServiceLogger(FileCabinetServiceBase service)
    {
        this.Service = service;
    }

    /// <summary>
    /// Gets or sets the service.
    /// </summary>
    /// <value>
    /// FileCabinetServiceBase object.
    /// </value>
    public FileCabinetServiceBase Service { get; set; }

    /// <summary>
    /// Gets or sets record validator.
    /// </summary>
    /// <value>
    /// IRecordValidator object.
    /// </value>
    public override IRecordValidator Validator
    {
        get => this.Service.Validator;
        set => this.Service.Validator = value;
    }

    /// <summary>
    /// Adds the record to the list.
    /// </summary>
    /// <param name="record">The record to add to the list.</param>
    /// <returns>Returns the id of the new record.</returns>
    public override int CreateRecord(FileCabinetRecord record)
    {
        this.WriteLog($"Calling Create() with FirstName = '{record.FirstName}', LastName = '{record.LastName}', DateOfBirth = '{record.DateOfBirth:MM/dd/yyyy}', Balance = '{record.Balance}', WorkExperience = '{record.WorkExperience}', FavLetter = '{record.FavLetter}'");
        var result = this.Service.CreateRecord(record);
        this.WriteLog($"Create() returned '{result}'");
        return result;
    }

    /// <summary>
    /// Edits the existing record.
    /// </summary>
    /// <param name="newRecordData">The new record data.</param>
    /// <param name="index">The index of the record to edit.</param>
    public override void EditRecord(FileCabinetRecord newRecordData, int index)
    {
        this.WriteLog($"Calling Edit() for Index = '{index}' with FirstName = '{newRecordData.FirstName}', LastName = '{newRecordData.LastName}', DateOfBirth = '{newRecordData.DateOfBirth:MM/dd/yyyy}', Balance = '{newRecordData.Balance}', WorkExperience = '{newRecordData.WorkExperience}', FavLetter = '{newRecordData.FavLetter}'");
        this.Service.EditRecord(newRecordData, index);
    }

    /// <summary>
    /// Returns the list of the records with recieved date of birth.
    /// </summary>
    /// <param name="date">Date of birth.</param>
    /// <returns>The list of the records with recieved date of birth.</returns>
    public override IEnumerable<FileCabinetRecord> FindByDateOfBirth(string date)
    {
        this.WriteLog($"Calling FindByDateOfBirth() with Date = '{date}'");
        var result = this.Service.FindByDateOfBirth(date);
        this.WriteLog($"FindByDateOfBirth() returned list with Size = '{result.Count()}'");
        return result;
    }

    /// <summary>
    /// Returns the list of the records with recieved first name.
    /// </summary>
    /// <param name="firstName">First name.</param>
    /// <returns>The list of the records with recieved first name.</returns>
    public override IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
    {
        this.WriteLog($"Calling FindByFirstName() with Date = '{firstName}'");
        var result = this.Service.FindByFirstName(firstName);
        this.WriteLog($"FindByFirstName() returned list with Size = '{result.Count()}'");
        return result;
    }

    /// <summary>
    /// Returns the list of the records with recieved last name.
    /// </summary>
    /// <param name="lastName">Last name.</param>
    /// <returns>The list of the records with recieved last name.</returns>
    public override IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
    {
        this.WriteLog($"Calling FindByLastName() with Date = '{lastName}'");
        var result = this.Service.FindByLastName(lastName);
        this.WriteLog($"FindByLastName() returned list with Size = '{result.Count()}'");
        return result;
    }

    /// <summary>
    /// Returns the list of the records with recieved balance.
    /// </summary>
    /// <param name="data">Balance.</param>
    /// <returns>The list of the records with recieved balance.</returns>
    public override IEnumerable<FileCabinetRecord> FindByBalance(string data)
    {
        this.WriteLog($"Calling FindByBalance() with Date = '{data}'");
        var result = this.Service.FindByBalance(data);
        this.WriteLog($"FindByBalance() returned list with Size = '{result.Count()}'");
        return result;
    }

    /// <summary>
    /// Returns the list of the records with recieved work experience.
    /// </summary>
    /// <param name="data">Work experience.</param>
    /// <returns>The list of the records with recieved work experience.</returns>
    public override IEnumerable<FileCabinetRecord> FindByWorkExperience(string data)
    {
        this.WriteLog($"Calling FindByWorkExperience() with Date = '{data}'");
        var result = this.Service.FindByWorkExperience(data);
        this.WriteLog($"FindByWorkExperience() returned list with Size = '{result.Count()}'");
        return result;
    }

    /// <summary>
    /// Returns the list of the records with recieved favorite letter.
    /// </summary>
    /// <param name="data">Favorite letter.</param>
    /// <returns>The list of the records with recieved favorite letter.</returns>
    public override IEnumerable<FileCabinetRecord> FindByFavLetter(string data)
    {
        this.WriteLog($"Calling FindByFavLetter() with Date = '{data}'");
        var result = this.Service.FindByFavLetter(data);
        this.WriteLog($"FindByFavLetter() returned list with Size = '{result.Count()}'");
        return result;
    }

    /// <summary>
    /// Returns the list of the records with recieved id.
    /// </summary>
    /// <param name="data">Id.</param>
    /// <returns>The list of the records with recieved id.</returns>
    public override IEnumerable<FileCabinetRecord> FindById(string data)
    {
        this.WriteLog($"Calling FindById() with Date = '{data}'");
        var result = this.Service.FindById(data);
        this.WriteLog($"FindById() returned list with Size = '{result.Count()}'");
        return result;
    }

    /// <summary>
    /// Returnd the index of the record with the recieved id.
    /// </summary>
    /// <param name="id">The record's id.</param>
    /// <returns>The index in the list of rhe record with the recieved id.</returns>
    public override int FindRecordIndexById(int id)
    {
        return this.Service.FindRecordIndexById(id);
    }

    /// <summary>
    /// Returns the list of the records with recieved complex SQL-like 'where' condition.
    /// </summary>
    /// <param name="parameters">SQL-like 'where' condition.</param>
    /// <returns>The list of the records with recieved complex SQL-like 'where' condition.</returns>
    public override IEnumerable<FileCabinetRecord> Find(string parameters)
    {
        this.WriteLog($"Calling Find() with Parameters = '{parameters}'");
        var result = this.Service.Find(parameters);
        this.WriteLog($"Find() returned list with Size = '{result.Count()}'");
        return result;
    }

    /// <summary>
    /// Returns the readonly collection of all records.
    /// </summary>
    /// <returns>The readonly collection of all records.</returns>
    public override IEnumerable<FileCabinetRecord> GetRecords()
    {
        this.WriteLog($"Calling GetRecords()");
        var result = this.Service.GetRecords();
        this.WriteLog($"GetRecords() returned list with Size = '{result.Count()}'");
        return result;
    }

    /// <summary>
    /// Returns the stats (the number of records in the list).
    /// </summary>
    /// <returns>The number of records in the list and the number of deleted records.</returns>
    public override (int, int) GetStat()
    {
        this.WriteLog($"Calling GetStat()");
        var result = this.Service.GetStat();
        this.WriteLog($"GetStat() returned '{result.Item1}', '{result.Item2}'");
        return result;
    }

    /// <summary>
    /// Creates the snapshot of the record list.
    /// </summary>
    /// <returns>The new snapshot object.</returns>
    public override FileCabinetServiceSnapshot MakeSnapshot()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// For this class does nothing.
    /// </summary>
    /// <returns>Zero, the number of all the records before purge.</returns>
    public override (int, int) Purge()
    {
        this.WriteLog($"Calling Purge()");
        var result = this.Service.Purge();
        this.WriteLog($"Purge() returned '{result.Item1}', '{result.Item2}'");
        return result;
    }

    /// <summary>
    /// Removes the existing record.
    /// </summary>
    /// <param name="index">The id of record to remove.</param>
    public override void RemoveRecord(int index)
    {
        this.WriteLog($"Calling RemoveRecord() with Id = '{index}'");
        this.Service.RemoveRecord(index);
    }

    /// <summary>
    /// Resores the list from the snapshot.
    /// </summary>
    /// <param name="snapshot">THe snapshot to restore from.</param>
    public override void Restore(FileCabinetServiceSnapshot snapshot)
    {
        throw new NotImplementedException();
    }

    private void WriteLog(string log)
    {
        using (var writer = new StreamWriter(path: ServiceLogger.FileName, append: true))
        {
            writer.Write(DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture));
            writer.Write(" - ");
            writer.WriteLine(log);
        }
    }
}