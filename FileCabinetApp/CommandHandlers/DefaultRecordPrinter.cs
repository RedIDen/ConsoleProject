using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    public class DefaultRecordPrinter : IRecordPrinter
    {
        public void Print(IEnumerable<FileCabinetRecord> records, string errorMessage)
        {
            if (records.Count() == 0)
            {
                Console.WriteLine(errorMessage);
                return;
            }

            foreach (var record in records)
            {
                StringBuilder stringBuilder = new StringBuilder();

                stringBuilder.Append($"#{record.Id}, ");
                stringBuilder.Append($"{record.FirstName}, ");
                stringBuilder.Append($"{record.LastName}, ");
                stringBuilder.Append($"{record.DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.CreateSpecificCulture("en-US"))}, ");
                stringBuilder.Append($"{record.WorkExperience}, ");
                stringBuilder.Append($"{record.Balance.ToString(CultureInfo.InvariantCulture)}, ");
                stringBuilder.Append($"\'{record.FavLetter}\'");

                Console.WriteLine(stringBuilder);
            }
        }
    }
}
