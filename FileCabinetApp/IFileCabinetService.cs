using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// The interface for the File Cabinet Service.
    /// </summary>
    public interface IFileCabinetService
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
        /// Returnd the index of the record with the recieved id.
        /// </summary>
        /// <param name="id">The record's id.</param>
        /// <returns>The index in the list of rhe record with the recieved id.</returns>
        public int FindRecordIndexById(int id);

        /// <summary>
        /// Returns the list of all records.
        /// </summary>
        /// <returns>The list of all records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords();

        /// <summary>
        /// Returns the stats (the number of records in the list).
        /// </summary>
        /// <returns>The number of records in the list.</returns>
        public int GetStat();

        /// <summary>
        /// Returns the list of the records with recieved first name.
        /// </summary>
        /// <param name="firstName">First name.</param>
        /// <returns>The list of the records with recieved first name.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName);

        /// <summary>
        /// Returns the list of the records with recieved last name.
        /// </summary>
        /// <param name="lastName">Last name.</param>
        /// <returns>The list of the records with recieved last name.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName);

        /// <summary>
        /// Returns the list of the records with recieved date of birth.
        /// </summary>
        /// <param name="date">Date of birth.</param>
        /// <returns>The list of the records with recieved date of birth.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string date);

        /// <summary>
        /// Makes the snapshot of the service.
        /// </summary>
        /// <returns>Returns the new snapshot object.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot();
    }
}
