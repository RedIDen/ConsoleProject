using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public interface IServiceDecorator : IFileCabinetService
    {
        public FileCabinetServiceBase Service { get; set; }

        public FileCabinetServiceBase GetLast()
        {
            return this.Service is IServiceDecorator ? ((IServiceDecorator)this.Service).GetLast() : this.Service;
        }
    }
}
