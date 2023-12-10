using Domain;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Carts.Commands.InitializeCart
{
    public class InitializeCartCommand : IInitializeCartCommand
    {
        private readonly StoreContext _context;
        public InitializeCartCommand(StoreContext context)
        {
            _context = context;
        }
        public async Task ExecuteAsync(AppUser user)
        {
            // User EFCore explicit loading to load related property of an existing entity object
            await _context.Entry(user).Navigation("Cart").LoadAsync();
            if (user.Cart == null)
            {
                var cart = new Cart
                {
                    CreatedAt = DateTime.Now,
                    LastUpdated = DateTime.Now,
                    CartTotal = 0,
                    AppUserId = user.Id,
                    AppUser = user
                };

                await _context.Carts.AddAsync(cart);
                await _context.SaveChangesAsync();
            }
        }
    }
}
