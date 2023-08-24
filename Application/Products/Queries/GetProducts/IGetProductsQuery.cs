using Domain;

namespace Application.Products.Queries.GetProducts
{
    public interface IGetProductsQuery
    {
        Task<List<Product>> ExecuteAsync();
    }
}
