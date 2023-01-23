using Dapper;
using Microsoft.Data.SqlClient;
using OnionSozluk.Common.Events.Entry;
using OnionSozluk.Common.Events.EntryComment;

namespace OnionSozluk.Projections.FavoriteService.Services
{
    public class FavoriteService
    {
        private readonly string connectionString;

        public FavoriteService(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task CreateEntryFav(CreateEntryFavEvent @event)
        {
            using var connection = new SqlConnection(connectionString);



            //düze sql mantığı için;
            /*
             * using var connection = new SqlConnection(connectionString); sql connection açılır.
             * connection.CreateCommand().ExecuteNonQueryAsync() şeklinde çalıştırılır.
             */

            // dapper
            // parametre olduğunu göstermek için başında @ olması gerekir.
            await connection
                .ExecuteAsync("INSERT INTO EntryFavorites (Id, EntryId, CreatedById, CreateDate) VALUES(@Id, @EntryId, @CreatedById, GETDATE())",
                new
                {
                    // burada parametre ismi ile aynı olmak zorunda!
                    Id = Guid.NewGuid(),
                    EntryId = @event.EntryId,
                    CreatedById = @event.CreatedBy
                });
        }

        public async Task DeleteEntryFav(DeleteEntryFavEvent @event)
        {
            using var connection = new SqlConnection(connectionString);

            await connection.ExecuteAsync("DELETE FROM EntryFavorites WHERE EntryId = @EntryId AND CreatedById = @CreatedById",
                new
                {
                    Id = Guid.NewGuid(),
                    EntryId = @event.EntryId,
                    CreatedById = @event.CreatedBy
                });
        }

        public async Task CreateEntryCommentFav(CreateEntryCommentFavEvent @event)
        {
            using var connection = new SqlConnection(connectionString);

            await connection.ExecuteAsync("INSERT INTO EntryCommentFavorites (Id, EntryCommentId, CreatedById, CreateDate) VALUES(@Id, @EntryCommentId, @CreatedById, GETDATE())",
                new
                {
                    Id = Guid.NewGuid(),
                    EntryCommentId = @event.EntryCommentId,
                    CreatedById = @event.CreatedBy
                });
        }

        public async Task DeleteEntryCommentFav(DeleteEntryCommentFavEvent @event)
        {
            using var connection = new SqlConnection(connectionString);

            await connection.ExecuteAsync("DELETE FROM EntryCommentFavorites WHERE EntryCommentId = @EntryCommentId AND CreatedById = @CreatedById",
                new
                {
                    Id = Guid.NewGuid(),
                    EntryCommentId = @event.EntryCommentId,
                    CreatedById = @event.CreatedBy
                });
        }
    }
}
