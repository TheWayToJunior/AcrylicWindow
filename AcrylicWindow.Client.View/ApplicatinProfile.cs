using AcrylicWindow.Client.Core.Models;
using AcrylicWindow.Client.Entity.Entities;
using AutoMapper;

namespace AcrylicWindow
{
    public class ApplicatinProfile : Profile
    {
        public ApplicatinProfile()
        {
            CreateMap<EmployeeEntity, Employee>();

            CreateMap<Employee, EmployeeEntity>()
                .ForMember(p => p.CreatedBy, opt => opt.Ignore())
                .ForMember(p => p.UpdatedBy, opt => opt.Ignore());

            CreateMap<StudentEntity, Student>();

            CreateMap<Student, StudentEntity>()
                .ForMember(p => p.CreatedBy, opt => opt.Ignore())
                .ForMember(p => p.UpdatedBy, opt => opt.Ignore());
        }
    }
}