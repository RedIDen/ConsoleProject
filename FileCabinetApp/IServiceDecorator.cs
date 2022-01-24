namespace FileCabinetApp;

/// <summary>
/// The service decorator.
/// </summary>
internal interface IServiceDecorator : IFileCabinetService
{
    /// <summary>
    /// Gets or sets service to decorate.
    /// </summary>
    /// <value>
    /// FileCabinetServiceBase object.
    /// </value>
    public FileCabinetServiceBase Service { get; set; }

    /// <summary>
    /// Gets the last service in the chain of decorators.
    /// </summary>
    /// <returns>The last service in the chain of decorators.</returns>
    public FileCabinetServiceBase GetLast()
    {
        return this.Service is IServiceDecorator decorator ? decorator.GetLast() : this.Service;
    }
}
