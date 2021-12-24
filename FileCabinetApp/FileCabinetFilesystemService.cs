using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable CS8602

namespace FileCabinetApp
{
    /// <summary>
    /// The File Cabinet Filesystem Service class.
    /// </summary>
    internal class FileCabinetFilesystemService : IFileCabinetService, IDisposable
    {
        /// <summary>
        /// The file link.
        /// </summary>
        public const string FILENAME = "cabinet-records.db";
        private const int RECORDLENGTH = 277;
        private readonly FileStream fileStream;
        private readonly BinaryWriter writer;
        private readonly BinaryReader reader;

        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();

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

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Close();
        }

        /// <summary>
        /// Gets or sets the record validator.
        /// </summary>
        /// <value>The object of the class realizing the IRecordValidator interface.</value>
        public IRecordValidator Validator { get; set; }

        /// <summary>
        /// Closes all the opened file streams.
        /// </summary>
        public void Close()
        {
            this.reader.Close();
            this.writer.Close();
            this.fileStream.Close();
        }

        /// <summary>
        /// If there's a correct data, adds the record to the list.
        /// </summary>
        /// <param name="record">The record to add to the list.</param>
        /// <returns>Returns the id of the new record.</returns>
        public int CreateRecord(FileCabinetRecord record)
        {
            record.Id = (int)(this.fileStream.Length / RECORDLENGTH) + 1;

            this.AddToDictionaries(record);

            this.WriteRecord(record, this.writer.BaseStream.Length);

            return record.Id;
        }

        /// <summary>
        /// Edits the existing record.
        /// </summary>
        /// <param name="record">The new record data.</param>
        /// <param name="index">The index of the record to edit.</param>
        public void EditRecord(FileCabinetRecord record, int index)
        {
            int position = index * RECORDLENGTH;
            this.reader.BaseStream.Position = position;

            var oldRecord = this.GetRecord();
            this.DeleteFromDictionaries(oldRecord);

            this.AddToDictionaries(record);
            this.WriteRecord(record, position);
        }

        /// <summary>
        /// Returns the list of the records with recieved date of birth.
        /// </summary>
        /// <param name="date">Date of birth.</param>
        /// <returns>The list of the records with recieved date of birth.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string date)
        {
            DateTime dateOfBirth = DateTime.Parse(
                date,
                CultureInfo.CreateSpecificCulture("en-US"),
                DateTimeStyles.None);

            return new ReadOnlyCollection<FileCabinetRecord>((this.dateOfBirthDictionary.GetValueOrDefault(dateOfBirth) ?? new List<FileCabinetRecord>()).ToArray());
        }

        /// <summary>
        /// Returns the list of the records with recieved first name.
        /// </summary>
        /// <param name="firstName">First name.</param>
        /// <returns>The list of the records with recieved first name.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName) =>
                    new ReadOnlyCollection<FileCabinetRecord>((this.firstNameDictionary.GetValueOrDefault(firstName.ToLower()) ?? new List<FileCabinetRecord>()).ToArray());

        /// <summary>
        /// Returns the list of the records with recieved last name.
        /// </summary>
        /// <param name="lastName">Last name.</param>
        /// <returns>The list of the records with recieved last name.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName) =>
            new ReadOnlyCollection<FileCabinetRecord>((this.lastNameDictionary.GetValueOrDefault(lastName.ToLower()) ?? new List<FileCabinetRecord>()).ToArray());

        /// <summary>
        /// Returnd the index of the record with the recieved id.
        /// </summary>
        /// <param name="id">The record's id.</param>
        /// <returns>The index in the list of rhe record with the recieved id.</returns>
        public int FindRecordIndexById(int id)
        {
            this.reader.BaseStream.Position = 2;

            while (this.reader.BaseStream.Position < this.reader.BaseStream.Length)
            {
                int currentId = this.reader.ReadInt32();

                if (id == currentId)
                {
                    return (int)(this.reader.BaseStream.Position / RECORDLENGTH);
                }

                this.reader.BaseStream.Position += RECORDLENGTH - 4;
            }

            return -1;
        }

        /// <summary>
        /// Returns the readonly collection of all records.
        /// </summary>
        /// <returns>The readonly collection of all records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords() => new ReadOnlyCollection<FileCabinetRecord>(this.GetListOfRecords());

        /// <summary>
        /// Returns the stats (the number of records in the list).
        /// </summary>
        /// <returns>The number of records in the list.</returns>
        public int GetStat() => (int)(this.fileStream.Length / RECORDLENGTH);

        /// <summary>
        /// Creates the snapshot of the record list.
        /// </summary>
        /// <returns>The new snapshot object.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot() => new FileCabinetServiceSnapshot(this.GetListOfRecords());

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
                list.Add(this.GetRecord());
            }

            return list;
        }

        /// <summary>
        /// Reads the record from the file from the current pointer position.
        /// </summary>
        /// <returns>The record.</returns>
        private FileCabinetRecord GetRecord()
        {
            var record = new FileCabinetRecord();

            this.reader.ReadBytes(2);

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
            string lowerFirstName = record.FirstName.ToLower();

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

            string lowerLastName = record.LastName.ToLower();

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
        }

        /// <summary>
        /// Deletes the record to dicionaries.
        /// </summary>
        /// <param name="record">The record to delete.</param>
        private void DeleteFromDictionaries(FileCabinetRecord record)
        {
            string lowerFirstName = record.FirstName.ToLower();
            this.firstNameDictionary.GetValueOrDefault(lowerFirstName).Remove(record);
            this.firstNameDictionary.Remove(lowerFirstName);

            string lowerLastName = record.LastName.ToLower();
            this.lastNameDictionary.GetValueOrDefault(lowerLastName).Remove(record);
            this.lastNameDictionary.Remove(lowerLastName);

            this.dateOfBirthDictionary.GetValueOrDefault(record.DateOfBirth).Remove(record);
            this.dateOfBirthDictionary.Remove(record.DateOfBirth);
        }
    }
}
