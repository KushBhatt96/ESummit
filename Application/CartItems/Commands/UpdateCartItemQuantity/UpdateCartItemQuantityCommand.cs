using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.CartItems.Commands.UpdateCartItemQuantity
{
    public class UpdateCartItemQuantityCommand : IUpdateCartItemQuantityCommand
    {
        private readonly StoreContext _context;
        public UpdateCartItemQuantityCommand(StoreContext context)
        {
            _context = context;
        }
        public async Task ExecuteAsync(int cartItemId, int quantity)
        {
            if (quantity < 1 || quantity > 5)
            {
                // TODO: Logging
                // TODO: Create RestException type and handle in global error handler
                throw new Exception("Please enter a valid quantity.");
            }

            // 1. Find the cart associated with this user, user cartId == 1 for now
            var tempCartId = 1;
            var cart = await _context.Carts.Include(cart => cart.CartItems).SingleOrDefaultAsync(cart => cart.CartId == tempCartId);

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
