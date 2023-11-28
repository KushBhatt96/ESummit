namespace Domain.DTO
{
    public class ProductListDTO
    {
        public List<Product> ProductList { get; set; }
        
        public int PageNumber { get; set; }

        public int ItemsPerPage { get; set; }
        public int TotalMatchingProducts { get; set; }

        public int NumberOfPages
        {
            get
            {
                var numberOfPages = (TotalMatchingProducts / ItemsPerPage) + (TotalMatchingProducts % ItemsPerPage == 0 ? 0 : 1);
                return numberOfPages;
            }
        }
    }
}
