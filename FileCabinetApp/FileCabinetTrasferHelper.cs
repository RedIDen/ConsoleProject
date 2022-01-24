namespace FileCabinetApp;

/// <summary>
/// The file cabinet transfer helper. This class is used to inject the service dependency to command handlers with the posibility to change service.
/// </summary>
internal class FileCabinetTrasferHelper
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FileCabinetTrasferHelper"/> class.
    /// </summary>
    /// <param name="service">Service.</param>
    public FileCabinetTrasferHelper(FileCabinetServiceBase service)
    {
        this.Service = service;
    }

    /// <summary>
    /// Gets or sets service.
    /// </summary>
    /// <value>
    /// FileCabinetServiceBase object.
    /// </value>
    public FileCabinetServiceBase Service { get; set; }

    /// <summary>
    /// Gets the last service in the chain of decorators.
    /// </summary>
    /// <returns>The last service in the chain of decorators.</returns>
    public IFileCabinetService GetLast()
    {
        return this.Service is IServiceDecorator decorator ? decorator.GetLast() : this.Service;
    }

    /// <summary>
    /// Sets the last service in the chain of decorators.
    /// </summary>
    /// <param name="service">New service.</param>
    public void SetLast(FileCabinetServiceBase service)
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
        else
        {
            this.Service = service;
        }
    }
}