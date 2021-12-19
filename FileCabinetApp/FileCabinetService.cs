using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable CS8618
#pragma warning disable CS8602

namespace FileCabinetApp
{
    /// <summary>
    /// The abstract class to ctreate File Cabinet Services.
    /// </summary>
    public abstract class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();

        /// <summary>
        /// If there's a correct data, adds the record to the list.
        /// </summary>
        /// <param name="record">The record to add to the list.</param>
        /// <returns>Returns the id of the new record.</returns>
        public int CreateRecord(FileCabinetRecord record)
        {
            this.ValidateParameters(record);

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
            this.ValidateParameters(record);

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
        public FileCabinetRecord[] GetRecords() => this.list.ToArray();

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
        public FileCabinetRecord[] FindByFirstName(string firstName) =>
            (this.firstNameDictionary.GetValueOrDefault(firstName.ToLower()) ?? new List<FileCabinetRecord>()).ToArray();

        /// <summary>
        /// Returns the list of the records with recieved last name.
        /// </summary>
        /// <param name="lastName">Last name.</param>
        /// <returns>The list of the records with recieved last name.</returns>
        public FileCabinetRecord[] FindByLastName(string lastName) =>
            (this.lastNameDictionary.GetValueOrDefault(lastName.ToLower()) ?? new List<FileCabinetRecord>()).ToArray();

        /// <summary>
        /// Returns the list of the records with recieved date of birth.
        /// </summary>
        /// <param name="date">Date of birth.</param>
        /// <returns>The list of the records with recieved date of birth.</returns>
        public FileCabinetRecord[] FindByDateOfBirth(string date)
        {
            DateTime dateOfBirth = DateTime.Parse(
                date,
                CultureInfo.CreateSpecificCulture("en-US"),
                DateTimeStyles.None);

            return (this.dateOfBirthDictionary.GetValueOrDefault(dateOfBirth) ?? new List<FileCabinetRecord>()).ToArray();
        }

        /// <summary>
        /// Validates the parameters.
        /// </summary>
        /// <param name="record">The record to check its data.</param>
        public abstract void ValidateParameters(FileCabinetRecord record);

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
