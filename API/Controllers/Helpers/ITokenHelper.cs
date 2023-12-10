using System.Security.Claims;

namespace API.Controllers.Helpers
{
    public interface ITokenHelper
    {
        public string GenerateAccessToken(IEnumerable<Claim> claims);

        public string GenerateRefreshToken();

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}