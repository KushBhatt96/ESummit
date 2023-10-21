using Domain;
using Domain.ExplicitJoinEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.CartItems.Commands.AddCartItem
{
    public class AddCartItemCommand : IAddCartItemCommand
    {

        private readonly StoreContext _context;
        private readonly UserManager<AppUser> _userManager;
        public AddCartItemCommand(StoreContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<CartItem> ExecuteAsync(string userName, int productId)
        {
            // 1. Make sure the product exists in the DB
            var product = await _context.Products.FindAsync(productId);

            if (product == null)
            {
                // TODO: Logging
                // TODO: Create RestException type and handle in global error handler
                throw new Exception("Unable to add. Please select a valid product.");
            }

            // 2. Based on the user information, we must retrieve their cart
            var user = await _userManager.FindByNameAsync(userName);

            var cart = await _context.Carts.Include(cart => cart.CartItems).SingleOrDefaultAsync(cart => cart.AppUserId == user.Id);

            if (cart == null)
            {
                // TODO: Logging
                throw new Exception("A server error occurred. Please try again later.");
            }

            // 3. Need to check if the cartItem containing the requested product is already in the cart
            var cartItem = cart.CartItems.SingleOrDefault(cartItem => cartItem.ProductId == productId);

            if (cartItem == null)
            {
                cartItem = new CartItem { ProductId = productId, CartId = cart.CartId, Quantity = 1 };
                await _context.CartItems.AddAsync(cartItem);
            }
            else
            {
                if (cartItem.Quantity < 5)
                {
                    cartItem.Quantity++;
                }
                else
                {
                    // TODO: Logging
                    // TODO: Create RestException type and handle in global error handler
                    throw new Exception("Item quantity limit has been reached.");
                }
            }

            await _context.SaveChangesAsync();
            return cartItem;
        }
    }
}
