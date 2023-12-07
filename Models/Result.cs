namespace LibraryManagementAPI.Models
{
    public class Result<T>
    {
        public List<T> Data { get; set; }

        public int TotalCount { get; set; }
    }
}
