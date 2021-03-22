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
                .ForMember(ee => ee.CreatedBy, opt => opt.Ignore())
                .ForMember(ee => ee.UpdatedBy, opt => opt.Ignore());

            CreateMap<StudentEntity, Student>();

            CreateMap<Student, StudentEntity>()
                .ForMember(se => se.CreatedBy, opt => opt.Ignore())
                .ForMember(se => se.UpdatedBy, opt => opt.Ignore());

            CreateMap<GroupEntity, Group>()
                .ForMember(p => p.Teacher, opt => opt.Ignore())
                .ForMember(p => p.Students, opt => opt.Ignore());

            CreateMap<GroupCreate, GroupEntity>()
                .ForMember(ee => ee.CreatedBy, opt => opt.Ignore())
                .ForMember(ee => ee.UpdatedBy, opt => opt.Ignore());

            CreateMap<GroupUpdate, GroupEntity>()
                .ForMember(ee => ee.CreatedBy, opt => opt.Ignore())
                .ForMember(ee => ee.UpdatedBy, opt => opt.Ignore()); ;
        }
    }
}