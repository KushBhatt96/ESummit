namespace Application.CartItems.Commands.UpdateCartItemQuantity
{
    public interface IUpdateCartItemQuantityCommand
    {
        Task ExecuteAsync(string userName, int cartItemId, int quantity);
    }
}
