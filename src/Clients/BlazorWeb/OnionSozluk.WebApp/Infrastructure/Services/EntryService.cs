using OnionSozluk.Common.ViewModels.Page;
using OnionSozluk.Common.ViewModels.Queries;
using OnionSozluk.Common.ViewModels.RequestModels;
using OnionSozluk.WebApp.Infrastructure.Services.Interfaces;
using System.Net.Http.Json;

namespace OnionSozluk.WebApp.Infrastructure.Services
{
    public class EntryService : IEntryService
    {
        private readonly HttpClient client;

        public EntryService(HttpClient client)
        {
            this.client = client;
        }

        public async Task<List<GetEntriesViewModel>> GetEntries()
        {
            var result = await client.GetFromJsonAsync<List<GetEntriesViewModel>>("/api/Entry/GetEntries?todayEntries=false&count=30");
            return result;
        }

        public async Task<GetEntryDetailViewModel> GetEntryDetail(Guid entryId)
        {
            var result = await client.GetFromJsonAsync<GetEntryDetailViewModel>($"/api/Entry/{entryId}");
            return result;
        }

        public async Task<PagedViewModel<GetEntryDetailViewModel>> GetMainPageEntries(int page, int pageSize)
        {
            var result = await client.GetFromJsonAsync<PagedViewModel<GetEntryDetailViewModel>>($"/api/Entry/GetMainPageEntries?page={page}&pageSize={pageSize}");
            return result;
        }

        public async Task<PagedViewModel<GetEntryDetailViewModel>> GetProfilePageEntries(int page, int pageSize, string userName = null)
        {
            var result = await client.GetFromJsonAsync<PagedViewModel<GetEntryDetailViewModel>>($"/api/Entry/UserEntries?username={userName}&page={page}&pageSize={pageSize}");
            return result;
        }

        public async Task<PagedViewModel<GetEntryCommentsViewModel>> GetEntryComments(Guid entryId, int page, int pageSize)
        {
            var result = await client.GetFromJsonAsync<PagedViewModel<GetEntryCommentsViewModel>>($"/api/Entry/Comments/{entryId}?page={page}&pageSize={pageSize}");
            return result;
        }

        public async Task<Guid> CreateEntry(CreateEntryCommand command)
        {
            var result = await client.PostAsJsonAsync("/api/Entry/CreateEntry", command);

            if (!result.IsSuccessStatusCode)
                return Guid.Empty;

            var guidStr = await result.Content.ReadAsStringAsync();

            return new Guid(guidStr.Trim('"'));
        }

        public async Task<Guid> CreateEntryComment(CreateEntryCommentCommand command)
        {
            var result = await client.PostAsJsonAsync("/api/Entry/CreateEntryComment", command);

            if (!result.IsSuccessStatusCode)
                return Guid.Empty; //geriye guid dönüyor eğer statüs kod hatalı gelirse boş guid olarak dön.

            var guidStr = await result.Content.ReadAsStringAsync(); 

            return new Guid(guidStr.Trim('"')); //guid dönerken içinde " lar ile dönüyor onu trimlemek için.
        }

        public async Task<List<SearchEntryViewModel>> SearchBySubject(string searchText)
        {
            var result = await client.GetFromJsonAsync<List<SearchEntryViewModel>>($"/api/Entry/Search?searchText={searchText}");
            return result;
        }
    }
}
