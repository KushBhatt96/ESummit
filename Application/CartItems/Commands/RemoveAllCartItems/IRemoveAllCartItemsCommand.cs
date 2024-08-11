namespace Application.CartItems.Commands.RemoveAllCartItems
{
    public interface IRemoveAllCartItemsCommand
    {
        Task ExecuteAsync(string userName);
    }
}
