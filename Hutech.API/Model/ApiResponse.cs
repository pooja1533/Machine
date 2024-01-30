namespace Imputabiliteafro.Api.Model
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }
        public int CurrentPage { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPage { get; set; }
    }
}
