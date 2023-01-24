using Dapper;
using Microsoft.Data.SqlClient;
using OnionSozluk.Common.Events.User;

namespace OnionSozluk.Projections.UserService.Services
{
    public class UserService
    {
        private readonly IConfiguration _configuration;
        private readonly string conn;

        public UserService(IConfiguration configuration)
        {
            _configuration = configuration;
            conn = _configuration.GetConnectionString("SqlServer");
        }


        public async Task<Guid> CreateEmailConfirmation(UserEmailChangedEvent userEmailChangedEvent)
        {
            var guid = Guid.NewGuid();

            using var connection = new SqlConnection(conn);

            await connection.ExecuteAsync("INSERT INTO Emailconfirmations (Id, OldEmailAddress, NewEmalAddress, CreateDate) " +
                                          "VALUES (@Id, @OldEmailAddress, @NewEmalAddress, GETDATE())",
                new
                {
                    Id = guid,
                    OldEmailAddress = userEmailChangedEvent.OldEmailAddress,
                    NewEmalAddress = userEmailChangedEvent.NewEmailAddress
                });

            return guid;
        }

    }
}
