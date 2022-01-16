using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class ServiceLogger : IServiceDecorator
    {
        public const string FileName = "log.txt";

        public IFileCabinetService Service { get; set; }

        public IRecordValidator Validator
        {
            get => this.Service.Validator;
            set => this.Service.Validator = value;
        }

        public ServiceLogger(IFileCabinetService service)
        {
            this.Service = service;
            this.Service.Validator = service.Validator;
        }

        public int CreateRecord(FileCabinetRecord record)
        {
            this.WriteLog($"Calling Create() with FirstName = '{record.FirstName}', LastName = '{record.LastName}', DateOfBirth = '{record.DateOfBirth:MM/dd/yyyy}', Balance = '{record.Balance}', WorkExperience = '{record.WorkExperience}', FavLetter = '{record.FavLetter}'");
            var result = this.Service.CreateRecord(record);
            this.WriteLog($"Create() returned '{result}'");
            return result;
        }

        public void EditRecord(FileCabinetRecord record, int index)
        {
            this.WriteLog($"Calling Edit() for Index = '{index}' with FirstName = '{record.FirstName}', LastName = '{record.LastName}', DateOfBirth = '{record.DateOfBirth:MM/dd/yyyy}', Balance = '{record.Balance}', WorkExperience = '{record.WorkExperience}', FavLetter = '{record.FavLetter}'");
            this.Service.EditRecord(record, index);
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string date)
        {
            this.WriteLog($"Calling FindByDateOfBirth() with Date = '{date}'");
            var result = this.Service.FindByDateOfBirth(date);
            this.WriteLog($"FindByDateOfBirth() returned list with Size = '{result.Count}'");
            return result;
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            this.WriteLog($"Calling FindByFirstName() with Date = '{firstName}'");
            var result = this.Service.FindByFirstName(firstName);
            this.WriteLog($"FindByFirstName() returned list with Size = '{result.Count}'");
            return result;
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            this.WriteLog($"Calling FindByLastName() with Date = '{lastName}'");
            var result = this.Service.FindByLastName(lastName);
            this.WriteLog($"FindByLastName() returned list with Size = '{result.Count}'");
            return result;
        }

        public int FindRecordIndexById(int id)
        {
            return this.Service.FindRecordIndexById(id);
        }

        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            this.WriteLog($"Calling GetRecords()");
            var result = this.Service.GetRecords();
            this.WriteLog($"GetRecords() returned list with Size = '{result.Count}'");
            return result;
        }

        public (int, int) GetStat()
        {
            this.WriteLog($"Calling GetStat()");
            var result = this.Service.GetStat();
            this.WriteLog($"GetStat() returned '{result.Item1}', '{result.Item2}'");
            return result;
        }

        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            throw new NotImplementedException();
        }

        public (int, int) Purge()
        {
            this.WriteLog($"Calling Purge()");
            var result = this.Service.Purge();
            this.WriteLog($"Purge() returned '{result.Item1}', '{result.Item2}'");
            return result;
        }

        public void RemoveRecord(int id)
        {
            this.WriteLog($"Calling RemoveRecord() with Id = '{id}'");
            this.Service.RemoveRecord(id);
        }

        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            throw new NotImplementedException();
        }

        private void WriteLog(string log)
        {
            using (var writer = new StreamWriter(path: ServiceLogger.FileName, append: true))
            {
                writer.Write(DateTime.Now.ToString("MM/dd/yyyy HH:mm"));
                writer.Write(" - ");
                writer.WriteLine(log);
            }
        }
    }
}
