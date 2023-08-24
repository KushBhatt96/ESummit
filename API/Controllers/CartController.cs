using Application.Carts.Queries.GetCart;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CartController : BaseApiController
    {
        private readonly IGetCartQuery _getCartQuery;

        public CartController(IGetCartQuery getCartQuery)
        {
            _getCartQuery = getCartQuery;
        }

        [HttpGet]
        public async Task<ActionResult<Cart>> GetCart()
        {
            return await _getCartQuery.ExecuteAsync();
        }
    }
}
