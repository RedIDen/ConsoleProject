using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// The intarface for the record validators.
    /// </summary>
    public interface IRecordValidator
    {
        /// <summary>
        /// The method to validate the record's data.
        /// </summary>
        /// <param name="record">Record.</param>
        public void ValidateParameters(FileCabinetRecord record);
    }
}
