using AcrylicWindow.Client.Core.Models;
using AcrylicWindow.Client.Entity.Entities;
using AutoMapper;
using System.Linq;

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

            CreateMap<GroupEntity, Group>()
                .ForMember(p => p.Teacher, opt => opt.Ignore())
                .ForMember(p => p.Students, opt => opt.Ignore());

            CreateMap<Group, GroupEntity>()
                .ForMember(p => p.CreatedBy, opt => opt.Ignore())
                .ForMember(p => p.UpdatedBy, opt => opt.Ignore())
                /// ToDo : Think about creating a separate DTO
                .ForMember(p => p.TeacherId, opt => opt.MapFrom(g => g.Id))
                .ForMember(p => p.StudentsIds, opt => opt.MapFrom(g => g.Students.Select(s => s.Id)));
        }
    }
}