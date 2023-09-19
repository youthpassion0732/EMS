namespace DTO
{
    public class PaginationRequestDto
    {
        public PaginationRequestDto()
        {
            PageNo = 1;
            PageSize = 10;
        }

        public string? Search { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int StartCount { get; set; }
        public int EndCount { get; set; }
    }
}
