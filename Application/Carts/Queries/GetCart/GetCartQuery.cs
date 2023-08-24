using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Carts.Queries.GetCart
{
    public class GetCartQuery : IGetCartQuery
    {
        private readonly StoreContext _context;

        public GetCartQuery(StoreContext context)
        {
            _context = context;
        }
        public async Task<Cart> ExecuteAsync()
        {
            // 1. Get cart based on the logged-in user info
            // Use tempCartId for now
            var tempCartId = 1;
            var cart = await _context.Carts
                .Include(cart => cart.CartItems)
                .ThenInclude(cartItem => cartItem.Product)
                .SingleOrDefaultAsync(cart => cart.CartId == tempCartId);

            if (cart == null)
            {
                throw new Exception("A server error occurred. Please try again later.");
            }

            return cart;
        }
    }
}
