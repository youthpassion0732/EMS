namespace DTO
{
    public class PaginationResponseDto<T>
    {
        public int TotalRecords { get; set; }
        public int PageIndex { get; set; }
        public int StartCount { get; set; }
        public int EndCount { get; set; }
        public int NoOfPages { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}
