using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class ServiceLogger : FileCabinetServiceBase
    {
        public const string FileName = "log.txt";

        public FileCabinetServiceBase Service { get; set; }

        public override IRecordValidator Validator
        {
            get => this.Service.Validator;
            set => this.Service.Validator = value;
        }

        public ServiceLogger(FileCabinetServiceBase service)
        {
            this.Service = service;
            this.Service.Validator = service.Validator;
        }

        public override int CreateRecord(FileCabinetRecord record)
        {
            this.WriteLog($"Calling Create() with FirstName = '{record.FirstName}', LastName = '{record.LastName}', DateOfBirth = '{record.DateOfBirth:MM/dd/yyyy}', Balance = '{record.Balance}', WorkExperience = '{record.WorkExperience}', FavLetter = '{record.FavLetter}'");
            var result = this.Service.CreateRecord(record);
            this.WriteLog($"Create() returned '{result}'");
            return result;
        }

        public override void EditRecord(FileCabinetRecord record, int index)
        {
            this.WriteLog($"Calling Edit() for Index = '{index}' with FirstName = '{record.FirstName}', LastName = '{record.LastName}', DateOfBirth = '{record.DateOfBirth:MM/dd/yyyy}', Balance = '{record.Balance}', WorkExperience = '{record.WorkExperience}', FavLetter = '{record.FavLetter}'");
            this.Service.EditRecord(record, index);
        }

        public override IEnumerable<FileCabinetRecord> FindByDateOfBirth(string date)
        {
            this.WriteLog($"Calling FindByDateOfBirth() with Date = '{date}'");
            var result = this.Service.FindByDateOfBirth(date);
            this.WriteLog($"FindByDateOfBirth() returned list with Size = '{result.Count()}'");
            return result;
        }

        public override IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            this.WriteLog($"Calling FindByFirstName() with Date = '{firstName}'");
            var result = this.Service.FindByFirstName(firstName);
            this.WriteLog($"FindByFirstName() returned list with Size = '{result.Count()}'");
            return result;
        }

        public override IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            this.WriteLog($"Calling FindByLastName() with Date = '{lastName}'");
            var result = this.Service.FindByLastName(lastName);
            this.WriteLog($"FindByLastName() returned list with Size = '{result.Count()}'");
            return result;
        }

        public override IEnumerable<FileCabinetRecord> Find(string parameters)
        {
            this.WriteLog($"Calling Find() with Parameters = '{parameters}'");
            var result = this.Service.Find(parameters);
            this.WriteLog($"Find() returned list with Size = '{result.Count()}'");
            return result;
        }

        public override IEnumerable<FileCabinetRecord> FindByBalance(string lastName)
        {
            this.WriteLog($"Calling FindByBalance() with Date = '{lastName}'");
            var result = this.Service.FindByBalance(lastName);
            this.WriteLog($"FindByBalance() returned list with Size = '{result.Count()}'");
            return result;
        }

        public override IEnumerable<FileCabinetRecord> FindByWorkExperience(string lastName)
        {
            this.WriteLog($"Calling FindByWorkExperience() with Date = '{lastName}'");
            var result = this.Service.FindByWorkExperience(lastName);
            this.WriteLog($"FindByWorkExperience() returned list with Size = '{result.Count()}'");
            return result;
        }

        public override IEnumerable<FileCabinetRecord> FindByFavLetter(string lastName)
        {
            this.WriteLog($"Calling FindByFavLetter() with Date = '{lastName}'");
            var result = this.Service.FindByFavLetter(lastName);
            this.WriteLog($"FindByFavLetter() returned list with Size = '{result.Count()}'");
            return result;
        }

        public override IEnumerable<FileCabinetRecord> FindById(string lastName)
        {
            this.WriteLog($"Calling FindById() with Date = '{lastName}'");
            var result = this.Service.FindById(lastName);
            this.WriteLog($"FindById() returned list with Size = '{result.Count()}'");
            return result;
        }

        public override int FindRecordIndexById(int id)
        {
            return this.Service.FindRecordIndexById(id);
        }

        public override IEnumerable<FileCabinetRecord> GetRecords()
        {
            this.WriteLog($"Calling GetRecords()");
            var result = this.Service.GetRecords();
            this.WriteLog($"GetRecords() returned list with Size = '{result.Count()}'");
            return result;
        }

        public override (int, int) GetStat()
        {
            this.WriteLog($"Calling GetStat()");
            var result = this.Service.GetStat();
            this.WriteLog($"GetStat() returned '{result.Item1}', '{result.Item2}'");
            return result;
        }

        public override FileCabinetServiceSnapshot MakeSnapshot()
        {
            throw new NotImplementedException();
        }

        public override (int, int) Purge()
        {
            this.WriteLog($"Calling Purge()");
            var result = this.Service.Purge();
            this.WriteLog($"Purge() returned '{result.Item1}', '{result.Item2}'");
            return result;
        }

        public override void RemoveRecord(int id)
        {
            this.WriteLog($"Calling RemoveRecord() with Id = '{id}'");
            this.Service.RemoveRecord(id);
        }

        public override void Restore(FileCabinetServiceSnapshot snapshot)
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
