using Common.Contants;
using Domain;
using Domain.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AccountController : BaseApiController
    {
        private readonly StoreContext _context;
        private readonly ILogger<AccountController> _logger;
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(StoreContext context,
                                 ILogger<AccountController> logger,
                                 IConfiguration configuration,
                                 UserManager<AppUser> userManager,
                                 SignInManager<AppUser> signInManager)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // TODO: Need to prevent logged in users from being able to call this endpoint
        [HttpPost]
        public async Task<ActionResult> Register(RegisterDTO input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newUser = new AppUser
                    {
                        FirstName = input.FirstName,
                        LastName = input.LastName,
                        DateOfBirth = input.DateOfBirth,
                        UserName = input.UserName,
                        Email = input.Email,
                    };

                    var result = await _userManager.CreateAsync(newUser, input.Password);

                    if (result.Succeeded)
                    {
                        // Now assign a customer role to this new user
                        await _userManager.AddToRoleAsync(newUser, RoleNames.Customer);

                        // TODO: Log "User {userName} ({email}) has been created." here
                        return StatusCode(201, $"User '{newUser.UserName}' has been created.");
                    }
                    else
                    {
                        // TODO: Add logging about error here
                        throw new Exception($"Error: {string.Join(" ", result.Errors.Select(e => e.Description))}");
                    }
                }
                else
                {
                    var details = new ValidationProblemDetails(ModelState);
                    details.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
                    details.Status = StatusCodes.Status400BadRequest;
                    return new BadRequestObjectResult(details);
                }
            }
            catch (Exception e)
            {
                var exceptionDetails = new ProblemDetails();
                exceptionDetails.Detail = e.Message;
                exceptionDetails.Status = StatusCodes.Status500InternalServerError;
                exceptionDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1";
                return StatusCode(StatusCodes.Status500InternalServerError, exceptionDetails);
            }
        }

        // TODO: Need to prevent logged in users from calling this endpoint
        [HttpPost]
        public async Task<ActionResult> Login(LoginDTO input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByNameAsync(input.UserName);
                    if (user == null || !await _userManager.CheckPasswordAsync(user, input.Password))
                    {
                        // TODO: Logging
                        // TODO: Use or create more specific Exception objects
                        throw new Exception("Invalid login attempt.");
                    }
                    else
                    {
                        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SigningKey"])), SecurityAlgorithms.HmacSha256);

                        var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Name, user.UserName));

                        // add a claim for every role that is assigned to this authenticated user
                        claims.AddRange((await _userManager.GetRolesAsync(user)).Select(role => new Claim(ClaimTypes.Role, role)));

                        var jwtObject = new JwtSecurityToken(
                                issuer: _configuration["JWT:Issuer"],
                                audience: _configuration["JWT:Audience"],
                                claims: claims,
                                expires: DateTime.Now.AddSeconds(300),
                                signingCredentials: signingCredentials
                            );

                        var jwtString = new JwtSecurityTokenHandler().WriteToken(jwtObject);

                        // Use EF Explicit Loading to load the cart for this user, or null if it doesn't exist yet
                        // EF Explicit Loading is for loading related properties for objects already in memory, such as this user
                        await _context.Entry(user).Navigation("Cart").LoadAsync();

                        if (user.Cart == null)
                        {
                            var cart = new Cart
                            {
                                CreatedAt = DateTime.Now,
                                LastUpdated = DateTime.Now,
                                CartTotal = 0,
                                AppUserId = user.Id,
                                AppUser = user
                            };

                            await _context.Carts.AddAsync(cart);
                            await _context.SaveChangesAsync();
                        }

                        return StatusCode(StatusCodes.Status200OK, jwtString);
                    }
                }
                else
                {
                    var details = new ValidationProblemDetails(ModelState);
                    details.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
                    details.Status = StatusCodes.Status400BadRequest;
                    return new BadRequestObjectResult(details);
                }
            }
            catch (Exception e)
            {
                var exceptionDetails = new ProblemDetails();
                exceptionDetails.Detail = e.Message;
                exceptionDetails.Status = StatusCodes.Status401Unauthorized;
                exceptionDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1";
                return StatusCode(StatusCodes.Status401Unauthorized, exceptionDetails);
            }
        }

    }
}
