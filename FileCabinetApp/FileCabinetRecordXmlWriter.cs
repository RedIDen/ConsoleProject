using System.Globalization;
using System.Xml;

namespace FileCabinetApp
{
    /// <summary>
    /// The file cabinet record XML writer.
    /// </summary>
    public class FileCabinetRecordXmlWriter
    {
        private readonly XmlWriter xmlWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlWriter"/> class.
        /// </summary>
        /// <param name="textWriter">The TextWriter object to work with.</param>
        public FileCabinetRecordXmlWriter(TextWriter textWriter)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            this.xmlWriter = XmlWriter.Create(textWriter, settings);
        }

        /// <summary>
        /// Wtires the beginning of the list.
        /// </summary>
        public void WriteBegin() => this.xmlWriter.WriteStartElement("records");

        /// <summary>
        /// Wtires the ending of the list.
        /// </summary>
        public void WriteEnd() => this.xmlWriter.WriteEndElement();

        /// <summary>
        /// Wtites the record into the file.
        /// </summary>
        /// <param name="record">The record to wtire.</param>
        public void Write(FileCabinetRecord record)
        {
            this.xmlWriter.WriteStartElement("record");
            this.xmlWriter.WriteAttributeString("id", record.Id.ToString());

            this.xmlWriter.WriteStartElement("name");
            this.xmlWriter.WriteAttributeString("first", record.FirstName);
            this.xmlWriter.WriteAttributeString("last", record.LastName);
            this.xmlWriter.WriteEndElement();

            this.xmlWriter.WriteElementString("dateOfBirth", record.DateOfBirth.ToString("MM/dd/yyyy", CultureInfo.CreateSpecificCulture("en-US")));

            this.xmlWriter.WriteElementString("workExperience", record.WorkExperience.ToString());

            this.xmlWriter.WriteElementString("balance", record.Balance.ToString());

            this.xmlWriter.WriteElementString("favLetter", record.FavLetter.ToString());

            this.xmlWriter.WriteEndElement();
        }

        /// <summary>
        /// Closes the current writer.
        /// </summary>
        public void Close()
        {
            this.xmlWriter.Close();
        }
    }
}