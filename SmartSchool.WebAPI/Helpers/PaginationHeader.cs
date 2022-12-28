namespace SmartSchool.WebAPI.Helpers
{
    public class PaginationHeader
    {
        
        public int CurrentPage { get; set; }
        public int ItensPerPage { get; set; }

        public int TotalItems { get; set; }
        public int TotalPages { get; set; }

        public PaginationHeader(int currentPage, int itensPerPage, int totalItems, int totalPages)
        {
            this.CurrentPage = currentPage;
            this.ItensPerPage = itensPerPage;
            this.TotalItems = totalItems;
            this.TotalPages = totalPages;

        }


    }
}