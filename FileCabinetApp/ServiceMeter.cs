using System.Diagnostics;

namespace FileCabinetApp;

/// <summary>
/// The service meter.
/// </summary>
internal class ServiceMeter : FileCabinetServiceBase, IServiceDecorator
{
    private readonly Stopwatch stopwatch = new Stopwatch();

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceMeter"/> class.
    /// </summary>
    /// <param name="service">Service.</param>
    public ServiceMeter(FileCabinetServiceBase service)
    {
        this.Service = service;
    }

    /// <summary>
    /// Gets or sets record validator.
    /// </summary>
    /// <value>
    /// IRecordValidator object.
    /// </value>
    public override IRecordValidator Validator
    {
        get => this.Service.Validator;
        set
        {
            this.StartStopwatch();
            this.Service.Validator = value;
            this.StopStopwatch("Validator");
        }
    }

    /// <summary>
    /// Gets or sets the service.
    /// </summary>
    /// <value>
    /// FileCabinetServiceBase object.
    /// </value>
    public FileCabinetServiceBase Service { get; set; }

    /// <summary>
    /// Adds the record to the list.
    /// </summary>
    /// <param name="record">The record to add to the list.</param>
    /// <returns>Returns the id of the new record.</returns>
    public override int CreateRecord(FileCabinetRecord record)
    {
        this.StartStopwatch();
        var result = this.Service.CreateRecord(record);
        this.StopStopwatch("Create");
        return result;
    }

    /// <summary>
    /// Edits the existing record.
    /// </summary>
    /// <param name="newRecordData">The new record data.</param>
    /// <param name="index">The index of the record to edit.</param>
    public override void EditRecord(FileCabinetRecord newRecordData, int index)
    {
        this.StartStopwatch();
        this.Service.EditRecord(newRecordData, index);
        this.StopStopwatch("Edit");
    }

    /// <summary>
    /// Returns the list of the records with recieved date of birth.
    /// </summary>
    /// <param name="date">Date of birth.</param>
    /// <returns>The list of the records with recieved date of birth.</returns>
    public override IEnumerable<FileCabinetRecord> FindByDateOfBirth(string date)
    {
        this.StartStopwatch();
        var result = this.Service.FindByDateOfBirth(date);
        this.StopStopwatch("Find dateofbirth");
        return result;
    }

    /// <summary>
    /// Returns the list of the records with recieved first name.
    /// </summary>
    /// <param name="firstName">First name.</param>
    /// <returns>The list of the records with recieved first name.</returns>
    public override IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
    {
        this.StartStopwatch();
        var result = this.Service.FindByFirstName(firstName);
        this.StopStopwatch("Find firstname");
        return result;
    }

    /// <summary>
    /// Returns the list of the records with recieved last name.
    /// </summary>
    /// <param name="lastName">Last name.</param>
    /// <returns>The list of the records with recieved last name.</returns>
    public override IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
    {
        this.StartStopwatch();
        var result = this.Service.FindByLastName(lastName);
        this.StopStopwatch("Find lastname");
        return result;
    }

    /// <summary>
    /// Returns the list of the records with recieved balance.
    /// </summary>
    /// <param name="data">Balance.</param>
    /// <returns>The list of the records with recieved balance.</returns>
    public override IEnumerable<FileCabinetRecord> FindByBalance(string data)
    {
        this.StartStopwatch();
        var result = this.Service.FindByBalance(data);
        this.StopStopwatch("Find balance");
        return result;
    }

    /// <summary>
    /// Returns the list of the records with recieved work experience.
    /// </summary>
    /// <param name="data">Work experience.</param>
    /// <returns>The list of the records with recieved work experience.</returns>
    public override IEnumerable<FileCabinetRecord> FindByWorkExperience(string data)
    {
        this.StartStopwatch();
        var result = this.Service.FindByWorkExperience(data);
        this.StopStopwatch("Find lastname");
        return result;
    }

    /// <summary>
    /// Returns the list of the records with recieved favorite letter.
    /// </summary>
    /// <param name="data">Favorite letter.</param>
    /// <returns>The list of the records with recieved favorite letter.</returns>
    public override IEnumerable<FileCabinetRecord> FindByFavLetter(string data)
    {
        this.StartStopwatch();
        var result = this.Service.FindByFavLetter(data);
        this.StopStopwatch("Find lastname");
        return result;
    }

    /// <summary>
    /// Returns the list of the records with recieved id.
    /// </summary>
    /// <param name="data">Id.</param>
    /// <returns>The list of the records with recieved id.</returns>
    public override IEnumerable<FileCabinetRecord> FindById(string data)
    {
        this.StartStopwatch();
        var result = this.Service.FindById(data);
        this.StopStopwatch("Find lastname");
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
        this.StartStopwatch();
        var result = this.Service.Find(parameters);
        this.StopStopwatch("Find");
        return result;
    }

    /// <summary>
    /// Returns the readonly collection of all records.
    /// </summary>
    /// <returns>The readonly collection of all records.</returns>
    public override IEnumerable<FileCabinetRecord> GetRecords()
    {
        this.StartStopwatch();
        var result = this.Service.GetRecords();
        this.StopStopwatch("List");
        return result;
    }

    /// <summary>
    /// Returns the stats (the number of records in the list).
    /// </summary>
    /// <returns>The number of records in the list and the number of deleted records.</returns>
    public override (int, int) GetStat()
    {
        this.StartStopwatch();
        var result = this.Service.GetStat();
        this.StopStopwatch("Stat");
        return result;
    }

    /// <summary>
    /// Creates the snapshot of the record list.
    /// </summary>
    /// <returns>The new snapshot object.</returns>
    public override FileCabinetServiceSnapshot MakeSnapshot()
    {
        return this.Service.MakeSnapshot();
    }

    /// <summary>
    /// Removes the existing record.
    /// </summary>
    /// <param name="index">The id of record to remove.</param>
    public override void RemoveRecord(int index)
    {
        this.StartStopwatch();
        this.Service.RemoveRecord(index);
        this.StopStopwatch("Remove");
    }

    /// <summary>
    /// Resores the list from the snapshot.
    /// </summary>
    /// <param name="snapshot">THe snapshot to restore from.</param>
    public override void Restore(FileCabinetServiceSnapshot snapshot)
    {
        this.Service.Restore(snapshot);
    }

    /// <summary>
    /// For this class does nothing.
    /// </summary>
    /// <returns>Zero, the number of all the records before purge.</returns>
    public override (int, int) Purge()
    {
        this.StartStopwatch();
        var result = this.Service.Purge();
        this.StopStopwatch("Purge");
        return result;
    }

    private void StartStopwatch()
    {
        this.stopwatch.Reset();
        this.stopwatch.Start();
    }

    private void StopStopwatch(string methodName)
    {
        this.stopwatch.Stop();
        Console.WriteLine($"{methodName} method execution duration {this.stopwatch.ElapsedTicks} ticks.");
    }
}