using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class FileCabinetTrasferHelper
    {
        public IFileCabinetService Service { get; set; }

        public FileCabinetTrasferHelper(IFileCabinetService service)
        {
            this.Service = service;
        }

        public IFileCabinetService GetLast()
        {
            return this.Service is IServiceDecorator ? ((IServiceDecorator)this.Service).GetLast() : this.Service;
        }

        public void SetLast(IFileCabinetService service)
        {
            var temp = this.Service;
            if (this.Service is IServiceDecorator)
            {
                while (true)
                {
                    if (((IServiceDecorator)temp).Service is IServiceDecorator)
                    {
                        temp = ((IServiceDecorator)temp).Service;
                    }
                    else
                    {
                        ((IServiceDecorator)temp).Service = service;
                        return;
                    }
                }
            }
        }
    }
}
