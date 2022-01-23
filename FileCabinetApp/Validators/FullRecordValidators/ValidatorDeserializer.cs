namespace FileCabinetApp.Validators.FullRecordValidators;

public class ValidatorDeserializer
{
    public Dictionary<string, CompositeValidator> Deserialize(string path)
    {
        if (!File.Exists(path))
        {
            return new Dictionary<string, CompositeValidator>
                {
                    { "default", new ValidatorBuilder().CreateDefault() },
                    { "custom", new ValidatorBuilder().CreateCustom() },
                };
        }

        var Validators = new
        {
            Default = new
            {
                FirstName = new
                {
                    minLength = 0,
                    maxLength = 0,
                },
                LastName = new
                {
                    minLength = 0,
                    maxLength = 0,
                },
                DateOfBirth = new
                {
                    from = DateTime.Now,
                    to = DateTime.Now,
                },
                Balance = new
                {
                    minValue = 0M,
                    maxValue = decimal.MaxValue,
                },
                WorkExperience = new
                {
                    minValue = (short)0,
                    maxValue = short.MaxValue,
                },
            },
            Custom = new
            {
                FirstName = new
                {
                    minLength = 0,
                    maxLength = 50,
                },
                LastName = new
                {
                    minLength = 0,
                    maxLength = 50,
                },
                DateOfBirth = new
                {
                    from = DateTime.Now,
                    to = DateTime.Now,
                },
                Balance = new
                {
                    minValue = 0M,
                    maxValue = 100M,
                },
                WorkExperience = new
                {
                    minValue = (short)0,
                    maxValue = (short)1,
                },
            },
        };

        string json;

        using (var filestream = new FileStream(path, FileMode.Open))
        {
            using (var reader = new StreamReader(filestream))
            {
                json = reader.ReadToEnd();
            }
        }

        var tempClasses = JsonConvert.DeserializeAnonymousType(json, Validators);

        return new Dictionary<string, CompositeValidator>
        {
            {
            "default",
            new ValidatorBuilder()
            .ValidateFirstName(tempClasses.Default.FirstName.minLength, tempClasses.Default.FirstName.maxLength)
            .ValidateLastName(tempClasses.Default.LastName.minLength, tempClasses.Default.LastName.maxLength)
            .ValidateDateOfBirth(tempClasses.Default.DateOfBirth.from, tempClasses.Default.DateOfBirth.to)
            .ValidateFavLetter()
            .ValidateBalance(tempClasses.Default.Balance.minValue, tempClasses.Default.Balance.maxValue)
            .ValidateWorkExperience(tempClasses.Default.WorkExperience.minValue, tempClasses.Default.WorkExperience.maxValue)
            .Create()
            },
            {
            "custom",
            new ValidatorBuilder()
            .ValidateFirstName(tempClasses.Custom.FirstName.minLength, tempClasses.Custom.FirstName.maxLength)
            .ValidateLastName(tempClasses.Custom.LastName.minLength, tempClasses.Custom.LastName.maxLength)
            .ValidateDateOfBirth(tempClasses.Custom.DateOfBirth.from, tempClasses.Custom.DateOfBirth.to)
            .ValidateFavLetter()
            .ValidateBalance(tempClasses.Custom.Balance.minValue, tempClasses.Custom.Balance.maxValue)
            .ValidateWorkExperience(tempClasses.Custom.WorkExperience.minValue, tempClasses.Custom.WorkExperience.maxValue)
            .Create()
            },
        };
    }
}