using AcrylicWindow.Client.Core.Model;
using AcrylicWindow.Client.Data;
using System;
using System.Threading.Tasks;

namespace AcrylicWindow
{
    public class InitializeDatabase
    {
        public static async Task Initialize(IDataProvider provider)
        {
            var res = await provider.Employees.FindAsync<Employee, string>(p => p.Name, "Смоленский М.С");

            if(res == null)
            {
                await provider.Employees.InsertAsync(new Employee
                {
                    Id = Guid.NewGuid(),
                    Name = "Смоленский М.С",
                    Position = "Backend",
                    Phone = "071-311-25-29",
                    Email = "miha.smoelnsky2000@mail.ru",
                    Img = "https://sun2.48276.userapi.com/c629508/v629508849/e7f2/RMYOFC_9YDg.jpg"
                });
            }    
        }
    }
}
