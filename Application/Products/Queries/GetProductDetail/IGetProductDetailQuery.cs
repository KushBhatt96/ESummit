using Domain;

namespace Application.Products.Queries.GetProductDetail
{
    public interface IGetProductDetailQuery
    {
        Task<Product> ExecuteAsync(int productId); 
    }
}
