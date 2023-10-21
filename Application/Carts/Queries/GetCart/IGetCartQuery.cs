using Domain;

namespace Application.Carts.Queries.GetCart
{
    public interface IGetCartQuery
    {
        Task<Cart> ExecuteAsync(string userName);
    }
}
