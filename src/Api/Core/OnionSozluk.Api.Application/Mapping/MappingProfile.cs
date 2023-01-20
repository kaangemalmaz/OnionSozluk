using AutoMapper;
using OnionSozluk.Api.Domain.Models;
using OnionSozluk.Common.ViewModels.Queries;
using OnionSozluk.Common.ViewModels.RequestModels;

namespace OnionSozluk.Api.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<LoginUserViewModel, User>().ReverseMap();
            CreateMap<CreateUserCommand, User>();
            CreateMap<UpdateUserCommand, User>();

            CreateMap<CreateEntryCommand, Entry>().ReverseMap();
            CreateMap<CreateEntryCommentCommand, EntryComment>().ReverseMap();
            CreateMap<Entry, GetEntriesViewModel>()
                .ForMember(x => x.CommentCount, y => y.MapFrom(z => z.EntryComments.Count)); // commentcount alanını suna göre maple demektir.

        }
    }
}
