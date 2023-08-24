using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.CartItems.Commands.RemoveCartItem
{
    public class RemoveCartItemCommand : IRemoveCartItemCommand
    {
        private readonly StoreContext _context;

        public RemoveCartItemCommand(StoreContext context)
        {
            _context = context;
        }

        public async Task ExecuteAsync(int cartItemId)
        {
            // 1. Find the cart associated with this user, user cartId == 1 for now
            var tempCartId = 1;
            var cart = await _context.Carts.Include(cart => cart.CartItems).SingleOrDefaultAsync(cart => cart.CartId == tempCartId);

            if (cart == null)
            {
                // TODO: Logging
                throw new Exception("A server error occurred. Please try again later.");
            }

            // 2. Find the cartItem associated with the incoming productId, if such a cartItem exists, remove it from the cart and product
            var cartItem = cart.CartItems.SingleOrDefault(cartItem => cartItem.CartItemId == cartItemId);
            if (cartItem == null)
            {
                // TODO: Logging
                // TODO: Create RestException type and handle in global error handler
                throw new Exception("This product is not in the shopping cart.");
            }

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

        }
    }
}
