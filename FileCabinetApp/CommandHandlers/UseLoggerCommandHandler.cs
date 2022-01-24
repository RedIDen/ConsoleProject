namespace FileCabinetApp.CommandHandlers;

/// <summary>
/// The use-logger command handler.
/// </summary>
internal class UseLoggerCommandHandler : ServiceCommandHandlerBase
{
    private bool useLogger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UseLoggerCommandHandler"/> class.
    /// </summary>
    /// <param name="service">Transfer helper.</param>
    public UseLoggerCommandHandler(FileCabinetTrasferHelper service)
        : base(service)
    {
    }

    /// <summary>
    /// Gets the list of command names (only full or full and short).
    /// </summary>
    /// <value>
    /// The list of command names (strings).
    /// </value>
    protected override string[] CommandNames { get; } = { "use-logger" };

    /// <summary>
    /// Enables or disables logger.
    /// </summary>
    /// <param name="parameters">Command parameters.</param>
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
