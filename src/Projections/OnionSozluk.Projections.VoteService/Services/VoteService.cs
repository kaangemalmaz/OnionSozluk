using Dapper;
using Microsoft.Data.SqlClient;
using OnionSozluk.Common.Events.Entry;
using OnionSozluk.Common.Events.EntryComment;

namespace OnionSozluk.Projections.VoteService.Services
{
    public class VoteService
    {
        private string connectionString;

        public VoteService(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task CreateEntryUpVote(CreateEntryVoteEvent createEntryVoteEvent)
        {
            using var connection = new SqlConnection(connectionString);

            await
                connection.ExecuteAsync("INSERT INTO EntryVotes (Id, EntryId, VoteType, CreateById, CreateDate) " +
                                                        "VALUES (@Id, @EntryId, @VoteType, @CreateById, GETDATE())",
                new
                {
                    Id = Guid.NewGuid(),
                    EntryId = createEntryVoteEvent.EntryId,
                    VoteType = createEntryVoteEvent.VoteType,
                    CreateById = createEntryVoteEvent.CreatedBy
                });
        }

        public async Task DeleteEntryUpVote(DeleteEntryVoteEvent deleteEntryVoteEvent)
        {
            using var connection = new SqlConnection(connectionString);

            await
                connection.ExecuteAsync("DELETE FROM EntryVotes WHERE EntryId = @EntryId and CreateById = @CreateById",
                new
                {
                    EntryId = deleteEntryVoteEvent.EntryId,
                    CreateById = deleteEntryVoteEvent.CreatedBy
                });
        }

        public async Task CreateEntryCommentUpVote(CreateEntryCommentVoteEvent createEntryCommentVoteEvent)
        {
            using var connection = new SqlConnection(connectionString);

            await
                connection.ExecuteAsync("INSERT INTO EntryCommentVotes (Id, EntryCommentId, VoteType, CreateById, CreateDate) " +
                                                        "VALUES (@Id, @EntryCommentId, @VoteType, @CreateById, GETDATE())",
                new
                {
                    Id = Guid.NewGuid(),
                    EntryCommentId = createEntryCommentVoteEvent.EntryCommentId,
                    VoteType = createEntryCommentVoteEvent.VoteType,
                    CreateById = createEntryCommentVoteEvent.CreatedBy
                });
        }

        public async Task DeleteEntryCommentUpVote(DeleteEntryCommentVoteEvent deleteEntryCommentVoteEvent)
        {
            using var connection = new SqlConnection(connectionString);

            await
                connection.ExecuteAsync("DELETE FROM EntryCommentVotes WHERE EntryCommentId = @EntryCommentId and CreatedBy = @CreatedBy",
                new
                {
                    EntryCommentId = deleteEntryCommentVoteEvent.EntryCommentId,
                    CreatedBy = deleteEntryCommentVoteEvent.CreatedBy
                });
        }

    }
}
