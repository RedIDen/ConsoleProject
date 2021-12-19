using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable CS8602

namespace FileCabinetApp;

/// <summary>
/// The class overriding FileCabinetService.ValidateParameters method.
/// </summary>
public class FileCabinetCustomService : FileCabinetService
{
    /// <summary>
    /// Validates the parameters.
    /// </summary>
    /// <param name="record">The record to check its data.</param>
    public override void ValidateParameters(FileCabinetRecord record)
    {
        CheckNames(record.FirstName, record.LastName);
        CheckDateOfBirth(record.DateOfBirth);
        CheckWorkExperience(record.WorkExperience, record.DateOfBirth);
        CheckBalance(record.Balance);
        CheckFavLetter(record.FavLetter);
    }

    /// <summary>
    /// Validates the first and the last names.
    /// </summary>
    /// <param name="firstName">First name.</param>
    /// <param name="lastName">Last name.</param>
    /// <exception cref="ArgumentNullException">Thrown when one of the names is null.</exception>
    /// <exception cref="ArgumentException">Thrown when one of the names is shorter than 2 symbols.
    /// Also thrown when the name consists of only whitespaces.</exception>
    private static void CheckNames(string firstName, string lastName)
    {
        if (firstName is null)
        {
            throw new ArgumentNullException(nameof(firstName));
        }

        if (lastName is null)
        {
            throw new ArgumentNullException(nameof(lastName));
        }

        CheckNamesLength(firstName, "First name");
        CheckNamesLength(lastName, "Last name");

        CheckNameForSpaces(firstName, "First name");
        CheckNameForSpaces(lastName, "Last name");

        void CheckNamesLength(string a, string nameOfString)
        {
            if (a.Length < 2)
            {
                throw new ArgumentException($"{nameOfString} is too short.");
            }
        }

        void CheckNameForSpaces(string a, string nameOfString)
        {
            if (a.Trim().Length == 0)
            {
                throw new ArgumentException($"{nameOfString} can't consist of only whitespaces.");
            }
        }
    }

    /// <summary>
    /// Validates the date of birth.
    /// </summary>
    /// <param name="date">Date of birth.</param>
    /// <exception cref="ArgumentException">Thrown when the dae is less than 1900-Jan-1 or more than today's date.</exception>
    private static void CheckDateOfBirth(DateTime date)
    {
        if (date < new DateTime(1900, 1, 1) || date > DateTime.Now)
        {
            throw new ArgumentException("Incorrect date.");
        }
    }

    /// <summary>
    /// Validates the work experience.
    /// </summary>
    /// <param name="workExperience">Work experience.</param>
    /// <param name="dateOfBirth">Date of birth.</param>
    /// <exception cref="ArgumentException">Thrown when the work experience more than persons age - 16.</exception>
    private static void CheckWorkExperience(short workExperience, DateTime dateOfBirth)
    {
        int age = DateTime.Now.Subtract(dateOfBirth).Days / 365;
        if (workExperience < 0 || workExperience > age - 16)
        {
            throw new ArgumentException("Incorrect work experisnce.");
        }
    }

    /// <summary>
    /// Validates the balance.
    /// </summary>
    /// <param name="balance">Balance.</param>
    /// <exception cref="ArgumentException">Thrown when balence is less than 0.</exception>
    private static void CheckBalance(decimal balance)
    {
        if (balance < 0)
        {
            throw new ArgumentException("Incorrect balance.");
        }
    }

    /// <summary>
    /// Validates the favorite letter.
    /// </summary>
    /// <param name="favLetter">Favorite letter.</param>
    /// <exception cref="ArgumentException">Thrown when char is not a letter.</exception>
    private static void CheckFavLetter(char favLetter)
    {
        if (!char.IsLetter(favLetter))
        {
            throw new ArgumentException("The char is not a letter.");
        }
    }
}