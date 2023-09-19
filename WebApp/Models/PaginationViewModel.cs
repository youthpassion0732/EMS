namespace WebApp.Models
{
    public class PaginationViewModel
    {
        public PaginationViewModel()
        {
            Pages = new List<PageUrl>();
        }

        public int CurrentPage { get; set; }
        public string NextPageUrl { get; set; }
        public string PreviousPageUrl { get; set; }
        public int TotalPages { get; set; }
        public List<PageUrl> Pages { get; set; }
    }

    public class PageUrl
    {
        public string LinkValue { get; set; }
        public string Url { get; set; }
    }

}