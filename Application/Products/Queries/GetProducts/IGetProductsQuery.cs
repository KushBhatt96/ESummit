using Domain;
using Domain.DTO;

namespace Application.Products.Queries.GetProducts
{
    public interface IGetProductsQuery
    {
        Task<ProductListDTO?> ExecuteAsync(string? orderBy, string? searchKey, 
            string? brands, string? types, string? colors, string? sex,
            int pageNumber, int itemsPerPage);
    }
}
