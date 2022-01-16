using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Validators
{
    [JsonObject(MemberSerialization.Fields)]
    public class FirstNameValidator : IRecordValidator
    {
       // [JsonProperty("First name min length")]
        private int minLength;

       // [JsonProperty("First name max length")]
        private int maxLength;

        public FirstNameValidator(int minLength, int maxLength)
        {
            this.minLength = minLength;
            this.maxLength = maxLength;
        }

        public (bool, string) Validate(FileCabinetRecord record)
        {
            var value = record.FirstName;

            if (value is null)
            {
                return (false, "the name is null");
            }
            else if (value.Length > this.maxLength)
            {
                return (false, "the name is too long");
            }
            else if (value.Length < this.minLength)
            {
                return (false, "the name is too short");
            }
            else if (value.Trim().Length == 0)
            {
                return (false, "the name can't consist of only whitespaces");
            }
            else
            {
                return (true, string.Empty);
            }
        }
    }
}
