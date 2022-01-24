namespace FileCabinetApp.CommandHandlers;

internal class UpdateCommandHandler : ServiceCommandHandlerBase
{
    public UpdateCommandHandler(FileCabinetTrasferHelper service)
        : base(service)
    {
    }

    protected override string[] CommandNames { get; } = { "update" };

    protected override void MakeWork(string parameters)
    {
        var parametersAndPredicates = parameters.Split("where").ToList();

        if (parametersAndPredicates.Count != 2)
        {
            Console.WriteLine(WrongSyntaxError);
            return;
        }

        char[] symbols = { ',', ' ', '(', ')', '\'', '\"', '=' };
        var parametersList = parametersAndPredicates[0].Split(symbols, StringSplitOptions.RemoveEmptyEntries).ToList();
        var record = new FileCabinetRecord() { Balance = -1, WorkExperience = -1 };

        if (parametersList.Count < 2)
        {
            Console.WriteLine(WrongSyntaxError);
            return;
        }

        for (int i = 1; i < parametersList.Count; i++)
        {
            var convertResult = parametersList[i].ToLower() switch
            {
                _ when ++i >= parametersList.Count => (false, WrongSyntaxError),
                FirstNameWord => this.TryInsertFirstName(record, parametersList[i]),
                LastNameWord => this.TryInsertLastName(record, parametersList[i]),
                DateOfBirthWord => this.TryInsertDateOfBirth(record, parametersList[i]),
                BalanceWord => this.TryInsertBalance(record, parametersList[i]),
                WorkExperienceWord => this.TryInsertWorkExperience(record, parametersList[i]),
                FavLetterWord => this.TryInsertFavLetter(record, parametersList[i]),
                _ => (false, WrongSyntaxError),
            };

            if (!convertResult.Item1)
            {
                var convertError = $"Error: {convertResult.Item2}.";
                Console.WriteLine(convertError);
                return;
            }
        }

        var validationResult = (this.transferHelper.Service.Validator as CompositeValidator).ValidateInitializedFields(record);

        if (!validationResult.Item1)
        {
            var validationError = $"Error: {validationResult.Item2}.";
            Console.WriteLine(validationError);
            return;
        }

        var list = this.transferHelper.Service.Find(parametersAndPredicates[1]);
        var ids = new List<int>();

        int count = 0;
        foreach (var foundRecord in list)
        {
            ids.Add(foundRecord.Id);
            count++;
            this.transferHelper.Service.EditRecord(record, this.transferHelper.Service.FindRecordIndexById(foundRecord.Id));
        }

        if (count == 0)
        {
            Console.WriteLine("No records found.");
        }
        else if (count == 1)
        {
            Console.WriteLine($"Record #{ids[0]} is updated.");
        }
        else
        {
            Console.Write("Records ");
            foreach (var id in ids)
            {
                Console.Write($"#{id} ");
            }

            Console.WriteLine("are updated.");
        }
    }
}