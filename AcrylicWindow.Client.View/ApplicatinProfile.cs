using AcrylicWindow.Client.Core.Models;
using AcrylicWindow.Client.Entity.Entities;
using AutoMapper;

namespace AcrylicWindow
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            CreateMap<EmployeeEntity, Employee>();

            CreateMap<Employee, EmployeeEntity>()
                .ForMember(ee => ee.Groups, opt => opt.Ignore())
                .ForMember(ee => ee.CreatedBy, opt => opt.Ignore())
                .ForMember(ee => ee.UpdatedBy, opt => opt.Ignore());

            CreateMap<StudentEntity, Student>();

            CreateMap<Student, StudentEntity>()
                .ForMember(se => se.Groups, opt => opt.Ignore())
                .ForMember(se => se.CreatedBy, opt => opt.Ignore())
                .ForMember(se => se.UpdatedBy, opt => opt.Ignore());

            CreateMap<GroupEntity, Group>()
                .ForMember(g => g.Teacher, opt => opt.Ignore())
                .ForMember(g => g.Students, opt => opt.Ignore());

            CreateMap<GroupCreate, GroupEntity>()
                .ForMember(ge => ge.CreatedBy, opt => opt.Ignore())
                .ForMember(ge => ge.UpdatedBy, opt => opt.Ignore())
                .ForMember(ge => ge.Begin, opt => opt.Ignore())
                .ForMember(ge => ge.End, opt => opt.Ignore())
                .ForMember(ge => ge.TeacherId, opt => opt.Ignore())
                .ForMember(ge => ge.StudentsIds, opt => opt.Ignore());

            CreateMap<GroupUpdate, GroupEntity>()
                .ForMember(ge => ge.CreatedBy, opt => opt.Ignore())
                .ForMember(ge => ge.UpdatedBy, opt => opt.Ignore());
        }
    }
}