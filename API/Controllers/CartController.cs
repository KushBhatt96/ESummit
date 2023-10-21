using Application.Carts.Queries.GetCart;
using Common.Contants;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Authorize (Roles = RoleNames.Customer)]
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
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            if (userName == null)
            {
                throw new Exception("A problem occurred. Please try again later.");
            }


            return await _getCartQuery.ExecuteAsync(userName);
        }
    }
}
