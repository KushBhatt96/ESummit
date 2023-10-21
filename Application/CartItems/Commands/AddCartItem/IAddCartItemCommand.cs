using Domain.ExplicitJoinEntities;

namespace Application.CartItems.Commands.AddCartItem
{
    public interface IAddCartItemCommand
    {
        Task<CartItem> ExecuteAsync(string userName, int productId);
    }
}
