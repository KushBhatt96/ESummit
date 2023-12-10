using API.Controllers.Helpers;
using Application.Carts.Commands.InitializeCart;
using Common.Contants;
using Domain;
using Domain.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using System.Globalization;
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
        private readonly ITokenHelper _tokenHelper;
        private readonly IInitializeCartCommand _initializeCartCommand;

        public AccountController(StoreContext context,
                                 ILogger<AccountController> logger,
                                 IConfiguration configuration,
                                 UserManager<AppUser> userManager,
                                 SignInManager<AppUser> signInManager,
                                 ITokenHelper tokenHelper,
                                 IInitializeCartCommand initializeCartCommand)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
            _userManager = userManager;
            _tokenHelper = tokenHelper;
            _initializeCartCommand = initializeCartCommand;
        }

        // TODO: Need to prevent logged in users from being able to call this endpoint
        [HttpPost]
        public async Task<ActionResult> Register(RegisterDTO registerDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newUser = new AppUser
                    {
                        FirstName = registerDTO.FirstName,
                        LastName = registerDTO.LastName,
                        DateOfBirth = DateTime.ParseExact(registerDTO.DateOfBirth, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                        UserName = registerDTO.UserName,
                        Email = registerDTO.Email,
                    };

                    var result = await _userManager.CreateAsync(newUser, registerDTO.Password);

                    if (!result.Succeeded)
                    {
                        // TODO: Add logging about error here
                        throw new Exception($"Error: {string.Join(" ", result.Errors.Select(e => e.Description))}");
                    }

                    // Now assign a customer role to this new user
                    await _userManager.AddToRoleAsync(newUser, RoleNames.Customer);

                    // TODO: Log "User {userName} ({email}) has been created." here
                    return StatusCode(StatusCodes.Status201Created, $"User '{newUser.UserName}' has been created.");
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
        public async Task<ActionResult> Login(LoginDTO loginDTO)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(loginDTO.UserName);
                if (user == null || !await _userManager.CheckPasswordAsync(user, loginDTO.Password))
                {
                    // TODO: Logging
                    // TODO: Use or create more specific Exception objects
                    throw new Exception("Invalid login attempt.");
                }
                else
                {
                    var claims = await SetupClaimsAsync(user);
                    var accessToken = _tokenHelper.GenerateAccessToken(claims);

                    user.RefreshToken = _tokenHelper.GenerateRefreshToken();
                    user.RefreshTokenExpiryTime = DateTime.Now.AddHours(24);

                    await _userManager.UpdateAsync(user);

                    await _initializeCartCommand.ExecuteAsync(user);

                    // Place access and refresh tokens into httponly cookie
                    Response.Cookies.Append("access-token", accessToken,
                        new CookieOptions
                        {
                            Expires = DateTime.Now.AddHours(24),
                            HttpOnly = true,
                            Secure = true,
                            IsEssential = true,
                            SameSite = SameSiteMode.None
                        });

                    Response.Cookies.Append("refresh-token", user.RefreshToken,
                        new CookieOptions
                        {
                            Expires = DateTime.Now.AddHours(24),
                            HttpOnly = true,
                            Secure = true,
                            IsEssential = true,
                            SameSite = SameSiteMode.None
                        });

                    return StatusCode(StatusCodes.Status200OK,
                        new AppUserDTO(user));
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

        [HttpPost, Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Customer}")]
        public async Task<ActionResult> Logout()
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrWhiteSpace(userName))
            {
                return BadRequest("Invalid client request.");
            }

            var user = await _userManager.FindByNameAsync(userName);

            if(user == null)
            {
                return BadRequest("Invalid user.");
            }

            // Revoke ref token
            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);

            // expire cookies (thus removing from client)
            Response.Cookies.Delete("access-token",
                        new CookieOptions
                        {
                            Expires = DateTime.Now.AddYears(-1),
                            HttpOnly = true,
                            Secure = true,
                            IsEssential = true,
                            SameSite = SameSiteMode.None
                        });
            Response.Cookies.Delete("refresh-token",
                        new CookieOptions
                        {
                            Expires = DateTime.Now.AddYears(-1),
                            HttpOnly = true,
                            Secure = true,
                            IsEssential = true,
                            SameSite = SameSiteMode.None
                        });
            return StatusCode(StatusCodes.Status200OK);
        }

        #region Helpers
        private async Task<List<Claim>> SetupClaimsAsync(AppUser user)
        {
            // A claim is something that the user says they are or something they have
            // these claims will be included in the payload of the JWT
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }
        #endregion
    }
}
