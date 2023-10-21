using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Carts.Queries.GetCart
{
    public class GetCartQuery : IGetCartQuery
    {
        private readonly StoreContext _context;
        private readonly UserManager<AppUser> _userManager;

        public GetCartQuery(StoreContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<Cart> ExecuteAsync(string userName)
        {
            // 1. Get cart based on the logged-in user info
            var user = await _userManager.FindByNameAsync(userName);
            var cart = await _context.Carts
                .Include(cart => cart.CartItems)
                .ThenInclude(cartItem => cartItem.Product)
                .SingleOrDefaultAsync(cart => cart.AppUserId == user.Id);

            if (cart == null)
            {
                throw new Exception("A server error occurred. Please try again later.");
            }

            return cart;
        }
    }
}
