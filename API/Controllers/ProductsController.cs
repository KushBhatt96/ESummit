using Application.Products.Queries.GetProductDetail;
using Application.Products.Queries.GetProducts;
using Common.Contants;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(Roles = RoleNames.Admin)]
    public class ProductsController : BaseApiController
    {
        private readonly IGetProductsQuery _getProductsQuery;
        private readonly IGetProductDetailQuery _getProductDetailQuery;
        public ProductsController(IGetProductsQuery getProductsQuery, IGetProductDetailQuery getProductDetailQuery)
        {
            _getProductsQuery = getProductsQuery;
            _getProductDetailQuery = getProductDetailQuery;
        }

        [HttpGet] // api/products
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            return await _getProductsQuery.ExecuteAsync();
        }

        [HttpGet("{id}")] // api/products/3
        public async Task<ActionResult<Product>> GetProductDetail(int id)
        {
            return await _getProductDetailQuery.ExecuteAsync(id);
        }
    }
}