using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Validators
{
    public class LastNameValidator : IRecordValidator
    {
        private int minLength;
        private int maxLength;

        public LastNameValidator(int minLength, int maxLength)
        {
            this.minLength = minLength;
            this.maxLength = maxLength;
        }

        public (bool, string) Validate(FileCabinetRecord record)
        {
            var value = record.LastName;

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
