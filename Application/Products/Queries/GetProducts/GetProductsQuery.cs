using Domain;
using Domain.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Persistence;

namespace Application.Products.Queries.GetProducts
{
    public class GetProductsQuery : IGetProductsQuery
    {
        private readonly StoreContext _context;
        //private readonly IMemoryCache _memoryCache;
        private readonly IDistributedCache _distributedCache;

        public GetProductsQuery(StoreContext context, IDistributedCache distributedCache)
        {
            _context = context;
            _distributedCache = distributedCache;
        }
        public async Task<ProductListDTO?> ExecuteAsync(string? orderBy, string? searchKey, 
            string? brands, string? types, string? colors, string? sex,
            int pageNumber, int itemsPerPage)
        {
            // Lets implement in-memory caching
            ProductListDTO? productListInfo;
            var cacheKey = $"{orderBy}{searchKey}{brands}{types}{colors}{sex}{pageNumber}{itemsPerPage}";

            // If cacheKey not found in the _memoryCache, then do DB operations below and then set result in _memoryCache
            if (!_distributedCache.TryGetValue(cacheKey, out productListInfo))
            {
                var query = _context.Products.AsQueryable();
                // 1. Handle sorting
                switch (orderBy)
                {
                    case "price":
                        query = query.OrderBy(product => product.Price);
                        break;
                    case "priceDesc":
                        query = query.OrderByDescending(product => product.Price);
                        break;
                    default:
                        query = query.OrderBy(product => product.Name);
                        break;
                }

                // 2. Handle search operation
                if (!string.IsNullOrEmpty(searchKey))
                {
                    var cleansedSearchKey = searchKey.Trim().ToLower();
                    query = query.Where(product => product.Name.ToLower().Contains(cleansedSearchKey));
                }

                // 3. Handle filtering operation
                var brandList = new List<string>();
                var typeList = new List<string>();
                var colorList = new List<string>();
                var sexSelected = new List<string>();

                if (!string.IsNullOrEmpty(brands))
                {
                    brandList.AddRange(brands.ToLower().Split(",").ToList());
                }
                if (!string.IsNullOrEmpty(types))
                {
                    typeList.AddRange(types.ToLower().Split(",").ToList());
                }
                if (!string.IsNullOrEmpty(colors))
                {
                    colorList.AddRange(colors.ToLower().Split(",").ToList());
                }
                if (!string.IsNullOrEmpty(sex))
                {
                    sexSelected.AddRange(sex.ToLower().Split(",").ToList());
                }

                query = query.Where(product => brandList.Count == 0 || brandList.Contains(product.Brand.ToLower()));
                query = query.Where(product => typeList.Count == 0 || typeList.Contains(product.Type.ToLower()));
                query = query.Where(product => colorList.Count == 0 || colorList.Contains(product.Color.ToLower()));
                query = query.Where(product => sexSelected.Count == 0 || sexSelected.Contains(product.Sex.ToLower()));

                // 4. Total number of matching products that we must paginate
                var totalMatchingProducts = query.Count();

                // 5. Finally, handle pagination once we know what we're sending back
                query = query.Skip((pageNumber - 1) * itemsPerPage).Take(itemsPerPage);

                var productList = await query.ToListAsync();
                productListInfo = new ProductListDTO
                {
                    ProductList = productList,
                    PageNumber = pageNumber,
                    ItemsPerPage = itemsPerPage,
                    TotalMatchingProducts = totalMatchingProducts
                };
                _distributedCache.Set(cacheKey, productListInfo, new TimeSpan(0, 0, 30));
            }
            return productListInfo;
        }
    }
}
