using Domain.ExplicitJoinEntities;

namespace Application.CartItems.Commands.AppendLocalCartItems
{
    public interface IAppendLocalCartItemsCommand
    {
        Task ExecuteAsync(string userName, List<CartItem> localCartItems);
    }
}