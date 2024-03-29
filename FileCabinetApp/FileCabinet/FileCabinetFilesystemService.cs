﻿#pragma warning disable CS8602

namespace FileCabinetApp.FileCabinet;

/// <summary>
/// The File Cabinet Filesystem Service class.
/// </summary>
internal class FileCabinetFilesystemService : FileCabinetServiceBase, IDisposable
{
    /// <summary>
    /// The file link.
    /// </summary>
    public const string FILENAME = "cabinet-records.db";
    private const int RECORDLENGTH = 277;
    private readonly FileStream fileStream;
    private readonly BinaryWriter writer;
    private readonly BinaryReader reader;

    private readonly Dictionary<string, List<long>> firstNameDictionary = new Dictionary<string, List<long>>();
    private readonly Dictionary<string, List<long>> lastNameDictionary = new Dictionary<string, List<long>>();
    private readonly Dictionary<DateTime, List<long>> dateOfBirthDictionary = new Dictionary<DateTime, List<long>>();
    private readonly Dictionary<decimal, List<long>> balanceDictionary = new Dictionary<decimal, List<long>>();
    private readonly Dictionary<short, List<long>> workExperienceDictionary = new Dictionary<short, List<long>>();
    private readonly Dictionary<char, List<long>> favLetterDictionary = new Dictionary<char, List<long>>();

    /// <summary>
    /// Initializes a new instance of the <see cref="FileCabinetFilesystemService"/> class.
    /// </summary>
    /// <param name="recordValidator">Record validator.</param>
    /// <param name="fileStream">File stream.</param>
    public FileCabinetFilesystemService(IRecordValidator recordValidator, FileStream fileStream)
    {
        this.Validator = recordValidator;
        this.fileStream = fileStream;
        this.writer = new BinaryWriter(this.fileStream);
        this.reader = new BinaryReader(this.fileStream);
        this.FillDictionaries();
    }

    /// <summary>
    /// Finalizes an instance of the <see cref="FileCabinetFilesystemService"/> class.
    /// </summary>
    ~FileCabinetFilesystemService()
    {
        this.Close();
    }

    /// <summary>
    /// Gets or sets the record validator.
    /// </summary>
    /// <value>The object of the class realizing the IRecordValidator interface.</value>
    public override IRecordValidator Validator { get; set; }

    /// <summary>
    /// Closes all the opened file streams.
    /// </summary>
    public void Close()
    {
        this.reader.Close();
        this.writer.Close();
        this.fileStream.Dispose();
        this.Dispose();
    }

    /// <summary>
    /// Adds the record to the list.
    /// </summary>
    /// <param name="record">The record to add to the list.</param>
    /// <returns>Returns the id of the new record.</returns>
    public override int CreateRecord(FileCabinetRecord record)
    {
        var list = this.GetListOfRecords();

        if (record.Id == 0)
        {
            record.Id = list.Count == 0 ? 1 : list.Max(x => x.Id) + 1;
        }

        this.WriteRecord(record, this.writer.BaseStream.Length);

        this.AddToDictionaries(record);

        return record.Id;
    }

    /// <summary>
    /// Edits the existing record.
    /// </summary>
    /// <param name="newRecordData">The new record data.</param>
    /// <param name="index">The index of the record to edit.</param>
    public override void EditRecord(FileCabinetRecord newRecordData, int index)
    {
        long position = index * FileCabinetFilesystemService.RECORDLENGTH;
        this.reader.BaseStream.Position = position;

        var record = this.GetRecord();
        this.DeleteFromDictionaries(record);

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

        this.WriteRecord(record, position);
        this.AddToDictionaries(record);
    }

    /// <summary>
    /// Returns the list of the records with recieved date of birth.
    /// </summary>
    /// <param name="date">Date of birth.</param>
    /// <returns>The list of the records with recieved date of birth.</returns>
    public override IEnumerable<FileCabinetRecord> FindByDateOfBirth(string date)
    {
        var parseResult = DateTime.TryParse(
            date,
            CultureInfo.CreateSpecificCulture("en-US"),
            DateTimeStyles.None,
            out DateTime dateOfBirth);

        var positions = parseResult ? this.dateOfBirthDictionary.GetValueOrDefault(dateOfBirth) : null;
        return this.GetEnumerable(positions);
    }

    /// <summary>
    /// Returns the list of the records with recieved first name.
    /// </summary>
    /// <param name="firstName">First name.</param>
    /// <returns>The list of the records with recieved first name.</returns>
    public override IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
    {
        var positions = this.firstNameDictionary.GetValueOrDefault(firstName.ToLower(CultureInfo.InvariantCulture));
        return this.GetEnumerable(positions);
    }

