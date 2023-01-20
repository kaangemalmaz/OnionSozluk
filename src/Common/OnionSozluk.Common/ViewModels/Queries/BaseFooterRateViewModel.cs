namespace OnionSozluk.Common.ViewModels.Queries
{
    public class BaseFooterRateViewModel
    {
        public VoteType VoteType { get; set; }

    }
    public class BaseFooterFavoritedViewModel
    {
        public bool IsFavorited { get; set; }
        public int FavoritedCount { get; set; }
    }
    public class BaseFooterRateFavoritedViewModel : BaseFooterFavoritedViewModel 
    {
        // çift implement yok o yüzden bu şekilde
        public VoteType VoteType { get; set; }

    }
}
