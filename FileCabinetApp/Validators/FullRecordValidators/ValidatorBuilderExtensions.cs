using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Validators.FullRecordValidators
{
    public static class ValidatorBuilderExtensions
    {
        public static CompositeValidator CreateDefault(this ValidatorBuilder validatorBuilder) => validatorBuilder
            .ValidateFirstName(0, 60)
            .ValidateLastName(0, 60)
            .ValidateDateOfBirth(new DateTime(1950, 1, 1), DateTime.Now)
            .ValidateFavLetter()
            .ValidateBalance(0, decimal.MaxValue)
            .ValidateWorkExperience(0, short.MaxValue)
            .Create();

        public static CompositeValidator CreateCustom(this ValidatorBuilder validatorBuilder) => validatorBuilder
            .ValidateFirstName(0, 50)
            .ValidateLastName(0, 50)
            .ValidateDateOfBirth(new DateTime(1900, 1, 1), DateTime.Now)
            .ValidateFavLetter()
            .ValidateBalance(0, 100)
            .ValidateWorkExperience(0, 100)
            .Create();
    }
}
