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
            record.Id = (int)(this.fileStream.Length / RECORDLENGTH) + 1;

            this.WriteRecord(record, this.writer.BaseStream.Length);

            return record.Id;
        }

        public void EditRecord(FileCabinetRecord record, int index)
        {
            int position = index * RECORDLENGTH;
            this.reader.BaseStream.Position = position;
            record.Id = this.reader.ReadInt32();
            this.WriteRecord(record, position);
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
            this.reader.BaseStream.Position = 0;

            while (this.reader.BaseStream.Position < this.reader.BaseStream.Length)
            {
                int currentId = this.reader.ReadInt32();

                if (id == currentId)
                {
                    return (int)(this.reader.BaseStream.Position / RECORDLENGTH);
                }

                this.reader.BaseStream.Position += RECORDLENGTH - 4;
            }

            return -1;
        }

        public ReadOnlyCollection<FileCabinetRecord> GetRecords() => new ReadOnlyCollection<FileCabinetRecord>(this.GetListOfRecords());

        public int GetStat() => (int)(this.fileStream.Length / RECORDLENGTH);

        public FileCabinetServiceSnapshot MakeSnapshot() => new FileCabinetServiceSnapshot(this.GetListOfRecords());

        private List<FileCabinetRecord> GetListOfRecords()
        {
            var list = new List<FileCabinetRecord>();

            this.reader.BaseStream.Position = 0;

            while (this.reader.BaseStream.Position < this.reader.BaseStream.Length)
            {
                var record = new FileCabinetRecord();

                record.Id = this.reader.ReadInt32();

                record.FirstName = Encoding.Unicode.GetString(this.reader.ReadBytes(120)).Trim('\0');

                record.LastName = Encoding.Unicode.GetString(this.reader.ReadBytes(120)).Trim('\0');

                record.DateOfBirth = new DateTime(this.reader.ReadInt32(), this.reader.ReadInt32(), this.reader.ReadInt32());

                record.Balance = this.reader.ReadDecimal();

                record.WorkExperience = this.reader.ReadInt16();

                record.FavLetter = this.reader.ReadChar();

                list.Add(record);
            }

            return list;
        }

        private void WriteRecord(FileCabinetRecord record, long position)
        {
            this.writer.BaseStream.Position = position;

            this.writer.Write(record.Id);

            byte[] firstNameBytes = new byte[120];
            var temp1 = Encoding.Unicode.GetBytes(record.FirstName);
            Array.Copy(temp1, firstNameBytes, temp1.Length);
            this.writer.Write(firstNameBytes);

            byte[] lastNameBytes = new byte[120];
            var temp2 = Encoding.Unicode.GetBytes(record.LastName);
            Array.Copy(temp2, lastNameBytes, temp2.Length);
            this.writer.Write(lastNameBytes);

            this.writer.Write(record.DateOfBirth.Year);

            this.writer.Write(record.DateOfBirth.Month);

            this.writer.Write(record.DateOfBirth.Day);

            this.writer.Write(record.Balance);

            this.writer.Write(record.WorkExperience);

            this.writer.Write(record.FavLetter);
        }
    }
}
