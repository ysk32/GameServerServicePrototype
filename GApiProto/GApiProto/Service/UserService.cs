using GApiProto.Dto.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using GApiProto.Dto.Responce;
using Microsoft.Extensions.Logging;
using GApiProto.UserModel;
using GApiProto.DataAccess;

namespace GApiProto.Service
{
    public class UserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly userContext _userContext;
        private readonly UserProfileService _userProfileService;

        public UserService(UserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        public async Task<LoginDto> Login(LoginPostDto loginPostDto)
        {
            // 認証
            var userProfile = _userProfileService.GetByUserId(loginPostDto?.UserId)?.Result;
            if (userProfile == null)
            {
                return new LoginDto();
                //return new CustomJsonResult(HttpStatusCode.BadRequest, "User name or password is incorrect.");
            }

            //認証
            //bool authResult = (await AuthenticateAsync(userProfile.UserId));
            //if (authResult == false)
            //{
            //    return new LoginDto();
            //    //return new CustomJsonResult(HttpStatusCode.BadRequest, "User name or password is incorrect.");
            //}

            //認可処理してトークンを作成
            var token = GenerateToken(userProfile.Id);

            //var result = new
            //{
            //    Token = token,
            //    UserName = userProfile.UserId,
            //    ExpiresIn = AuthConfig.ApiJwtExpirationSec
            //};

            return new LoginDto
            {
                UserProfile = userProfile,
                Token = token,
                ExpiresIn = AuthConfig.ApiJwtExpirationSec
            };

            //return new CustomJsonResult(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// 認証処理
        /// </summary>
        //private async Task<bool> AuthenticateAsync(string userId)
        //{
        //    //何かしらの認証処理（Kerberos認証したり、LDAPしたり、よしなに）
        //    //await Task.CompletedTask;

            
        //    //今はユーザ名/パスワードがhoge/hugaならtrue
        //    return userName == "hoge" && password == "huga";
        //}

        /// <summary>
        /// 認可処理
        /// </summary>
        private List<Claim> Authorize(UInt64 userId)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Sid, userId.ToString()));
            //claims.Add(new Claim(ClaimTypes.Name, userName));

            //何かしらの認可処理（グループ付けたり、ロール付けたり、よしなに）
            //string groupId = "piyo";
            //claims.Add(new Claim(ClaimTypes.GroupSid, groupId));

            return claims;
        }

        /// <summary>
        /// JWTトークンを発行する
        /// </summary>
        /// <param name="userId">ユーザID</param>
        /// <param name="expiresIn">有効期限（秒）</param>
        private string GenerateToken(UInt64 userId)
        {
            //トークンに含めるクレームの入れ物
            List<Claim> claims = Authorize(userId);

            //現在時刻をepochタイムスタンプに変更
            var now = DateTime.UtcNow;
            long epochTime = (long)Math.Round((
                now.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)
                ).TotalSeconds);

            //JWT ID（トークン生成ごとに一意になるようなトークンのID）。ランダムなGUIDを採用する。
            string jwtId = Guid.NewGuid().ToString();
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, jwtId));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, epochTime.ToString(), ClaimValueTypes.Integer64));

            //期限が切れる時刻
            DateTime expireDate = now + TimeSpan.FromSeconds(AuthConfig.ApiJwtExpirationSec);

            // Json Web Tokenを生成
            var jwt = new JwtSecurityToken(
                AuthConfig.ApiJwtIssuer, //発行者(iss)
                AuthConfig.ApiJwtAudience, //トークンの受け取り手（のリスト）
                claims, //付与するクレーム(sub,jti,iat)
                now, //開始時刻(nbf)（not before = これより早い時間のトークンは処理しない）
                expireDate, //期限(exp)
                new SigningCredentials(AuthConfig.ApiJwtSigningKey, SecurityAlgorithms.HmacSha256) //署名に使うCredential
                );
            //トークンを作成（トークンは上記クレームをBase64エンコードしたものに署名をつけただけ）
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        public static Int64 getUserIdFromClaimsIdentity(ClaimsIdentity claimsIdentity)
        {
            var id = claimsIdentity?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            return Int64.Parse(id);
        }
    }
}
