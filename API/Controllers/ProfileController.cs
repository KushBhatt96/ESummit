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
    public class ProfileController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;

        public ProfileController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPut]
        [Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Customer}")]
        public async Task<ActionResult> Update(ProfileUpdateDTO profileUpdateDTO)
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrWhiteSpace(userName))
            {
                return BadRequest("Invalid request.");
            }

            var user = await _userManager.FindByNameAsync(userName);

            if(user == null)
            {
                return BadRequest("Invalid request.");
            }

            switch (profileUpdateDTO.Property.ToLower())
            {
                case "firstname":
                    user.FirstName = profileUpdateDTO.Value;
                    break;
                case "lastname":
                    user.LastName = profileUpdateDTO.Value;
                    break;
                case "email":
                    await _userManager.SetEmailAsync(user, profileUpdateDTO.Value);
                    break;
                case "password":
                    await _userManager.ChangePasswordAsync(user, profileUpdateDTO.OldValue, profileUpdateDTO.Value);
                    break;
                default:
                    break;
            }

            await _userManager.UpdateAsync(user);

            var appUserDTO = new AppUserDTO(user);
            return StatusCode(StatusCodes.Status200OK, appUserDTO);
        }
    }
}
