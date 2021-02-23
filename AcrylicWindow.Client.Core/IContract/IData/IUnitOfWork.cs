namespace AcrylicWindow.Client.Data
{
    public interface IUnitOfWork
    {
        IEmployeeRepository Employees { get; }
    }
}
