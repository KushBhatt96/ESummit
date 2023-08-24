using Domain;
using Persistence;

namespace Application.Products.Queries.GetProductDetail
{
    public class GetProductDetailQuery : IGetProductDetailQuery
    {
        private readonly StoreContext _context;

        public GetProductDetailQuery(StoreContext context)
        {
            _context = context;
        }
        public async Task<Product> ExecuteAsync(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                // TODO: Logging
                // TODO: Create appropriate RestException class and pass not found status code
                throw new Exception("Product was not found.");
            }
            return product;
        }
    }
}
