using AutoMapper;
using Infomed.Domain.Dto;
using Infomed.Domain.Entities;

namespace Infomed.Domain.AutoMapper.Profiles;

public class AutocompleteMappingProfile : Profile
{
    public AutocompleteMappingProfile()
    {
        CreateMap<DoctorEntity, AutocompleteOptionDto>()
            .ForMember(destination => destination.Id, options => options.MapFrom(source => source.Id))
            .ForMember(destination => destination.Name, options => options.MapFrom(source => source.FullName));

        CreateMap<CityEntity, AutocompleteOptionDto>()
            .ForMember(destination => destination.Id, options => options.MapFrom(source => source.Id))
            .ForMember(destination => destination.Name, options => options.MapFrom(source => source.Name));
    }
}