    /// <summary>
    /// Returns the list of the records with recieved last name.
    /// </summary>
    /// <param name="lastName">Last name.</param>
    /// <returns>The list of the records with recieved last name.</returns>
    public override IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
    {
        var positions = this.lastNameDictionary.GetValueOrDefault(lastName.ToLower(CultureInfo.InvariantCulture));
        return this.GetEnumerable(positions);
    }

    /// <summary>
    /// Returns the list of the records with recieved balance.
    /// </summary>
    /// <param name="data">Balance.</param>
    /// <returns>The list of the records with recieved balance.</returns>
    public override IEnumerable<FileCabinetRecord> FindByBalance(string data)
    {
        var parseResult = decimal.TryParse(data, out decimal balance);

        var positions = parseResult ? this.balanceDictionary.GetValueOrDefault(balance) : null;
        return this.GetEnumerable(positions);
    }

    /// <summary>
    /// Returns the list of the records with recieved work experience.
    /// </summary>
    /// <param name="data">Work experience.</param>
    /// <returns>The list of the records with recieved work experience.</returns>
    public override IEnumerable<FileCabinetRecord> FindByWorkExperience(string data)
    {
        var parseResult = short.TryParse(data, out short workExperience);

        var positions = parseResult ? this.workExperienceDictionary.GetValueOrDefault(workExperience) : null;
        return this.GetEnumerable(positions);
    }

    /// <summary>
    /// Returns the list of the records with recieved favorite letter.
    /// </summary>
    /// <param name="data">Favorite letter.</param>
    /// <returns>The list of the records with recieved favorite letter.</returns>
    public override IEnumerable<FileCabinetRecord> FindByFavLetter(string data)
    {
        var parseResult = char.TryParse(data, out char favLetter);

        var positions = parseResult ? this.favLetterDictionary.GetValueOrDefault(favLetter) : null;
        return this.GetEnumerable(positions);
    }

    /// <summary>
    /// Returns the list of the records with recieved id.
    /// </summary>
    /// <param name="data">Id.</param>
    /// <returns>The list of the records with recieved id.</returns>
    public override IEnumerable<FileCabinetRecord> FindById(string data)
    {
        var positions = new List<long>(0);

        if (int.TryParse(data, out int id))
        {
            var index = this.FindRecordIndexById(id);
            if (index != -1)
            {
                positions.Add(index * FileCabinetFilesystemService.RECORDLENGTH);
            }
        }

        return this.GetEnumerable(positions);
    }

    /// <summary>
    /// Returnd the index of the record with the recieved id.
    /// </summary>
    /// <param name="id">The record's id.</param>
    /// <returns>The index in the list of rhe record with the recieved id.</returns>
    public override int FindRecordIndexById(int id)
    {
        this.reader.BaseStream.Position = 2;

        while (this.reader.BaseStream.Position < this.reader.BaseStream.Length)
        {
            int currentId = this.reader.ReadInt32();

            if (id == currentId)
            {
                this.reader.BaseStream.Position -= 6;
                short deleted = this.reader.ReadInt16();
                if ((deleted >> 2 & 1) != 1)
                {
                    return (int)(this.reader.BaseStream.Position / FileCabinetFilesystemService.RECORDLENGTH);
                }

                this.reader.BaseStream.Position += FileCabinetFilesystemService.RECORDLENGTH;
            }
            else
            {
                this.reader.BaseStream.Position += FileCabinetFilesystemService.RECORDLENGTH - 4;
            }
        }

        return -1;
    }

    /// <summary>
    /// Returns the readonly collection of all records.
    /// </summary>
    /// <returns>The readonly collection of all records.</returns>
    public override IEnumerable<FileCabinetRecord> GetRecords() => new ReadOnlyCollection<FileCabinetRecord>(this.GetListOfRecords());

    /// <summary>
    /// Returns the stats (the number of records in the list).
    /// </summary>
    /// <returns>The number of records in the list and the number of deleted records.</returns>
    public override (int, int) GetStat() => ((int)(this.fileStream.Length / FileCabinetFilesystemService.RECORDLENGTH), this.CountDeletedRecords());

    /// <summary>
    /// Creates the snapshot of the record list.
    /// </summary>
    /// <returns>The new snapshot object.</returns>
    public override FileCabinetServiceSnapshot MakeSnapshot() => new FileCabinetServiceSnapshot(this.GetListOfRecords());

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
                    this.WriteRecord(record, this.writer.BaseStream.Length);
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
        long position = FileCabinetFilesystemService.RECORDLENGTH * index;
        this.reader.BaseStream.Position = position;

        var record = this.GetRecord();
        this.DeleteFromDictionaries(record);

