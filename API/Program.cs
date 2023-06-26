using API.Data;
using API.Middleware;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container. ORDER *NOT* IMPORTANT HERE.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<StoreContext>(opt =>
            {
                opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddCors();

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