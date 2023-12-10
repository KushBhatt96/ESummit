using API.Controllers.Helpers;
using Common.Contants;
using Domain;
using Domain.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TokenController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenHelper _tokenHelper;

        public TokenController(UserManager<AppUser> userManager, ITokenHelper tokenHelper)
        {
            _userManager = userManager;
            _tokenHelper = tokenHelper;
        }

        [HttpPost]
        public async Task<ActionResult> Refresh()
        {
            if(!Request.Cookies.TryGetValue("access-token", out var accessToken) || !Request.Cookies.TryGetValue("refresh-token", out var refreshToken))
            {
                return BadRequest("Invalid request.");
            }

            if(string.IsNullOrWhiteSpace(accessToken) || string.IsNullOrWhiteSpace(refreshToken))
            {
                return BadRequest("Invalid request.");
            }

            var principal = _tokenHelper.GetPrincipalFromExpiredToken(accessToken);
            var username = principal.Identity?.Name;

            if (string.IsNullOrWhiteSpace(username))
            {
                throw new Exception("Server error occurred.");
            }

            var user = await _userManager.FindByNameAsync(username);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime == null || user.RefreshToken == null)
            {
                return BadRequest("Invalid request.");
            }

            if(user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid request.");
            }

            var newAccessToken = _tokenHelper.GenerateAccessToken(principal.Claims);
            user.RefreshToken = _tokenHelper.GenerateRefreshToken();

            await _userManager.UpdateAsync(user);

            // Place new tokens into httponly cookies
            Response.Cookies.Append("access-token", newAccessToken,
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

            return StatusCode(StatusCodes.Status200OK);
        }

        [HttpPost, Authorize(Roles = RoleNames.Admin)]
        public async Task<ActionResult> Revoke()
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;

            if (userName == null)
            {
                throw new Exception("A problem occurred. Please try again later.");
            }

            var user = await _userManager.FindByNameAsync(userName);

            user.RefreshToken = null;

            await _userManager.UpdateAsync(user);

            return NoContent();
        }
    }
}
