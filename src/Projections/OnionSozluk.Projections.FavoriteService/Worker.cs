using OnionSozluk.Common;
using OnionSozluk.Common.Events.Entry;
using OnionSozluk.Common.Events.EntryComment;
using OnionSozluk.Common.Infrastructure;

namespace OnionSozluk.Projections.FavoriteService
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

            string conn = _configuration.GetConnectionString("SqlServer");

            var favService = new Services.FavoriteService(conn);

            //Create fav entry
            QueueFactory.CreateBasicConsumer()
                .EnsureExchange(SozlukConstants.FavExchangeName)
                .EnsureQueue(SozlukConstants.CreateEntryFavQueueName, SozlukConstants.FavExchangeName)
                .Receive<CreateEntryFavEvent>(fav =>
                {
                    //queye bir þey geldiði zaman ne yapacaksýn demektir bu.
                    //db insert
                    favService.CreateEntryFav(fav).GetAwaiter().GetResult();
                    _logger.LogInformation($"Received EntryId {fav.EntryId}");
                }).StartConsuming(SozlukConstants.CreateEntryFavQueueName);

            //Delete fav entry
            QueueFactory.CreateBasicConsumer()
                .EnsureExchange(SozlukConstants.FavExchangeName)
                .EnsureQueue(SozlukConstants.DeleteEntryFavQueueName, SozlukConstants.FavExchangeName)
                .Receive<DeleteEntryFavEvent>(fav =>
                {
                    //queye bir þey geldiði zaman ne yapacaksýn demektir bu.
                    //db insert
                    favService.DeleteEntryFav(fav).GetAwaiter().GetResult();
                    _logger.LogInformation($"Received EntryId {fav.EntryId}");
                }).StartConsuming(SozlukConstants.DeleteEntryFavQueueName);


            //Create fav entrycomment
            QueueFactory.CreateBasicConsumer()
                .EnsureExchange(SozlukConstants.FavExchangeName)
                .EnsureQueue(SozlukConstants.CreateEntryCommentFavQueueName, SozlukConstants.FavExchangeName)
                .Receive<CreateEntryCommentFavEvent>(fav =>
                {
                    //queye bir þey geldiði zaman ne yapacaksýn demektir bu.
                    //db insert
                    favService.CreateEntryCommentFav(fav).GetAwaiter().GetResult();
                    _logger.LogInformation($"Received EntryId {fav.EntryCommentId}");
                }).StartConsuming(SozlukConstants.CreateEntryCommentFavQueueName);


            //Delete fav entrycomment
            QueueFactory.CreateBasicConsumer()
                .EnsureExchange(SozlukConstants.FavExchangeName)
                .EnsureQueue(SozlukConstants.DeleteEntryCommentFavQueueName, SozlukConstants.FavExchangeName)
                .Receive<DeleteEntryCommentFavEvent>(fav =>
                {
                    //queye bir þey geldiði zaman ne yapacaksýn demektir bu.
                    //db insert
                    favService.DeleteEntryCommentFav(fav).GetAwaiter().GetResult();
                    _logger.LogInformation($"Received EntryId {fav.EntryCommentId}");
                }).StartConsuming(SozlukConstants.DeleteEntryCommentFavQueueName);
        }
    }
}