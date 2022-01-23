namespace FileCabinetApp;

/// <summary>
/// The interface for the File Cabinet Service.
/// </summary>
internal interface IFileCabinetService
{
    /// <summary>
    /// Gets or sets the record validator.
    /// </summary>
    /// <value>The object of the class realizing the IRecordValidator interface.</value>
    public IRecordValidator Validator { get; set; }

    /// <summary>
    /// If there's a correct data, adds the record to the list.
    /// </summary>
    /// <param name="record">The record to add to the list.</param>
    /// <returns>Returns the id of the new record.</returns>
    public int CreateRecord(FileCabinetRecord record);

    /// <summary>
    /// Edits the existing record.
    /// </summary>
    /// <param name="record">The new record data.</param>
    /// <param name="index">The index of the record to edit.</param>
    public void EditRecord(FileCabinetRecord record, int index);

    /// <summary>
    /// Returns the index of the record with the recieved id.
    /// </summary>
    /// <param name="id">The record's id.</param>
    /// <returns>The index in the list of rhe record with the recieved id.</returns>
    public int FindRecordIndexById(int id);

    /// <summary>
    /// Returns the list of all records.
    /// </summary>
    /// <returns>The list of all records.</returns>
    public IEnumerable<FileCabinetRecord> GetRecords();

    /// <summary>
    /// Returns the stats (the number of records in the list).
    /// </summary>
    /// <returns>The number of records in the list and the number of deleted records.</returns>
    public (int, int) GetStat();

    /// <summary>
    /// Returns the list of the records with recieved first name.
    /// </summary>
    /// <param name="firstName">First name.</param>
    /// <returns>The list of the records with recieved first name.</returns>
    public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName);

    /// <summary>
    /// Returns the list of the records with recieved last name.
    /// </summary>
    /// <param name="lastName">Last name.</param>
    /// <returns>The list of the records with recieved last name.</returns>
    public IEnumerable<FileCabinetRecord> FindByLastName(string lastName);

    /// <summary>
    /// Returns the list of the records with recieved date of birth.
    /// </summary>
    /// <param name="date">Date of birth.</param>
    /// <returns>The list of the records with recieved date of birth.</returns>
    public IEnumerable<FileCabinetRecord> FindByDateOfBirth(string date);

    public IEnumerable<FileCabinetRecord> FindByBalance(string data);

    public IEnumerable<FileCabinetRecord> FindByWorkExperience(string data);

    public IEnumerable<FileCabinetRecord> FindByFavLetter(string data);

    public IEnumerable<FileCabinetRecord> FindById(string data);

    /// <summary>
    /// Makes the snapshot of the service.
    /// </summary>
    /// <returns>Returns the new snapshot object.</returns>
    public FileCabinetServiceSnapshot MakeSnapshot();

    /// <summary>
    /// Restores the list from the snapshot.
    /// </summary>
    /// <param name="snapshot">The snapshot to restore from.</param>
    public void Restore(FileCabinetServiceSnapshot snapshot);

    /// <summary>
    /// Removes the existing record.
    /// </summary>
    /// <param name="id">The id of record to remove.</param>
    public void RemoveRecord(int id);

    public (int, int) Purge();

    public IEnumerable<FileCabinetRecord> Find(string parameters);
}