using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    public class FileCabineRecordsComparer : IEqualityComparer<FileCabinetRecord>
    {
        public bool Equals(FileCabinetRecord? x, FileCabinetRecord? y)
        {
            return x.Id == y.Id &&
                x.FirstName == y.FirstName &&
                x.LastName == y.LastName &&
                x.DateOfBirth == y.DateOfBirth &&
                x.Balance == y.Balance &&
                x.WorkExperience == y.WorkExperience &&
                x.FavLetter == y.FavLetter;
        }

        public int GetHashCode([DisallowNull] FileCabinetRecord obj)
        {
            return obj.Id.GetHashCode() & obj.FirstName.GetHashCode();
        }
    }
}
