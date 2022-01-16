using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class ServiceMeter : IServiceDecorator
    {
        private readonly Stopwatch stopwatch = new Stopwatch();

        public IRecordValidator Validator { get => this.Service.Validator;
            set
            {
                this.StartStopwatch();
                this.Service.Validator = value;
                this.StopStopwatch("Validator");
            }
        }

        public IFileCabinetService Service { get; set; }

        public ServiceMeter(IFileCabinetService service)
        {
            this.Service = service;
            this.Service.Validator = service.Validator;
        }

        public int CreateRecord(FileCabinetRecord record)
        {
            this.StartStopwatch();
            var result = this.Service.CreateRecord(record);
            this.StopStopwatch("Create");
            return result;
        }

        public void EditRecord(FileCabinetRecord record, int index)
        {
            this.StartStopwatch();
            this.Service.EditRecord(record, index);
            this.StopStopwatch("Edit");
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string date)
        {
            this.StartStopwatch();
            var result = this.Service.FindByDateOfBirth(date);
            this.StopStopwatch("Find dateofbirth");
            return result;
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            this.StartStopwatch();
            var result = this.Service.FindByFirstName(firstName);
            this.StopStopwatch("Find firstname");
            return result;
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            this.StartStopwatch();
            var result = this.Service.FindByLastName(lastName);
            this.StopStopwatch("Find lastname");
            return result;
        }

        public int FindRecordIndexById(int id)
        {
            return this.Service.FindRecordIndexById(id);
        }

        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            this.StartStopwatch();
            var result = this.Service.GetRecords();
            this.StopStopwatch("List");
            return result;
        }

        public (int, int) GetStat()
        {
            this.StartStopwatch();
            var result = this.Service.GetStat();
            this.StopStopwatch("Stat");
            return result;
        }

        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            return this.Service.MakeSnapshot();
        }

        public void RemoveRecord(int id)
        {
            this.StartStopwatch();
            this.Service.RemoveRecord(id);
            this.StopStopwatch("Remove");
        }

        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            this.Service.Restore(snapshot);
        }

        private void StartStopwatch()
        {
            this.stopwatch.Reset();
            this.stopwatch.Start();
        }

        private void StopStopwatch(string methodName)
        {
            this.stopwatch.Stop();
            Console.WriteLine($"{methodName} method execution duration {this.stopwatch.ElapsedTicks} ticks.");
        }

        public (int, int) Purge()
        {
            this.StartStopwatch();
            var result = this.Service.Purge();
            this.StopStopwatch("Purge");
            return result;
        }
    }
}
