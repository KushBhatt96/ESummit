using Common.Contants;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Persistence
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(StoreContext context, UserManager<AppUser> userManager)
        {
            var hasUsers = await userManager.Users.AnyAsync();
            var hasProducts = await context.Products.AnyAsync();

            if (hasUsers && hasProducts) return;

            if (!hasUsers)
            {
                await AddAdministratorAsync(userManager);
            }

            if (!hasProducts)
            {
                await AddProductsAsync(context);
            }

            await context.SaveChangesAsync();
        }

        public static async Task AddAdministratorAsync(UserManager<AppUser> userManager)
        {
            var results = new List<IdentityResult>();
            // 1. Create a single admin user for test purposes
            var admin = new AppUser
            {
                FirstName = "test",
                LastName = "admin",
                DateOfBirth = DateTime.ParseExact("2023-10-01", "yyyy-MM-dd", new CultureInfo("en-US", true)),
                UserName = "test.admin",
                Email = "test.admin@esummit.com",
            };

            results.Add(await userManager.CreateAsync(admin, "ESummit1234$"));

            // 2. Assign appropriate roles to newly created admin user
            results.Add(await userManager.AddToRolesAsync(admin, new string[] { RoleNames.Admin, RoleNames.Customer }));

            results.ForEach(result =>
            {
                if (!result.Succeeded)
                {
                    throw new Exception("Error occurred while attempting to seed data.");
                }
            });
        }

        public static async Task AddProductsAsync(DbContext context)
        {
            var products = new List<Product>
            {
                new Product
                {
                    Name = "Mens React Grey Top",
                    Description =
                        "Stay comfortable and stylish with our Mens React Grey Top. Made from high-quality materials, " +
                        "this top is perfect for casual outings or workouts.",
                    Price = 8000,
                    PictureUrl = "/images/products/mens_react_grey_top.png",
                    TransitionPictureUrl = "/images/products/mens_react_grey_top_model.png",
                    Brand = "React",
                    Type = "Tops",
                    Color = "grey",
                    Sex = "Men"
                },
                new Product
                {
                    Name = "Mens Redux Lightblue Top",
                    Description =
                        "Elevate your wardrobe with our Mens Redux Lightblue Top. Its vibrant color and modern design make" +
                        " it a versatile choice for various occasions.",
                    Price = 9000,
                    PictureUrl = "/images/products/mens_redux_lightblue_top.png",
                    TransitionPictureUrl = "/images/products/mens_redux_lightblue_top_model.png",
                    Brand = "Redux",
                    Type = "Tops",
                    Color = "lightblue",
                    Sex = "Men"
                },
                new Product
                {
                    Name = "Mens Docker Pink Top",
                    Description =
                        "Make a bold statement with our Mens Docker Pink Top. This eye-catching top offers both fashion and comfort, " +
                        "making it a great addition to your collection.",
                    Price = 9000,
                    PictureUrl = "/images/products/mens_docker_pink_top.png",
                    TransitionPictureUrl = "/images/products/mens_docker_pink_top_model.png",
                    Brand = "Docker",
                    Type = "Tops",
                    Color = "pink",
                    Sex = "Men"
                },
                new Product
                {
                    Name = "Mens CSharp Whitesmoke Top",
                    Description =
                        "Embrace a clean and timeless look with the Mens CSharp Whitesmoke Top. Its classic design and " +
                        "neutral color are perfect for any outfit.",
                    Price = 7000,
                    PictureUrl = "/images/products/mens_csharp_whitesmoke_top.png",
                    TransitionPictureUrl = "/images/products/mens_csharp_whitesmoke_top_model.png",
                    Brand = "CSharp",
                    Type = "Tops",
                    Color = "whitesmoke",
                    Sex = "Men"
                },
                new Product
                {
                    Name = "Mens Kubernetes Green Top",
                    Description =
                        "Show your love for technology and style with our Mens Kubernetes Green Top. This unique design is perfect " +
                        "for tech enthusiasts and fashion-forward individuals.",
                    Price = 7500,
                    PictureUrl = "/images/products/mens_kubernetes_green_top.png",
                    TransitionPictureUrl = "/images/products/mens_kubernetes_green_top_model.png",
                    Brand = "Kubernetes",
                    Type = "Tops",
                    Color = "green",
                    Sex = "Men"
                },
                new Product
                {
                    Name = "Mens TypeScript Black Top",
                    Description =
                        "The Mens TypeScript Black Top is a sleek and versatile choice for any occasion. Its dark color adds a " +
                        "touch of elegance to your outfit.",
                    Price = 8000,
                    PictureUrl = "/images/products/mens_typescript_black_top.png",
                    TransitionPictureUrl = "/images/products/mens_typescript_black_top_model.png",
                    Brand = "TypeScript",
                    Type = "Tops",
                    Color = "black",
                    Sex = "Men"
                },
                new Product
                {
                    Name = "Mens CSharp Whitesmoke Jacket",
                    Description =
                        "Stay warm in style with the Mens CSharp Whitesmoke Jacket. This jacket combines fashion and function for a trendy look.",
                    Price = 25000,
                    PictureUrl = "/images/products/mens_csharp_whitesmoke_jacket.png",
                    TransitionPictureUrl = "/images/products/mens_csharp_whitesmoke_jacket_model.png",
                    Brand = "CSharp",
                    Type = "Jackets",
                    Color = "whitesmoke",
                    Sex = "Men"
                },
                new Product
                {
                    Name = "Mens React Black Jacket",
                    Description =
                        "Our Mens React Black Jacket is perfect for the cooler seasons. It offers a modern design and reliable warmth.",
                    Price = 25000,
                    PictureUrl = "/images/products/mens_react_black_jacket.png",
                    TransitionPictureUrl = "/images/products/mens_react_black_jacket_model.png",
                    Brand = "React",
                    Type = "Jackets",
                    Color = "black",
                    Sex = "Men"
                },
                new Product
                {
                    Name = "Mens Docker Grey Bottom",
                    Description =
                        "Complete your outfit with the Mens Docker Grey Bottom. These comfortable and stylish bottoms are perfect for a casual look.",
                    Price = 11000,
                    PictureUrl = "/images/products/mens_docker_grey_bottom.png",
                    TransitionPictureUrl = "/images/products/mens_docker_grey_bottom_model.png",
                    Brand = "Docker",
                    Type = "Bottoms",
                    Color = "grey",
                    Sex = "Men"
                },
                new Product
                {
                    Name = "Mens Kubernetes Whitesmoke Bottom",
                    Description =
                        "The Mens Kubernetes Whitesmoke Bottom is a versatile addition to your wardrobe. Its neutral " +
                        "color makes it easy to pair with other clothing items.",
                    Price = 13000,
                    PictureUrl = "/images/products/mens_kubernetes_whitesmoke_bottom.png",
                    TransitionPictureUrl = "/images/products/mens_kubernetes_whitesmoke_bottom_model.png",
                    Brand = "Kubernetes",
                    Type = "Bottoms",
                    Color = "whitesmoke",
                    Sex = "Men"
                },
                new Product
                {
                    Name = "Mens Redux Black Bottom",
                    Description =
                        "Enhance your style with the Mens Redux Black Bottom. These bottoms offer a sleek and modern look " +
                        "for everyday wear.",
                    Price = 12000,
                    PictureUrl = "/images/products/mens_redux_black_bottom.png",
                    TransitionPictureUrl = "/images/products/mens_redux_black_bottom_model.png",
                    Brand = "Redux",
                    Type = "Bottoms",
                    Color = "black",
                    Sex = "Men"
                },
                new Product
                {
                    Name = "Womens CSharp Pink Top",
                    Description =
                        "Make a statement with our Womens CSharp Pink Top. Its vibrant color and trendy design are " +
                        "perfect for adding a pop of color to your wardrobe.",
                    Price = 9000,
                    PictureUrl = "/images/products/womens_csharp_pink_top.png",
                    TransitionPictureUrl = "/images/products/womens_csharp_pink_top_model.png",
                    Brand = "CSharp",
                    Type = "Tops",
                    Color = "pink",
                    Sex = "Women"
                },
                new Product
                {
                    Name = "Womens React Grey Top",
                    Description =
                        "The Womens React Grey Top combines comfort and style. It's a great choice for a " +
                        "relaxed and fashionable look.",
                    Price = 9000,
                    PictureUrl = "/images/products/womens_react_grey_top.png",
                    TransitionPictureUrl = "/images/products/womens_react_grey_top_model.png",
                    Brand = "React",
                    Type = "Tops",
                    Color = "grey",
                    Sex = "Women"
                },
                new Product
                {
                    Name = "Womens TypeScript Whitesmoke Top",
                    Description =
                        "Stay chic and comfortable in our Womens TypeScript Whitesmoke Top. Its classic color " +
                        "and design make it a versatile option.",
                    Price = 9000,
                    PictureUrl = "/images/products/womens_typescript_whitesmoke_top.png",
                    TransitionPictureUrl = "/images/products/womens_typescript_whitesmoke_top_model.png",
                    Brand = "TypeScript",
                    Type = "Tops",
                    Color = "whitesmoke",
                    Sex = "Women"
                },
                new Product
                {
                    Name = "Womens Redux Green Top",
                    Description =
                        "Elevate your fashion game with the Womens Redux Green Top. Its eye-catching color and " +
                        "design are sure to turn heads.",
                    Price = 9500,
                    PictureUrl = "/images/products/womens_redux_green_top.png",
                    TransitionPictureUrl = "/images/products/womens_redux_green_top_model.png",
                    Brand = "Redux",
                    Type = "Tops",
                    Color = "green",
                    Sex = "Women"
                },
                new Product
                {
                    Name = "Womens Kubernetes Lightblue Top",
                    Description =
                        "Embrace a tech-inspired look with the Womens Kubernetes Lightblue Top. It's a perfect choice " +
                        "for tech enthusiasts who want to stay stylish.",
                    Price = 9500,
                    PictureUrl = "/images/products/womens_kubernetes_lightblue_top.png",
                    TransitionPictureUrl = "/images/products/womens_kubernetes_lightblue_top_model.png",
                    Brand = "Kubernetes",
                    Type = "Tops",
                    Color = "lightblue",
                    Sex = "Women"
                },
                new Product
                {
                    Name = "Womens Docker Black Top",
                    Description =
                        "The Womens Docker Black Top is a versatile addition to your wardrobe. Its black color and " +
                        "comfortable fit make it a great choice for any occasion.",
                    Price = 9500,
                    PictureUrl = "/images/products/womens_docker_black_top.png",
                    TransitionPictureUrl = "/images/products/womens_docker_black_top_model.png",
                    Brand = "Docker",
                    Type = "Tops",
                    Color = "black",
                    Sex = "Women"
                },
                new Product
                {
                    Name = "Womens Docker Whitesmoke Jacket",
                    Description =
                        "Stay cozy and fashionable with our Womens Docker Whitesmoke Jacket. This jacket offers " +
                        "both style and warmth.",
                    Price = 24000,
                    PictureUrl = "/images/products/womens_docker_whitesmoke_jacket.png",
                    TransitionPictureUrl = "/images/products/womens_docker_whitesmoke_jacket_model.png",
                    Brand = "Docker",
                    Type = "Jackets",
                    Color = "whitesmoke",
                    Sex = "Women"
                },
                new Product
                {
                    Name = "Womens TypeScript Black Jacket",
                    Description =
                        "The Womens TypeScript Black Jacket is a trendy and functional choice for cooler days. Its " +
                        "dark color adds sophistication to your outfit.",
                    Price = 27000,
                    PictureUrl = "/images/products/womens_typescript_black_jacket.png",
                    TransitionPictureUrl = "/images/products/womens_typescript_black_jacket_model.png",
                    Brand = "TypeScript",
                    Type = "Jackets",
                    Color = "black",
                    Sex = "Women"
                },
                new Product
                {
                    Name = "Womens CSharp Grey Bottom",
                    Description =
                        "Complete your look with the Womens CSharp Grey Bottom. These bottoms offer both style and " +
                        "comfort for everyday wear.",
                    Price = 11000,
                    PictureUrl = "/images/products/womens_csharp_grey_bottom.png",
                    TransitionPictureUrl = "/images/products/womens_csharp_grey_bottom_model.png",
                    Brand = "CSharp",
                    Type = "Bottoms",
                    Color = "grey",
                    Sex = "Women"
                },
                new Product
                {
                    Name = "Womens Redux Whitesmoke Bottom",
                    Description =
                        "The Womens Redux Whitesmoke Bottom is a versatile addition to your wardrobe. Its neutral color " +
                        "makes it easy to mix and match with other clothing items.",
                    Price = 9000,
                    PictureUrl = "/images/products/womens_redux_whitesmoke_bottom.png",
                    TransitionPictureUrl = "/images/products/womens_redux_whitesmoke_bottom_model.png",
                    Brand = "Redux",
                    Type = "Bottoms",
                    Color = "whitesmoke",
                    Sex = "Women"
                },
                new Product
                {
                    Name = "Womens React Black Bottom",
                    Description =
                        "Enhance your style with the Womens React Black Bottom. These bottoms offer a modern and trendy " +
                        "look for any occasion.",
                    Price = 12000,
                    PictureUrl = "/images/products/womens_react_black_bottom.png",
                    TransitionPictureUrl = "/images/products/womens_react_black_bottom_model.png",
                    Brand = "React",
                    Type = "Bottoms",
                    Color = "black",
                    Sex = "Women"
                },
            };

            foreach (var product in products)
            {
                await context.AddAsync(product);
            }
        }

    }
}