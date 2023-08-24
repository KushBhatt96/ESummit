using API.Middleware;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Application.CartItems.Commands.AddCartItem;
using System.Text.Json.Serialization;
using Application.CartItems.Commands.RemoveCartItem;
using Application.CartItems.Commands.UpdateCartItemQuantity;
using Application.Carts.Queries.GetCart;
using Application.Products.Queries.GetProducts;
using Application.Products.Queries.GetProductDetail;

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container. ORDER *NOT* IMPORTANT HERE.

            builder.Services.AddControllers()
                            .AddJsonOptions(opt =>
                            {
                                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

                                // This configuration is required for System.Text.Json to prevent object cycle errors during json serialization
                                opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<StoreContext>(opt =>
            {
                // Here we are providing EF Core with the DB connection string which is in our appsettings.Development.json file.
                // The reason we have access to the .UseSqlite extension method is bc we installed the EFCore Sqlite provider
                // package. If we had installed some other provider, say EFCore SqlServer, we'd see that extension method instead.
                opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddCors();
            builder.Services.AddScoped<IAddCartItemCommand, AddCartItemCommand>();
            builder.Services.AddScoped<IRemoveCartItemCommand, RemoveCartItemCommand>();
            builder.Services.AddScoped<IUpdateCartItemQuantityCommand, UpdateCartItemQuantityCommand>();
            builder.Services.AddScoped<IGetCartQuery, GetCartQuery>();
            builder.Services.AddScoped<IGetProductsQuery, GetProductsQuery>();
            builder.Services.AddScoped<IGetProductDetailQuery, GetProductDetailQuery>();

            var app = builder.Build();

            // Configure the HTTP request pipeline. AKA middleware pipeline. ORDER *IS* IMPORTANT HERE.
            app.UseMiddleware<ExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // app.UseHttpsRedirection();

            // Server must respond with CORS header indicating that the specified client url is allowed to display the returned data
            app.UseCors(opt =>
            {
                opt.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://127.0.0.1:5173", "http://localhost:5173");
            });

            //TODO: need to understand this middleware a bit better
            app.UseAuthorization();

            // Without this built-in MapControllers middleware, we won't be able to hit our API endpoints
            app.MapControllers();

            var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<StoreContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            try
            {
                await context.Database.MigrateAsync();
                await DbInitializer.InitializeAsync(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "A problem occurred during migration");
            }

            app.Run();
        }
    }
}