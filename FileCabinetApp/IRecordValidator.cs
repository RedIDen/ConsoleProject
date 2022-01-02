using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// The intarface for the record validators.
    /// </summary>
    public interface IRecordValidator
    {
        /// <summary>
        /// Validates the first name.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns>Result of validation.</returns>
        public (bool, string) FirstNameValidator(string value);

        /// <summary>
        /// Validates the second name.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns>Result of validation.</returns>
        public (bool, string) LastNameValidator(string value);

        /// <summary>
        /// Validates the date of birth.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns>Result of validation.</returns>
        public (bool, string) DateOfBirthValidator(DateTime value);

        /// <summary>
        /// Validates the work experience.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns>Result of validation.</returns>
        public (bool, string) WorkExperienceValidator(short value);

        /// <summary>
        /// Validates the balance.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns>Result of validation.</returns>
        public (bool, string) BalanceValidator(decimal value);

        /// <summary>
        /// Validates the favorite letter.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns>Result of validation.</returns>
        public (bool, string) FavLetterValidator(char value);

        public (bool, string) RecordValidator(FileCabinetRecord record);
    }
}
