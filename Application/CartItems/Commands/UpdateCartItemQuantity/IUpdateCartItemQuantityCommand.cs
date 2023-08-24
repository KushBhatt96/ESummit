namespace Application.CartItems.Commands.UpdateCartItemQuantity
{
    public interface IUpdateCartItemQuantityCommand
    {
        Task ExecuteAsync(int cartItemId, int quantity);
    }
}
