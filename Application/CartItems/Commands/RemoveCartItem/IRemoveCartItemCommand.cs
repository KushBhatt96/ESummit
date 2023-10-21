namespace Application.CartItems.Commands.RemoveCartItem
{
    public interface IRemoveCartItemCommand
    {
        Task ExecuteAsync(string userName, int cartItemId);
    }
}
