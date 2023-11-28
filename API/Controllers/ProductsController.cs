using Application.Products.Queries.GetProductDetail;
using Application.Products.Queries.GetProducts;
using Domain;
using Domain.Constants;
using Domain.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        
        private readonly IGetProductsQuery _getProductsQuery;
        private readonly IGetProductDetailQuery _getProductDetailQuery;
        public ProductsController(IGetProductsQuery getProductsQuery, IGetProductDetailQuery getProductDetailQuery)
        {
            
            _getProductsQuery = getProductsQuery;
            _getProductDetailQuery = getProductDetailQuery;
        }

        // if we do not specify a route parameter for the HttpGet, it will assume we are looking in the query string for mapping to the target .NET type parameters
        // TODO: add model validation for query string parameters using data annotations
        [HttpGet] // api/products
        [ResponseCache(CacheProfileName = "Any-60")]
        public async Task<ActionResult<ProductListDTO>> GetProducts(string? orderBy, string? searchKey,
            string? brands, string? types, string? colors, string? sex,
            int pageNumber = 1, int itemsPerPage = 9)
        {
            return await _getProductsQuery.ExecuteAsync(orderBy, searchKey, brands, types, colors, sex, pageNumber, itemsPerPage);
        }

        [HttpGet("{id}")] // api/products/3
        [ResponseCache(CacheProfileName = "Any-60")]
        public async Task<ActionResult<Product>> GetProductDetail(int id)
        {
            return await _getProductDetailQuery.ExecuteAsync(id);
        }

        [HttpGet]
        [Route("[action]")]
        [ResponseCache(CacheProfileName = "Any-60")]
        public ActionResult<List<IReadOnlyCollection<string>>> GetFilters()
        {
            return new List<IReadOnlyCollection<string>>()
            {
                ProductConstants.SizeOptions,
                ProductConstants.Brands,
                ProductConstants.Colors,
                ProductConstants.ProductTypes,
                ProductConstants.Sex
            };
        }
    }
}