using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// The intarface for the record validators.
    /// </summary>
    public interface IRecordValidator
    {
        public (bool, string) Validate(FileCabinetRecord record);
    }
}
