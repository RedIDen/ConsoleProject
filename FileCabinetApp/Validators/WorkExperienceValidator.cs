﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Validators
{
    [JsonObject(MemberSerialization.Fields)]
    public class WorkExperienceValidator : IRecordValidator
    {
        [JsonProperty("Work experience min value")]
        private short minValue;

        [JsonProperty("Work experience max value")]
        private short maxValue;

        public WorkExperienceValidator(short minValue, short maxValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
        }

        public (bool, string) Validate(FileCabinetRecord record)
        {
            var value = record.WorkExperience;

            if (value < this.minValue)
            {
                return (false, $"the work experience can't be less than {this.minValue}");
            }
            else if (value > this.maxValue)
            {
                return (false, $"the work experience can't be more than {this.maxValue}");
            }
            else
            {
                return (true, string.Empty);
            }
        }
    }
}