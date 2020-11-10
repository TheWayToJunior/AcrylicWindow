using AcrylicWindow.IContract;
using AcrylicWindow.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcrylicWindow.Services
{
    public class EmployeeService : IEmployeeService
    {
        public async Task<IEnumerable<Employee>> GetAll()
        {
            var list = new List<Employee>()
            {
                new Employee
                {
                    Id = 1, Name = "Смоленский М.С",
                    Position = "Backend",
                    Phone = "071-311-25-29",
                    Email = "miha.smoelnsky2000@mail.ru",
                    Img = "https://sun2.48276.userapi.com/c629508/v629508849/e7f2/RMYOFC_9YDg.jpg"
                },

                 new Employee
                 {
                    Id = 2, Name = "Филин Д.С",
                    Position = "FullStack",
                    Phone = "071-329-75-31",
                    Email = "Dema.saw12q1wqsa@mail.ru",
                    Img = "https://sun9-55.userapi.com/c841639/v841639638/399c0/r3jChpCwgAE.jpg"
                 },

                 new Employee
                 {
                    Id = 3, Name = "Мелиневский Р.В",
                    Position = "Frontend",
                    Phone = "071-316-11-17",
                    Email = "Roro.195623@mail.ru",
                    Img = "https://sun9-56.userapi.com/vr7-StutElseiY4lR19Lpuz43SkyPRHCJPICxg/5MWXrvYwhMQ.jpg"
                 }
            };

            return await Task.FromResult(list);
        }
    }
}
