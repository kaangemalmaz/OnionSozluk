using AutoMapper;
using OnionSozluk.Api.Domain.Models;
using OnionSozluk.Common.ViewModels.Queries;

namespace OnionSozluk.Api.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<LoginUserViewModel, User>().ReverseMap();
        }
    }
}
