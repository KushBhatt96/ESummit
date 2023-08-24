using Application.CartItems.Commands.AddCartItem;
using Application.CartItems.Commands.RemoveCartItem;
using Application.CartItems.Commands.UpdateCartItemQuantity;
using Domain.DTO;
using Domain.ExplicitJoinEntities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CartItemsController : BaseApiController
    {
        private readonly IAddCartItemCommand _addCartItemCommand;
        private readonly IRemoveCartItemCommand _removeCartItemCommand;
        private readonly IUpdateCartItemQuantityCommand _updateCartItemQuantityCommand;
        public CartItemsController(IAddCartItemCommand addCartItemCommand,
                                   IRemoveCartItemCommand removeCartItemCommand,
                                   IUpdateCartItemQuantityCommand updateCartItemQuantityCommand)
        {
            _addCartItemCommand = addCartItemCommand;
            _removeCartItemCommand = removeCartItemCommand;
            _updateCartItemQuantityCommand = updateCartItemQuantityCommand;
        }

        [HttpPost("{productId}")]
        public async Task<ActionResult<CartItem>> AddCartItem(int productId)
        {
            return await _addCartItemCommand.ExecuteAsync(productId);
        }

        [HttpDelete("{cartItemId}")]
        public async Task<ActionResult> RemoveCartItem(int cartItemId)
        {
            await _removeCartItemCommand.ExecuteAsync(cartItemId);
            return Ok();
        }

        [HttpPut("{cartItemId}")]
        public async Task<ActionResult> UpdateCartItemQuantity(int cartItemId, [FromBody] QuantityDTO quantityDTO)
        {
            var quantity = quantityDTO.Quantity;
            await _updateCartItemQuantityCommand.ExecuteAsync(cartItemId, quantity);
            return Ok();
        }
    }
}
