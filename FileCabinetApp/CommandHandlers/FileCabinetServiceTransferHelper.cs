using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    public class FileCabinetServiceTransferHelper
    {
        public IFileCabinetService fileCabinetService { get; set; }

        public FileCabinetServiceTransferHelper(IFileCabinetService fileCabinetService)
        {
            this.fileCabinetService = fileCabinetService;
        }
    }
}
