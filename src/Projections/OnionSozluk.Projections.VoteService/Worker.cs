using OnionSozluk.Common;
using OnionSozluk.Common.Events.Entry;
using OnionSozluk.Common.Events.EntryComment;
using OnionSozluk.Common.Infrastructure;

namespace OnionSozluk.Projections.VoteService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var conn = _configuration.GetConnectionString("SqlServer");

            var voteService = new Services.VoteService(conn);

            // CreateEntryVote
            QueueFactory.CreateBasicConsumer()
                        .EnsureExchange(SozlukConstants.VoteExchangeName, SozlukConstants.DefaultExchangeType)
                        .EnsureQueue(SozlukConstants.CreateEntryVoteQueueName, SozlukConstants.VoteExchangeName)
                        .Receive<CreateEntryVoteEvent>(vote =>
                        {
                            // tabloya kayýt at.
                            voteService.CreateEntryUpVote(vote).GetAwaiter().GetResult(); //bakýlacak!
                            _logger.LogInformation($"Received EntryId {vote.EntryId}");
                        }).StartConsuming(SozlukConstants.CreateEntryVoteQueueName);


            // DeleteEntryVote
            QueueFactory.CreateBasicConsumer()
                        .EnsureExchange(SozlukConstants.VoteExchangeName, SozlukConstants.DefaultExchangeType)
                        .EnsureQueue(SozlukConstants.DeleteEntryVoteQueueName, SozlukConstants.VoteExchangeName)
                        .Receive<DeleteEntryVoteEvent>(vote =>
                        {
                            voteService.DeleteEntryUpVote(vote).GetAwaiter().GetResult();
                        }).StartConsuming(SozlukConstants.DeleteEntryVoteQueueName);


            // CreateEntryCommentVote
            QueueFactory.CreateBasicConsumer()
                        .EnsureExchange(SozlukConstants.VoteExchangeName, SozlukConstants.DefaultExchangeType)
                        .EnsureQueue(SozlukConstants.CreateEntryCommentVoteQueueName, SozlukConstants.VoteExchangeName)
                        .Receive<CreateEntryCommentVoteEvent>(vote =>
                        {
                            // tabloya kayýt at.
                            voteService.CreateEntryCommentUpVote(vote).GetAwaiter().GetResult(); //bakýlacak!
                            _logger.LogInformation($"Received EntryId {vote.EntryCommentId}");
                        }).StartConsuming(SozlukConstants.CreateEntryCommentVoteQueueName);

            // DeleteEntryVote
            QueueFactory.CreateBasicConsumer()
                        .EnsureExchange(SozlukConstants.VoteExchangeName, SozlukConstants.DefaultExchangeType)
                        .EnsureQueue(SozlukConstants.DeleteEntryCommentVoteQueueName, SozlukConstants.VoteExchangeName)
                        .Receive<DeleteEntryCommentVoteEvent>(vote =>
                        {
                            voteService.DeleteEntryCommentUpVote(vote).GetAwaiter().GetResult();
                        }).StartConsuming(SozlukConstants.DeleteEntryCommentVoteQueueName);

        }
    }
}