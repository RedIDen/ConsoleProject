﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    public interface ICommandHandler
    {
        public void SetNext(ICommandHandler commandHandler);

        public void Handle(AppCommandRequest appCommandRequest);
    }
}
