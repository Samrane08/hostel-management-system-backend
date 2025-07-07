namespace Model
{
    public class TableResponseModel<T>
    {       
        public List<T>? List { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
    }

    public class PaginationDataModel
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
    }
    public class FilterParamModel
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
