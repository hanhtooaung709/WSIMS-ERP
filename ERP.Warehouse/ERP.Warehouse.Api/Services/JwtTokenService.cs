using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options; // 💡 ဒါ တိုးရပါမယ်
using ERP.Warehouse.Models.Models.Signin.Signin;
using WSIMS_ERP.Shared;
using WSIMS_ERP.Shared.Models.ConfigModel; // CustomSettingModel ရှိရာ နေရာ

namespace ERP.Warehouse.Api.Services;

public class JwtTokenService
{
    private readonly CustomSettingModel _setting;

    // IConfiguration အစား IOptions ကို သုံးပြီး ဖတ်ခိုင်းလိုက်ပါမယ်
    public JwtTokenService(IOptions<CustomSettingModel> setting)
    {
        _setting = setting.Value;
    }

    public string GenerateAccessToken(SigninResModel reqModel, string userName, string roleCode)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            // Class ထဲကနေ တိုက်ရိုက်ဆွဲဖတ်ခြင်း
            var secret = _setting.Jwt?.Key ?? "FALLBACK_SECRET_KEY_THAT_IS_LONG_ENOUGH_FOR_SHA256_AUTHENTICATION_2026";
            var subject = _setting.Jwt?.Subject ?? "WarehouseERP";

            var key = Encoding.UTF8.GetBytes(secret);
            var tokenExpire = DateTime.UtcNow.AddMinutes(60);

            #region Create HashString For Claim

            var rawClaimData = $"{reqModel.UserId}|{reqModel.SessionId}|{userName}|{roleCode}";
            var hashString = rawClaimData.ToEncrypt().ToBase64Encode();

            #endregion

            #region Generate Jwt Token

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, subject),
                    new Claim(JwtRegisteredClaimNames.Jti, reqModel.SessionId),
                    new Claim("Claim", hashString),
                    new Claim("UserId", reqModel.UserId)
                }),
                Expires = tokenExpire,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var generateToken = tokenHandler.WriteToken(token);

            var hashToken = generateToken.ToEncrypt().ToBase64Encode();
            return hashToken;

            #endregion
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[JWT Service Error]: {ex.Message}");
            throw;
        }
    }
}