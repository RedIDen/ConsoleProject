namespace FileCabinetApp.Validators.FullRecordValidators
{
    public class CompositeValidator : IRecordValidator
    {
        private List<IRecordValidator> validators;

        public CompositeValidator(IEnumerable<IRecordValidator> validators)
        {
            this.validators = validators.ToList();
        }

        public (bool, string) Validate(FileCabinetRecord record)
        {
            bool result = true;
            StringBuilder errorMessage = new StringBuilder();
            bool tempResult;
            string tempMessage;

            foreach (var validator in this.validators)
            {
                (tempResult, tempMessage) = validator.Validate(record);
                result &= tempResult;
                errorMessage.Append(tempMessage.Length == 0 ? tempMessage : tempMessage + '\n');
            }

            return (result, errorMessage.ToString().Trim('\n'));
        }
    }
}
