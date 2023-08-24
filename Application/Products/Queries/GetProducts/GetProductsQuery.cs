using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Products.Queries.GetProducts
{
    public class GetProductsQuery : IGetProductsQuery
    {
        private readonly StoreContext _context;

        public GetProductsQuery(StoreContext context)
        {
            _context = context;
        }
        public async Task<List<Product>> ExecuteAsync()
        {
            var products = await _context.Products.ToListAsync();
            return products;
        }
    }
}
