namespace FileCabinetApp.CommandHandlers;

/// <summary>
/// The use-stopwatch command handler.
/// </summary>
internal class UseStopwatchCommandHandler : ServiceCommandHandlerBase
{
    private bool useStopwatch;

    /// <summary>
    /// Initializes a new instance of the <see cref="UseStopwatchCommandHandler"/> class.
    /// </summary>
    /// <param name="service">Transfer helper.</param>
    public UseStopwatchCommandHandler(FileCabinetTrasferHelper service)
        : base(service)
    {
    }

    /// <summary>
    /// Gets the list of command names (only full or full and short).
    /// </summary>
    /// <value>
    /// The list of command names (strings).
    /// </value>
    protected override string[] CommandNames { get; } = { "use-stopwatch" };

    /// <summary>
    /// Enables or disables the stopwatch.
    /// </summary>
    /// <param name="parameters">Command parameters.</param>
    protected override void MakeWork(string parameters)
    {
        if (this.useStopwatch)
        {
            var temp = this.transferHelper.Service;
            if (temp is ServiceMeter meter1)
            {
                this.transferHelper.Service = meter1.Service;
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
            this.transferHelper.Service = new ServiceMeter(this.transferHelper.Service);
            this.useStopwatch = true;
            Console.WriteLine("Stopwatch enabled.");
        }
    }
}
