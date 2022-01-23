namespace FileCabinetApp;

public interface IServiceDecorator : IFileCabinetService
{
    public FileCabinetServiceBase Service { get; set; }

    public FileCabinetServiceBase GetLast()
    {
        return this.Service is IServiceDecorator ? ((IServiceDecorator)this.Service).GetLast() : this.Service;
    }
}
