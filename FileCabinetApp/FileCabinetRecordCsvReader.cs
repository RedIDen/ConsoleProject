#pragma warning disable CS8602

using System.Collections.ObjectModel;
using System.Globalization;

namespace FileCabinetApp
{
    public class FileCabinetRecordCsvReader
    {
        private StreamReader stream;

        public FileCabinetRecordCsvReader(StreamReader stream)
        {
            this.stream = stream;
        }

        public ReadOnlyCollection<FileCabinetRecord> ReadAll()
        {
            var list = new List<FileCabinetRecord>();

            while (!this.stream.EndOfStream)
            {
                string[] data = this.stream.ReadLine().Split(',');

                list.Add(new FileCabinetRecord
                {
                    Id = int.Parse(data[0]),
                    FirstName = data[1],
                    LastName = data[2],
                    DateOfBirth = DateTime.Parse(data[3], CultureInfo.CreateSpecificCulture("en-US"), DateTimeStyles.None),
                    Balance = decimal.Parse(data[4]),
                    FavLetter = char.Parse(data[5]),
                    WorkExperience = short.Parse(data[6]),
                });
            }

            return new ReadOnlyCollection<FileCabinetRecord>(list);
        }
    }
}