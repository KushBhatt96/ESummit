using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.CartItems.Commands.RemoveAllCartItems
{
    public class RemoveAllCartItemsCommand : IRemoveAllCartItemsCommand
    {
        private readonly StoreContext _context;
        private readonly UserManager<AppUser> _userManager;
        public RemoveAllCartItemsCommand(StoreContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task ExecuteAsync(string userName)
        {
            // 1. Find the cart associated with this user
            var user = await _userManager.FindByNameAsync(userName);
            var cart = await _context.Carts.Include(cart => cart.CartItems).SingleOrDefaultAsync(cart => cart.AppUserId == user.Id);

            if (cart == null)
            {
                // TODO: Logging
                throw new Exception("A server error occurred. Please try again later.");
            }

            // 2. Remove all cart items from the Cart and save to DB
            foreach(var cartItem in cart.CartItems)
            {
                _context.CartItems.Remove(cartItem);
            }

            await _context.SaveChangesAsync();
        }
    }
}
