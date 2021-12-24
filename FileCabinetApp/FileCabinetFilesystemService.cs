using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    internal class FileCabinetFilesystemService : IFileCabinetService
    {
        private const int RECORDLENGTH = 275;
        public const string FILENAME = "cabinet-records.db";
        private readonly FileStream fileStream;
        private readonly BinaryWriter writer;
        private readonly BinaryReader reader;

        public FileCabinetFilesystemService(IRecordValidator recordValidator, FileStream fileStream)
        {
            this.Validator = recordValidator;
            this.fileStream = fileStream;
            this.writer = new BinaryWriter(this.fileStream);
            this.reader = new BinaryReader(this.fileStream);
        }

        ~FileCabinetFilesystemService()
        {
            this.Close();
        }

        public IRecordValidator Validator { get; set; }

        public void Close()
        {
            this.reader.Close();
            this.writer.Close();
            this.fileStream.Close();
        }

        public int CreateRecord(FileCabinetRecord record)
        {
            int id = (int)(this.fileStream.Length / RECORDLENGTH) + 1;

            this.writer.BaseStream.Position = this.writer.BaseStream.Length;

            this.writer.Write(id);

            byte[] firstNameBytes = new byte[120];
            var temp1 = Encoding.Default.GetBytes(record.FirstName);
            Array.Copy(temp1, firstNameBytes, temp1.Length);
            this.writer.Write(firstNameBytes);

            byte[] lastNameBytes = new byte[120];
            var temp2 = Encoding.Default.GetBytes(record.LastName);
            Array.Copy(temp2, lastNameBytes, temp2.Length);
            this.writer.Write(lastNameBytes);

            this.writer.Write(record.DateOfBirth.Year);

            this.writer.Write(record.DateOfBirth.Month);

            this.writer.Write(record.DateOfBirth.Day);

            this.writer.Write(record.Balance);

            this.writer.Write(record.WorkExperience);

            this.writer.Write(record.FavLetter);

            return id;
        }

        public void EditRecord(FileCabinetRecord record, int index)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string date)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            throw new NotImplementedException();
        }

        public int FindRecordIndexById(int id)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            var list = new List<FileCabinetRecord>();

            this.reader.BaseStream.Position = 0;

            while (this.reader.BaseStream.Position < this.reader.BaseStream.Length)
            {
                var record = new FileCabinetRecord();

                record.Id = this.reader.ReadInt32();

                record.FirstName = Encoding.Default.GetString(this.reader.ReadBytes(120));

                record.LastName = Encoding.Default.GetString(this.reader.ReadBytes(120));

                record.DateOfBirth = new DateTime(this.reader.ReadInt32(), this.reader.ReadInt32(), this.reader.ReadInt32());

                record.Balance = this.reader.ReadDecimal();

                record.WorkExperience = this.reader.ReadInt16();

                record.FavLetter = this.reader.ReadChar();

                list.Add(record);
            }

            return new ReadOnlyCollection<FileCabinetRecord>(list);
        }

        public int GetStat()
        {
            throw new NotImplementedException();
        }

        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            throw new NotImplementedException();
        }
    }
}
