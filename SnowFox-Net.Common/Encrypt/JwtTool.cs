using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SnowFox_Net.Shared.VOs;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SnowFox_Net.Common.Encrypt
{
    public class JwtTool
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expirationMinutes;

        public JwtTool(string secretKey, string issuer, string audience, int expirationMinutes=30)
        {
            _secretKey = secretKey;
            _issuer = issuer;
            _audience = audience;
            _expirationMinutes = expirationMinutes;
        }



        /// <summary>
        /// 生成 JWT 令牌
        /// </summary>
        public string GenerateToken(Token claim)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            claim.IssuedTime = DateTime.Now;
            claim.ExpTime = claim.IssuedTime.AddMinutes(_expirationMinutes);

            var claims = new[]
             {
                new Claim("claim",JsonConvert.SerializeObject(claim))
            };
            var token = new JwtSecurityToken(
              issuer: _issuer,
              audience: _audience,
              claims: claims,
              expires: claim.ExpTime,
              signingCredentials: credentials
          );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// 解析并验证 JWT 令牌
        /// </summary>
        public ClaimsPrincipal ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _issuer,
                    ValidAudience = _audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey))
                };

                return tokenHandler.ValidateToken(token, validationParameters, out _);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 从 Token 获取数据
        /// </summary>
        public Token GetTokenData(string token)
        {
            var principal = ValidateToken(token);
            var claim = principal?.FindFirst("claim");
            return claim!=null?JsonConvert.DeserializeObject<Token>(claim.Value):null;
        }


    }
}
