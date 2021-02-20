using AcrylicWindow.Client.Core.Model;
using AcrylicWindow.Client.Data.Entities;
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
        }
    }
}