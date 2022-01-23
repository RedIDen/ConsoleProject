namespace FileCabinetApp.CommandHandlers;

public class UseLoggerCommandHandler : ServiceCommandHandlerBase
{
    private bool useLogger = false;

    public UseLoggerCommandHandler(FileCabinetTrasferHelper service)
        : base(service)
    {
    }

    protected override string[] CommandNames { get; } = { "use-logger" };

    protected override void MakeWork(string parameters)
    {
        if (this.useLogger)
        {
            var temp = this.transferHelper.Service;
            if (temp is ServiceLogger meter1)
            {
                this.transferHelper.Service = meter1.Service;
            }
            else
            {
                while (temp is IServiceDecorator decorator)
                {
                    if (decorator.Service is ServiceLogger meter2)
                    {
                        decorator.Service = meter2.Service;
                        break;
                    }
                }
            }

            this.useLogger = false;
            Console.WriteLine("Logger disabled.");
        }
        else
        {
            this.transferHelper.Service = new ServiceLogger(this.transferHelper.Service);
            this.useLogger = true;
            Console.WriteLine($"Logger enabled. Logs are stored in {ServiceLogger.FileName}.");
        }
    }
}
