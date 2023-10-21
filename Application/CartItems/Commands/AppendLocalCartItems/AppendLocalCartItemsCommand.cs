using Domain;
using Domain.ExplicitJoinEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace Application.CartItems.Commands.AppendLocalCartItems
{
    public class AppendLocalCartItemsCommand : IAppendLocalCartItemsCommand
    {
        private readonly StoreContext _context;
        private readonly UserManager<AppUser> _userManager;

        public AppendLocalCartItemsCommand(StoreContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task ExecuteAsync(string userName, List<CartItem> localCartItems)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var cart = await _context.Carts.Include(cart => cart.CartItems).SingleOrDefaultAsync(cart => cart.AppUserId == user.Id);

            if (cart == null)
            {
                throw new Exception("A problem occurred while fetching cart. Please try again later.");
            }

            var dbCartItemsMap = new Dictionary<int, CartItem>();
            var newCartItems = new List<CartItem>();

            foreach (var item in cart.CartItems)
            {
                dbCartItemsMap.Add(item.ProductId, item);
            }

            foreach (var localItem in localCartItems)
            {
                if (dbCartItemsMap.ContainsKey(localItem.ProductId))
                {
                    var totalQuantity = dbCartItemsMap[localItem.ProductId].Quantity + localItem.Quantity;
                    if (!IsCartItemQuantityWithinRange(totalQuantity))
                    {
                        totalQuantity = 5;
                    }

                    dbCartItemsMap[localItem.ProductId].Quantity = totalQuantity;
                }
                else
                {
                    localItem.CartItemId = 0;
                    localItem.CartId = cart.CartId;
                    localItem.Product = null;
                    newCartItems.Add(localItem);
                }
            }

            await _context.CartItems.AddRangeAsync(newCartItems);
            var test = _context.ChangeTracker.DebugView;
            await _context.SaveChangesAsync();
        }

        private bool IsCartItemQuantityWithinRange(int quantity)
        {
            if (quantity < 1)
            {
                throw new Exception("The minimum quantity is 1.");
            }
            return quantity <= 5;
        }
    }
}
