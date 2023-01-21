using OnionSozluk.Common.ViewModels.Page;
using OnionSozluk.Common.ViewModels.Queries;
using OnionSozluk.Common.ViewModels.RequestModels;

namespace OnionSozluk.WebApp.Infrastructure.Services.Interfaces
{
    public interface IEntryService
    {
        Task<Guid> CreateEntry(CreateEntryCommand command);
        Task<Guid> CreateEntryComment(CreateEntryCommentCommand command);
        Task<List<GetEntriesViewModel>> GetEntries();
        Task<PagedViewModel<GetEntryCommentsViewModel>> GetEntryComments(Guid entryId, int page, int pageSize);
        Task<GetEntryDetailViewModel> GetEntryDetail(Guid entryId);
        Task<PagedViewModel<GetEntryDetailViewModel>> GetMainPageEntries(int page, int pageSize);
        Task<PagedViewModel<GetEntryDetailViewModel>> GetProfilePageEntries(int page, int pageSize, string userName = null);
        Task<List<SearchEntryViewModel>> SearchBySubject(string searchText);
    }
}