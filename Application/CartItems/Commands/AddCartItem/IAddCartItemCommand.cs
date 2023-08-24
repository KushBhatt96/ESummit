using Domain.ExplicitJoinEntities;

namespace Application.CartItems.Commands.AddCartItem
{
    public interface IAddCartItemCommand
    {
        Task<CartItem> ExecuteAsync(int productId);
    }
}
