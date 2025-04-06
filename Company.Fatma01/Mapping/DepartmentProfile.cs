using AutoMapper;
using Company.DAL.Models;
using Company.PL.Dtos;

namespace Company.PL.Mapping
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile() {
            CreateMap<CreateDepartmentDto, Department>();
            CreateMap<Department,CreateDepartmentDto>();
                }

    }
}
