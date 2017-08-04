using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Users.Models;

namespace Users.Controllers
{
    [Route("api/[controller]")]
    public class AccountApiController:Controller
    {

        private UserManager<AppUser> userManager;
        private SignInManager<AppUser> signInManager;
        private PasswordHasher<AppUser> passwordHasher;
        private IConfigurationRoot appConfiguration;

        public AccountApiController(UserManager<AppUser> userMgr, SignInManager<AppUser> signinMgr, IHostingEnvironment env)
        {
            userManager = userMgr;
            signInManager = signinMgr;
            passwordHasher = new PasswordHasher<AppUser>();

            appConfiguration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json").Build();



        }
        

        
        [HttpPost("token")]
        public async Task<IActionResult> Token([FromBody] LoginModel model)
        {
            //[FromBody] LoginModel model
            //LoginModel model =new LoginModel(){Email = "bob@example.com",Password = "secret123"};
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            AppUser user = await userManager.FindByEmailAsync(model.Email);

            if (user == null || passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password) != PasswordVerificationResult.Success)
            {
                return BadRequest();
            }

            JwtSecurityToken token = await GetJwtSecurityToken(user);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }

        private async Task<JwtSecurityToken> GetJwtSecurityToken(AppUser user)
        {
            IList<Claim> userClaims = await userManager.GetClaimsAsync(user);
            JwtSecurityToken jwt = new JwtSecurityToken();
            return new JwtSecurityToken(
                issuer: appConfiguration["JwtBearer:SiteUrl"],
                audience: appConfiguration["JwtBearer:SiteUrl"],
                claims: GetTokenClaims(user).Union(userClaims),
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: new SigningCredentials
                    (new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appConfiguration["JwtBearer:Key"])), SecurityAlgorithms.HmacSha256)
            );
        }

        private static IEnumerable<Claim> GetTokenClaims(AppUser user)
        {
            return new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName)
            };
        }
    }
}
