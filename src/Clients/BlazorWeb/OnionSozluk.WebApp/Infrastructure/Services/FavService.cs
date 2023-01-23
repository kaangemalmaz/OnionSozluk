using OnionSozluk.WebApp.Infrastructure.Services.Interfaces;

namespace OnionSozluk.WebApp.Infrastructure.Services
{
    public class FavService : IFavService
    {
        private readonly HttpClient client;

        public FavService(HttpClient client)
        {
            this.client = client;
        }

        public async Task CreateEntryFav(Guid entryId)
        {
            await client.PostAsync($"/api/favorite/entry/{entryId}", null);
        }

        public async Task CreateEntryCommentFav(Guid entryCommentId)
        {
            await client.PostAsync($"/api/favorite/entrycomment/{entryCommentId}", null);
        }

        public async Task DeleteEntryFav(Guid entryId)
        {
            await client.PostAsync($"/api/favorite/deleteentryfav/{entryId}", null);
        }

        public async Task DeleteEntryCommentFav(Guid entryCommentId)
        {
            await client.PostAsync($"/api/favorite/deleteentrycommentfav/{entryCommentId}", null);
        }
    }
}
