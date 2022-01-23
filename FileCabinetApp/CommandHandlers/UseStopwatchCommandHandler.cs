namespace FileCabinetApp.CommandHandlers;

public class UseStopwatchCommandHandler : ServiceCommandHandlerBase
{
    private bool useStopwatch = false;

    public UseStopwatchCommandHandler(FileCabinetTrasferHelper service)
        : base(service)
    {
    }

    protected override string[] CommandNames { get; } = { "use-stopwatch" };

    protected override void MakeWork(string parameters)
    {
        if (this.useStopwatch)
        {
            var temp = this.service.Service;
            if (temp is ServiceMeter meter1)
            {
                this.service.Service = meter1.Service;
            }
            else
            {
                while (temp is IServiceDecorator decorator)
                {
                    if (decorator.Service is ServiceMeter meter2)
                    {
                        decorator.Service = meter2.Service;
                        break;
                    }
                }
            }

            this.useStopwatch = false;
            Console.WriteLine("Stopwatch disabled.");
        }
        else
        {
            this.service.Service = new ServiceMeter(this.service.Service);
            this.useStopwatch = true;
            Console.WriteLine("Stopwatch enabled.");
        }
    }
}
