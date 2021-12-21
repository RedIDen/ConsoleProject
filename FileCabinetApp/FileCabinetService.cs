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
    /// The abstract class to ctreate File Cabinet Services.
    /// </summary>
    public class FileCabinetService : IFileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetService"/> class.
        /// </summary>
        /// <param name="recordValidator">Record validator.</param>
        public FileCabinetService(IRecordValidator recordValidator)
        {
            this.Validator = recordValidator;
        }

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
        public int CreateRecord(FileCabinetRecord record)
        {
            record.Id = this.list.Count + 1;

            this.AddToDictionaries(record);

            this.list.Add(record);

            return record.Id;
        }

        /// <summary>
        /// Edits the existing record.
        /// </summary>
        /// <param name="record">The new record data.</param>
        /// <param name="index">The index of the record to edit.</param>
        public void EditRecord(FileCabinetRecord record, int index)
        {
            record.Id = this.list[index].Id;

            this.DeleteFromDictionaries(this.list[index]);

            this.list[index] = record;

            this.AddToDictionaries(record);
        }

        /// <summary>
        /// Returnd the index of the record with the recieved id.
        /// </summary>
        /// <param name="id">The record's id.</param>
        /// <returns>The index in the list of rhe record with the recieved id.</returns>
        public int FindRecordIndexById(int id) => this.list.FindIndex(e => e.Id == id);

        /// <summary>
        /// Returns the list of all records.
        /// </summary>
        /// <returns>The list of all records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords() => new ReadOnlyCollection<FileCabinetRecord>(this.list);

        /// <summary>
        /// Returns the stats (the number of records in the list).
        /// </summary>
        /// <returns>The number of records in the list.</returns>
        public int GetStat() => this.list.Count;

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
        /// Creates the snapshot of the record list.
        /// </summary>
        /// <returns>The new snapshot object.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot() => new FileCabinetServiceSnapshot(this.list);

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
