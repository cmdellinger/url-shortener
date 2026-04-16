using Core.Dtos;
using Core.Entities;
using Core.Interfaces;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(UserManager<AppUser> userManager,
                                ITokenService tokenService,
                                IConfiguration config) : ControllerBase
    {
        [HttpPost("google")]
        public async Task<ActionResult<AuthResponseDto>> LoginWithGoogle(GoogleLoginDto googleLoginDto)
        {
            // decode Google token from frontend
            GoogleJsonWebSignature.Payload payload;
            try
            {
                payload = await GoogleJsonWebSignature.ValidateAsync(
                    googleLoginDto.IdToken,
                    new GoogleJsonWebSignature.ValidationSettings
                    { 
                        Audience = new[] { config["Google:ClientId"] } 
                    }
                );
            }
            catch (InvalidJwtException)
            {
                return Unauthorized();
            }
            if (string.IsNullOrEmpty(payload.Email))
                return BadRequest("Google token did not include an email claim. Request the 'email' scope.");

            // get user from database
            var user = await userManager.FindByLoginAsync("Google", payload.Subject);
            // register user if user doesn't exist in the database
            if (user == null)
            {
                user = new AppUser
                {
                    UserName = payload.Email,
                    Email = payload.Email,
                    DisplayName = payload.Name,
                    CreatedAt = DateTimeOffset.UtcNow
                };
                
                var registerAttempt = await userManager.CreateAsync(user);
                if (!registerAttempt.Succeeded) return BadRequest(registerAttempt.Errors);

                var addLoginAttempt = await userManager.AddLoginAsync(
                    user,
                    new UserLoginInfo ("Google", payload.Subject, "Google")
                );
                if (!addLoginAttempt.Succeeded) return BadRequest(addLoginAttempt.Errors);
            }
            // generate token
            var token = tokenService.CreateToken(user);
            return Ok(new AuthResponseDto
            {
                Token = token,
                User = UserDto.FromEntity(user)
            });
        }
    }
}
