using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public interface IServiceDecorator : IFileCabinetService
    {
        public IFileCabinetService Service { get; set; }

        public IFileCabinetService GetLast()
        {
            return this.Service is IServiceDecorator ? ((IServiceDecorator)this.Service).GetLast() : this.Service;
        }
    }
}
