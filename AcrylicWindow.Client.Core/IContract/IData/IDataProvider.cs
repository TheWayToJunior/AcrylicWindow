namespace AcrylicWindow.Client.Data
{
    public interface IDataProvider
    {
        IEmployeeRepository Employees { get; }
    }
}
