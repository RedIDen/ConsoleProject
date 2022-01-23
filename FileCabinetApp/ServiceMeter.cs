using System.Diagnostics;

namespace FileCabinetApp;
public class ServiceMeter : FileCabinetServiceBase
{
    private readonly Stopwatch stopwatch = new Stopwatch();

    public override IRecordValidator Validator
    {
        get => this.Service.Validator;
        set
        {
            this.StartStopwatch();
            this.Service.Validator = value;
            this.StopStopwatch("Validator");
        }
    }

    public FileCabinetServiceBase Service { get; set; }

    public ServiceMeter(FileCabinetServiceBase service)
    {
        this.Service = service;
        this.Service.Validator = service.Validator;
    }

    public override int CreateRecord(FileCabinetRecord record)
    {
        this.StartStopwatch();
        var result = this.Service.CreateRecord(record);
        this.StopStopwatch("Create");
        return result;
    }

    public override void EditRecord(FileCabinetRecord record, int index)
    {
        this.StartStopwatch();
        this.Service.EditRecord(record, index);
        this.StopStopwatch("Edit");
    }

    public override IEnumerable<FileCabinetRecord> FindByDateOfBirth(string date)
    {
        this.StartStopwatch();
        var result = this.Service.FindByDateOfBirth(date);
        this.StopStopwatch("Find dateofbirth");
        return result;
    }

    public override IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
    {
        this.StartStopwatch();
        var result = this.Service.FindByFirstName(firstName);
        this.StopStopwatch("Find firstname");
        return result;
    }

    public override IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
    {
        this.StartStopwatch();
        var result = this.Service.FindByLastName(lastName);
        this.StopStopwatch("Find lastname");
        return result;
    }

    public override IEnumerable<FileCabinetRecord> FindByBalance(string lastName)
    {
        this.StartStopwatch();
        var result = this.Service.FindByBalance(lastName);
        this.StopStopwatch("Find balance");
        return result;
    }

    public override IEnumerable<FileCabinetRecord> FindByWorkExperience(string lastName)
    {
        this.StartStopwatch();
        var result = this.Service.FindByWorkExperience(lastName);
        this.StopStopwatch("Find lastname");
        return result;
    }

    public override IEnumerable<FileCabinetRecord> FindByFavLetter(string lastName)
    {
        this.StartStopwatch();
        var result = this.Service.FindByFavLetter(lastName);
        this.StopStopwatch("Find lastname");
        return result;
    }

    public override IEnumerable<FileCabinetRecord> FindById(string lastName)
    {
        this.StartStopwatch();
        var result = this.Service.FindById(lastName);
        this.StopStopwatch("Find lastname");
        return result;
    }

    public override int FindRecordIndexById(int id)
    {
        return this.Service.FindRecordIndexById(id);
    }

    public override IEnumerable<FileCabinetRecord> GetRecords()
    {
        this.StartStopwatch();
        var result = this.Service.GetRecords();
        this.StopStopwatch("List");
        return result;
    }

    public override (int, int) GetStat()
    {
        this.StartStopwatch();
        var result = this.Service.GetStat();
        this.StopStopwatch("Stat");
        return result;
    }

    public override FileCabinetServiceSnapshot MakeSnapshot()
    {
        return this.Service.MakeSnapshot();
    }

    public override void RemoveRecord(int id)
    {
        this.StartStopwatch();
        this.Service.RemoveRecord(id);
        this.StopStopwatch("Remove");
    }

    public override IEnumerable<FileCabinetRecord> Find(string parameters)
    {
        this.StartStopwatch();
        var result = this.Service.Find(parameters);
        this.StopStopwatch("Find");
        return result;
    }

    public override void Restore(FileCabinetServiceSnapshot snapshot)
    {
        this.Service.Restore(snapshot);
    }

    private void StartStopwatch()
    {
        this.stopwatch.Reset();
        this.stopwatch.Start();
    }

    public override (int, int) Purge()
    {
        this.StartStopwatch();
        var result = this.Service.Purge();
        this.StopStopwatch("Purge");
        return result;
    }

    private void StopStopwatch(string methodName)
    {
        this.stopwatch.Stop();
        Console.WriteLine($"{methodName} method execution duration {this.stopwatch.ElapsedTicks} ticks.");
    }
}