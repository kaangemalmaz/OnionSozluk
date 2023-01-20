namespace OnionSozluk.Common.ViewModels.Page
{
    public class PagedViewModel<T> where T : class
    {
        public IList<T> Results { get; set; }
        public Page pageInfo { get; set; }
        public PagedViewModel() : this(new List<T>(), new Page())
        {

        }

        public PagedViewModel(IList<T> results, Page pageInfo)
        {
            Results = results;
            this.pageInfo = pageInfo;
        }
    }
}
