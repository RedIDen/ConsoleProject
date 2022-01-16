using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Validators
{
    [JsonObject(MemberSerialization.Fields)]
    public class BalanceValidator : IRecordValidator
    {
        [JsonProperty("Balance min value")]
        private decimal minValue;

        [JsonProperty("Balance max value")]
        private decimal maxValue;

        public BalanceValidator(decimal minValue, decimal maxValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
        }

        public (bool, string) Validate(FileCabinetRecord record)
        {
            var value = record.Balance;

            if (value < this.minValue)
            {
                return (false, $"the balance can't be less than {this.minValue}");
            }
            else if (value > this.maxValue)
            {
                return (false, $"the balance can't be more than {this.maxValue}");
            }
            else
            {
                return (true, string.Empty);
            }
        }
    }
}
