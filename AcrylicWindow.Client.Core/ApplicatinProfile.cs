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

            CreateMap<ClientEntity, Client.Core.Models.Client>();

            CreateMap<Client.Core.Models.Client, ClientEntity>()
                .ForMember(p => p.CreatedBy, opt => opt.Ignore())
                .ForMember(p => p.UpdatedBy, opt => opt.Ignore());
        }
    }
}