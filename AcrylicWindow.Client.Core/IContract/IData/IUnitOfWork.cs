namespace AcrylicWindow.Client.Core.IContract.IData
{
    public interface IUnitOfWork
    {
        IEmployeeRepository Employees { get; }

        IStudentRepository Students { get; }

        IGroupRepository Groups { get; }
    }
}
