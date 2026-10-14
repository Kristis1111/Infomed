using AutoMapper;
using Infomed.Domain.Dto;
using Infomed.Domain.Entities;

namespace Infomed.Domain.AutoMapper.Profiles;

public class ContactRequestMappingProfile : Profile
{
    public ContactRequestMappingProfile()
    {
        CreateMap<ContactRequestDto, ContactRequestEntity>()
            .ForMember(destination => destination.Id, options => options.MapFrom(_ => Guid.NewGuid()))
            .ForMember(destination => destination.DoctorId,
                options => options.MapFrom((_, _, _, context) => (int)context.Items["DoctorId"]))
            .ForMember(destination => destination.FullName, options => options.MapFrom(source => source.FullName.Trim()))
            .ForMember(destination => destination.Phone, options => options.MapFrom(source => source.Phone.Trim()))
            .ForMember(destination => destination.Email, options => options.MapFrom(source => source.Email.Trim()))
            .ForMember(destination => destination.CreatedAt, options => options.MapFrom(_ => DateTimeOffset.UtcNow));
    }
}