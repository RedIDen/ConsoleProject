using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// The file cabinet service snapshot.
    /// </summary>
    public class FileCabinetServiceSnapshot
    {
        private readonly List<FileCabinetRecord> list;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.
        /// </summary>
        /// <param name="list">The list of records.</param>
        public FileCabinetServiceSnapshot(List<FileCabinetRecord> list)
        {
            this.list = list;
        }

        /// <summary>
        /// Saves the snapshot to the CSV file.
        /// </summary>
        /// <param name="stream">The stream to work with.</param>
        public void SaveToCsv(StreamWriter stream)
        {
            var fileCabinetRecordCsvWriter = new FileCabinetRecordCsvWriter(stream);

            foreach (var record in this.list)
            {
                fileCabinetRecordCsvWriter.Write(record);
            }

            fileCabinetRecordCsvWriter.Close();
        }

        /// <summary>
        /// Saves the snapshot to the XML file.
        /// </summary>
        /// <param name="stream">The stream to work with.</param>
        public void SaveToXml(StreamWriter stream)
        {
            var fileCabinetRecordXmlWriter = new FileCabinetRecordXmlWriter(stream);

            fileCabinetRecordXmlWriter.WriteBegin();

            foreach (var record in this.list)
            {
                fileCabinetRecordXmlWriter.Write(record);
            }

            fileCabinetRecordXmlWriter.WriteEnd();

            fileCabinetRecordXmlWriter.Close();
        }
    }
}
