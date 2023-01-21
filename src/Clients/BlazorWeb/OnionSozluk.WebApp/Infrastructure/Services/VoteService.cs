using OnionSozluk.Common.ViewModels;
using OnionSozluk.WebApp.Infrastructure.Services.Interfaces;
using System.Net.Http;

namespace OnionSozluk.WebApp.Infrastructure.Services
{
    public class VoteService : IVoteService
    {
        private readonly HttpClient _client;

        public VoteService(HttpClient client)
        {
            _client = client;
        }

        public async Task DeleteEntryVote(Guid entryId)
        {
            var response = await _client.PostAsync($"/api/Vote/DeleteEntryVote/{entryId}", null);
            if (!response.IsSuccessStatusCode) throw new Exception("DeleteEntryVote error");
        }
        public async Task DeleteEntryCommentVote(Guid entryCommentId)
        {
            var response = await _client.PostAsync($"/api/Vote/DeleteEntryCommentVote/{entryCommentId}", null);
            if (!response.IsSuccessStatusCode) throw new Exception("DeleteEntryVote error");
        }

        public async Task CreateEntryUpVote(Guid entryId)
        {
            await CreateEntryVote(entryId, VoteType.UpVote);
        }
        public async Task CreateEntryDownVote(Guid entryId)
        {
            await CreateEntryVote(entryId, VoteType.DownVote);
        }

        public async Task CreateEntryCommentUpVote(Guid entryCommentId)
        {
            await CreateEntryCommentVote(entryCommentId, VoteType.UpVote);
        }
        public async Task CreateEntryCommentDownVote(Guid entryCommentId)
        {
            await CreateEntryCommentVote(entryCommentId, VoteType.DownVote);
        }


        private async Task<HttpResponseMessage> CreateEntryVote(Guid entryId, VoteType voteType = VoteType.UpVote)
        {
            var response = await _client.PostAsync($"/api/Vote/Entry/{entryId}?voteType={voteType}", null);
            //check success
            return response;
            //if (!response.IsSuccessStatusCode) throw new Exception("CreateEntryType error");
        }

        private async Task<HttpResponseMessage> CreateEntryCommentVote(Guid entryCommentId, VoteType voteType = VoteType.UpVote)
        {
            var response = await _client.PostAsync($"/api/Vote/EntryComment/{entryCommentId}?voteType={voteType}", null);
            //check success
            return response;
            //if (!response.IsSuccessStatusCode) throw new Exception("CreateEntryType error");
        }

    }
}
