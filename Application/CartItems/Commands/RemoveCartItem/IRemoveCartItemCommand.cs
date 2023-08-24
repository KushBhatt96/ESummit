namespace Application.CartItems.Commands.RemoveCartItem
{
    public interface IRemoveCartItemCommand
    {
        Task ExecuteAsync(int cartItemId);
    }
}
