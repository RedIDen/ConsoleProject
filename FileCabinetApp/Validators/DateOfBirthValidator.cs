using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Validators
{
    [JsonObject(MemberSerialization.Fields)]
    public class DateOfBirthValidator : IRecordValidator
    {
        [JsonProperty("Min date of birth")]
        private DateTime from;

        [JsonProperty("Max date of birth")]
        private DateTime to;

        public DateOfBirthValidator(DateTime from, DateTime to)
        {
            this.from = from;
            this.to = to;
        }

        public (bool, string) Validate(FileCabinetRecord record)
        {
            var value = record.DateOfBirth;

            if (value < this.from || value > this.to)
            {
                return (false, $"the date should be between {this.from} and {this.to}");
            }
            else
            {
                return (true, string.Empty);
            }
        }
    }
}
