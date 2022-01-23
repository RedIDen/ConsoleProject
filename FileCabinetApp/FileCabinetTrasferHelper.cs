namespace FileCabinetApp;

public class FileCabinetTrasferHelper
{
    public FileCabinetServiceBase Service { get; set; }

    public FileCabinetTrasferHelper(FileCabinetServiceBase service)
    {
        this.Service = service;
    }

    public IFileCabinetService GetLast()
    {
        return this.Service is IServiceDecorator ? ((IServiceDecorator)this.Service).GetLast() : this.Service;
    }

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