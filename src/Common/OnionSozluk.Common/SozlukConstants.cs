namespace OnionSozluk.Common
{
    public class SozlukConstants
    {
        public const string RabbitMQHost = "localhost";
        public const string DefaultExchangeType = "direct";

        // USER
        public const string UserExchangeName = "UserExchange";
        public const string UserEmailChangedQueueName = "UserEmailChangedQueue";

        // FAV
        public const string FavExchangeName = "FavExchange";
        public const string CreateEntryFavQueueName = "CreateEntryFavQueue";
        public const string CreateEntryCommentFavQueueName = "CreateEntryCommentFavQueue";
        public const string DeleteEntryFavQueueName = "DeleteEntryFavQueue";
        public const string DeleteEntryCommentFavQueueName = "DeleteEntryCommentFavQueue";
        
        // VOTE
        public const string VoteExchangeName = "VoteExchange";
        public const string CreateEntryVoteQueueName = "CreateEntryVoteQueue";
        public const string DeleteEntryVoteQueueName = "DeleteEntryVoteQueue";
        public const string CreateEntryCommentVoteQueueName = "CreateEntryCommentVoteQueue";
        public const string DeleteEntryCommentVoteQueueName = "DeleteEntryCommentVoteQueue";
    }
}
