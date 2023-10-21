using Application.CartItems.Commands.AddCartItem;
using Application.CartItems.Commands.AppendLocalCartItems;
using Application.CartItems.Commands.RemoveCartItem;
using Application.CartItems.Commands.UpdateCartItemQuantity;
using Common.Contants;
using Domain.DTO;
using Domain.ExplicitJoinEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Authorize(Roles = RoleNames.Customer)]
    public class CartItemsController : BaseApiController
    {
        private readonly IAddCartItemCommand _addCartItemCommand;
        private readonly IRemoveCartItemCommand _removeCartItemCommand;
        private readonly IUpdateCartItemQuantityCommand _updateCartItemQuantityCommand;
        private readonly IAppendLocalCartItemsCommand _appendLocalCartItemsCommand;
        public CartItemsController(IAddCartItemCommand addCartItemCommand,
                                   IRemoveCartItemCommand removeCartItemCommand,
                                   IUpdateCartItemQuantityCommand updateCartItemQuantityCommand,
                                   IAppendLocalCartItemsCommand appendLocalCartItemsCommand)
        {
            _addCartItemCommand = addCartItemCommand;
            _removeCartItemCommand = removeCartItemCommand;
            _updateCartItemQuantityCommand = updateCartItemQuantityCommand;
            _appendLocalCartItemsCommand = appendLocalCartItemsCommand;
        }

        [HttpPost("{productId}")]
        public async Task<ActionResult<CartItem>> AddCartItem(int productId)
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            if (userName == null)
            {
                throw new Exception("A problem occurred. Please try again later.");
            }

            return await _addCartItemCommand.ExecuteAsync(userName, productId);
        }

        [HttpDelete("{cartItemId}")]
        public async Task<ActionResult> RemoveCartItem(int cartItemId)
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            if (userName == null)
            {
                throw new Exception("A problem occurred. Please try again later.");
            }

            await _removeCartItemCommand.ExecuteAsync(userName, cartItemId);
            return Ok();
        }

        [HttpPut("{cartItemId}")]
        public async Task<ActionResult> UpdateCartItemQuantity(int cartItemId, [FromBody] QuantityDTO quantityDTO)
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            if (userName == null)
            {
                throw new Exception("A problem occurred. Please try again later.");
            }

            var quantity = quantityDTO.Quantity;
            await _updateCartItemQuantityCommand.ExecuteAsync(userName, cartItemId, quantity);
            return Ok();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> AppendLocalCartItems([FromBody] List<CartItem> localCartItems)
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            if (userName == null)
            {
                throw new Exception("A problem occurred. Please try again later.");
            }

            await _appendLocalCartItemsCommand.ExecuteAsync(userName, localCartItems);
            return Ok();
        }
    }
}