        this.writer.BaseStream.Position = position;

        short data = this.reader.ReadInt16();

        this.writer.BaseStream.Position = position;

        this.writer.Write((short)(data | 0b0000_0000_0000_0100));
    }

    /// <summary>
    /// Purges the records with deleted-flag.
    /// </summary>
    /// <returns>The number of purged records, the number of all the records before purge.</returns>
    public override (int, int) Purge()
    {
        long lengthBefore = this.fileStream.Length;

        long writerPosition = 0;
        long readerPosition;
        this.writer.BaseStream.Position = writerPosition;

        while (this.fileStream.Position < this.fileStream.Length)
        {
            FileCabinetRecord record = new FileCabinetRecord();
            if (this.TryGetRecord(ref record))
            {
                readerPosition = this.reader.BaseStream.Position;
                this.DeleteFromDictionaries(record);
                this.reader.BaseStream.Position = readerPosition;
                this.WriteRecord(record, writerPosition);
                writerPosition += FileCabinetFilesystemService.RECORDLENGTH;
                this.reader.BaseStream.Position = readerPosition;
                this.AddToDictionaries(record);
                this.reader.BaseStream.Position = readerPosition;
            }
        }

        this.fileStream.SetLength(writerPosition);

        long lengthAfter = this.fileStream.Length;

        return ((int)((lengthBefore - lengthAfter) / FileCabinetFilesystemService.RECORDLENGTH), (int)(lengthBefore / FileCabinetFilesystemService.RECORDLENGTH));
    }

    /// <summary>
    /// Releases all the resources used by the service.
    /// </summary>
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    private IEnumerable<FileCabinetRecord> GetEnumerable(IEnumerable<long>? positions)
    {
        if (positions == null)
        {
            return Array.Empty<FileCabinetRecord>();
        }

        return new FilesystemEnumerator<FileCabinetRecord>(
            positions,
            () => this.GetRecord(),
            (long value) => this.fileStream.Position = value);
    }

    /// <summary>
    /// Returns the list of all records.
    /// </summary>
    /// <returns>The list of all records.</returns>
    private List<FileCabinetRecord> GetListOfRecords()
    {
        var list = new List<FileCabinetRecord>();

        this.reader.BaseStream.Position = 0;

        while (this.reader.BaseStream.Position < this.reader.BaseStream.Length)
        {
            FileCabinetRecord record = new FileCabinetRecord();
            if (this.TryGetRecord(ref record))
            {
                list.Add(record);
            }
        }

        return list;
    }

    /// <summary>
    /// Tries to read the record from file.
    /// </summary>
    /// <param name="record">The records to fill.</param>
    /// <returns>The result of reading. True - succesfully, false - not.</returns>
    private bool TryGetRecord(ref FileCabinetRecord record)
    {
        short deleted = this.reader.ReadInt16();

        this.reader.BaseStream.Position -= 2;

        record = this.GetRecord();

        return (deleted >> 2 & 1) != 1;
    }

    /// <summary>
    /// Reads the record from file.
    /// </summary>
    /// <returns>The record.</returns>
    private FileCabinetRecord GetRecord()
    {
        this.reader.ReadInt16();

        var record = new FileCabinetRecord();

        record.Id = this.reader.ReadInt32();

        record.FirstName = Encoding.Unicode.GetString(this.reader.ReadBytes(120)).Trim('\0');

        record.LastName = Encoding.Unicode.GetString(this.reader.ReadBytes(120)).Trim('\0');

        record.DateOfBirth = new DateTime(this.reader.ReadInt32(), this.reader.ReadInt32(), this.reader.ReadInt32());

        record.Balance = this.reader.ReadDecimal();

        record.WorkExperience = this.reader.ReadInt16();

        record.FavLetter = this.reader.ReadChar();

        return record;
    }

    /// <summary>
    /// Writes the record to the file to the position.
    /// </summary>
    /// <param name="record">The record to write.</param>
    /// <param name="position">The position.</param>
    private void WriteRecord(FileCabinetRecord record, long position)
    {
        this.writer.BaseStream.Position = position;

        this.writer.Write(new byte[2]);

        this.writer.Write(record.Id);

        byte[] firstNameBytes = new byte[120];
        var temp1 = Encoding.Unicode.GetBytes(record.FirstName);
        Array.Copy(temp1, firstNameBytes, temp1.Length);
        this.writer.Write(firstNameBytes);

        byte[] lastNameBytes = new byte[120];
        var temp2 = Encoding.Unicode.GetBytes(record.LastName);
        Array.Copy(temp2, lastNameBytes, temp2.Length);
        this.writer.Write(lastNameBytes);

        this.writer.Write(record.DateOfBirth.Year);

        this.writer.Write(record.DateOfBirth.Month);

        this.writer.Write(record.DateOfBirth.Day);

        this.writer.Write(record.Balance);

        this.writer.Write(record.WorkExperience);

        this.writer.Write(record.FavLetter);

        this.writer.Flush();
    }

    /// <summary>
    /// Fills the dictionaries with all the data from the file.
    /// </summary>
    private void FillDictionaries()
    {
        var list = this.GetListOfRecords();

        foreach (var record in list)
        {
            this.AddToDictionaries(record);
        }
    }

    /// <summary>
    /// Adds new record to dicionaries.
    /// </summary>
    /// <param name="record">The record to add.</param>
    private void AddToDictionaries(FileCabinetRecord record)
    {
        string lowerFirstName = record.FirstName.ToLower(CultureInfo.InvariantCulture);
        long position = this.FindRecordIndexById(record.Id) * FileCabinetFilesystemService.RECORDLENGTH;

        if (this.firstNameDictionary.ContainsKey(lowerFirstName))
        {
            this.firstNameDictionary.GetValueOrDefault(lowerFirstName).Add(position);
        }
        else
        {
            var list = new List<long>();
            list.Add(position);
            this.firstNameDictionary.Add(lowerFirstName, list);
        }

        string lowerLastName = record.LastName.ToLower(CultureInfo.InvariantCulture);

        if (this.lastNameDictionary.ContainsKey(lowerLastName))
        {
            this.lastNameDictionary.GetValueOrDefault(lowerLastName).Add(position);
        }
        else
        {
            var list = new List<long>();
            list.Add(position);
            this.lastNameDictionary.Add(lowerLastName, list);
        }

        if (this.dateOfBirthDictionary.ContainsKey(record.DateOfBirth))
        {
            this.dateOfBirthDictionary.GetValueOrDefault(record.DateOfBirth).Add(position);
        }
        else
        {
            var list = new List<long>();
            list.Add(position);
            this.dateOfBirthDictionary.Add(record.DateOfBirth, list);
        }

        if (this.balanceDictionary.ContainsKey(record.Balance))
        {
            this.balanceDictionary.GetValueOrDefault(record.Balance).Add(position);
        }
        else
        {
            var list = new List<long>();
            list.Add(position);
            this.balanceDictionary.Add(record.Balance, list);
        }

        if (this.workExperienceDictionary.ContainsKey(record.WorkExperience))
        {
            this.workExperienceDictionary.GetValueOrDefault(record.WorkExperience).Add(position);
        }
        else
        {
            var list = new List<long>();
            list.Add(position);
            this.workExperienceDictionary.Add(record.WorkExperience, list);
        }

        if (this.favLetterDictionary.ContainsKey(record.FavLetter))
        {
            this.favLetterDictionary.GetValueOrDefault(record.FavLetter).Add(position);
        }
        else
        {
            var list = new List<long>();
            list.Add(position);
            this.favLetterDictionary.Add(record.FavLetter, list);
        }
    }

    /// <summary>
    /// Deletes the record to dicionaries.
    /// </summary>
    /// <param name="record">The record to delete.</param>
    private void DeleteFromDictionaries(FileCabinetRecord record)
    {
        long position = this.FindRecordIndexById(record.Id) * FileCabinetFilesystemService.RECORDLENGTH;

        string lowerFirstName = record.FirstName.ToLower(CultureInfo.InvariantCulture);
        this.firstNameDictionary
            .GetValueOrDefault(lowerFirstName)
            .Remove(position);

        string lowerLastName = record.LastName.ToLower(CultureInfo.InvariantCulture);
        this.lastNameDictionary
            .GetValueOrDefault(lowerLastName)
            .Remove(position);

        this.dateOfBirthDictionary
            .GetValueOrDefault(record.DateOfBirth)
            .Remove(position);

        this.balanceDictionary
            .GetValueOrDefault(record.Balance)
            .Remove(position);

        this.workExperienceDictionary
            .GetValueOrDefault(record.WorkExperience)
            .Remove(position);

        this.favLetterDictionary
            .GetValueOrDefault(record.FavLetter)
            .Remove(position);
    }

    private int CountDeletedRecords()
    {
        int result = 0;
        this.reader.BaseStream.Position = 0;
        while (this.reader.BaseStream.Position < this.reader.BaseStream.Length)
        {
            short deleted = this.reader.ReadInt16();
            if ((deleted >> 2 & 1) == 1)
            {
                result++;
            }

            this.reader.BaseStream.Position += RECORDLENGTH - 2;
        }

        return result;
    }
}