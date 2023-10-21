using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.CartItems.Commands.UpdateCartItemQuantity
{
    public class UpdateCartItemQuantityCommand : IUpdateCartItemQuantityCommand
    {
        private readonly StoreContext _context;
        private readonly UserManager<AppUser> _userManager;
        public UpdateCartItemQuantityCommand(StoreContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task ExecuteAsync(string userName, int cartItemId, int quantity)
        {
            if (quantity < 1 || quantity > 5)
            {
                // TODO: Logging
                // TODO: Create RestException type and handle in global error handler
                throw new Exception("Please enter a valid quantity.");
            }

            // 1. Find the cart associated with this user, user cartId == 1 for now
            var user = await _userManager.FindByNameAsync(userName);
            var cart = await _context.Carts.Include(cart => cart.CartItems).SingleOrDefaultAsync(cart => cart.AppUserId == user.Id);

            if (cart == null)
            {
                // TODO: Logging
                throw new Exception("A server error occurred. Please try again later.");
            }

            // 2. Find the cartItem associated with the incoming cartItemId
            var cartItem = cart.CartItems.SingleOrDefault(cartItem => cartItem.CartItemId == cartItemId);
            if (cartItem == null)
            {
                // TODO: Logging
                // TODO: Create RestException type and handle in global error handler
                throw new Exception("A cart item does not exist for this product.");
            }

            cartItem.Quantity = quantity;
            await _context.SaveChangesAsync();
        }
    }
}
