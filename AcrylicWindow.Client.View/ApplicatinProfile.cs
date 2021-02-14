using AcrylicWindow.Client.Core.Model;
using AcrylicWindow.Client.Data.Entities;
using AutoMapper;

namespace AcrylicWindow
{
    internal class ApplicatinProfile : Profile
    {
        public ApplicatinProfile()
        {
            CreateMap<EmployeeEntity, Employee>();
            CreateMap<Employee, EmployeeEntity>();
        }
    }
}