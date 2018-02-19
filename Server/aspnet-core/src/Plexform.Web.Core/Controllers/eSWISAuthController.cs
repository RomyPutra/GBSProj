using Abp.Runtime.Security;
using Microsoft.AspNetCore.Mvc;
using Plexform.Authentication.JwtBearer;
using Plexform.Models;
using Plexform.Sessions.Dto;
using Plexform.SignalR;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Plexform.Controllers
{
    [Route("api/eswis/[controller]/[action]")]
    public class ESWISAuthController : PlexformControllerBase
    {
        private readonly TokenAuthConfiguration _configuration;

        public ESWISAuthController(TokenAuthConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations(string userID)
        {
            var output = new GetCurrentLoginInformationsOutput
            {
                Application = new ApplicationInfoDto
                {
                    Version = AppVersionHelper.Version,
                    ReleaseDate = AppVersionHelper.ReleaseDate,
                    Features = new Dictionary<string, bool>
                    {
                        { "SignalR", SignalRFeature.IsAvailable },
                        { "SignalR.AspNetCore", SignalRFeature.IsAspNetCore }
                    }
                }
            };

			if (!String.IsNullOrWhiteSpace(userID))
			{
				var loginResult = await GetLoginResultAsync(userID);
				if (loginResult != null)
				{
					var hexUserId = StringToHex(loginResult.UserID);
					var lng = Convert.ToInt64(hexUserId, 16);
					output.User = new UserLoginInfoDto
					{
						Id = lng,
						UserName = loginResult.UserName,
						Name = loginResult.UserName,
						Surname = loginResult.UserName
					};
				}
			}
            //if (AbpSession.UserId.HasValue)
            //{
            //    output.User = ObjectMapper.Map<UserLoginInfoDto>(await GetCurrentUserAsync());
            //}

            return output;
        }

        [HttpPost]
        public async Task<ESWISAuthResultModel> Authenticate([FromBody] ESWISAuthModel model)
        {
            var loginResult = await GetLoginResultAsync(model.UserName, model.Password);

            //var hexUserId = StringToHex(loginResult.UserID);
            //var lng = Convert.ToInt64(hexUserId, 16);

            //hexUserId = lng.ToString("X");
            //var userId = HexToString(hexUserId);

            var userIdentity = new ClaimsIdentity();
            userIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, loginResult.UserName));
            userIdentity.AddClaim(new Claim(ClaimTypes.Role, loginResult.GroupName));

            var accessToken = CreateAccessToken(CreateJwtClaims(userIdentity));
            return new ESWISAuthResultModel
            {
                AccessToken = accessToken,
                EncryptedAccessToken = GetEncrpyedAccessToken(accessToken),
                ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds,
                UserId = loginResult.UserID,
                GroupName = loginResult.GroupName
            };
        }

        private Task<eSWIS.Logic.UsrProfile.Container.USRPROFILE> GetLoginResultAsync(string userName, string password)
        {
            var loginResult = new Plexform.Logic.EswisAuthLogic();
            return loginResult.Authenticate(userName, password);
        }

        private Task<eSWIS.Logic.UsrProfile.Container.USRPROFILE> GetLoginResultAsync(string userID)
        {
            var loginResult = new Plexform.Logic.EswisAuthLogic();
            return loginResult.Authenticate(userID);
        }

        private string CreateAccessToken(IEnumerable<Claim> claims, TimeSpan? expiration = null)
        {
            var now = DateTime.UtcNow;

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration.Issuer,
                audience: _configuration.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(expiration ?? _configuration.Expiration),
                signingCredentials: _configuration.SigningCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        private static List<Claim> CreateJwtClaims(ClaimsIdentity identity)
        {
            var claims = identity.Claims.ToList();
            var nameIdClaim = claims.First(c => c.Type == ClaimTypes.NameIdentifier);

            // Specifically add the jti (random nonce), iat (issued timestamp), and sub (subject/user) claims.
            claims.AddRange(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, nameIdClaim.Value),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            });

            return claims;
        }

        private string GetEncrpyedAccessToken(string accessToken)
        {
            return SimpleStringCipher.Instance.Encrypt(accessToken, AppConsts.DefaultPassPhrase);
        }

        private string HexToString(String hexString)
        {
            try
            {
                string ascii = string.Empty;

                for (int i = 0; i < hexString.Length; i += 2)
                {
                    String hs = string.Empty;

                    hs = hexString.Substring(i, 2);
                    uint decval = System.Convert.ToUInt32(hs, 16);
                    char character = System.Convert.ToChar(decval);
                    ascii += character;
                }

                return ascii;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            return string.Empty;
        }

        private string StringToHex(string input)
        {
            char[] values = input.ToCharArray();
            var output = string.Empty;
            foreach (char letter in values)
            {
                // Get the integral value of the character.
                int value = Convert.ToInt32(letter);
                // Convert the decimal value to a hexadecimal value in string form.
                string hexOutput = String.Format("{0:X}", value);
                output += hexOutput;
            }
            return output;
        }
    }
}
