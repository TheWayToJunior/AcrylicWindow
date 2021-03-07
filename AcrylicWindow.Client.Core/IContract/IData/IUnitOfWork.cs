namespace AcrylicWindow.Client.Core.IContract.IData
{
    public interface IUnitOfWork
    {
        IEmployeeRepository Employees { get; }
    }
}
