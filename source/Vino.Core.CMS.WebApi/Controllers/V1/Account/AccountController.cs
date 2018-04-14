﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Vino.Core.Cache;
using Vino.Core.CMS.Domain;
using Vino.Core.CMS.Domain.Dto.Membership;
using Vino.Core.CMS.Web.Base;
using Vino.Core.CMS.Web.Configs;
using Vino.Core.CMS.Web.Extensions;
using Vino.Core.CMS.Web.Filters;
using Vino.Core.CMS.Web.Security;
using Vino.Core.Infrastructure.IdGenerator;
using Vino.Core.Tokens.Jwt;

namespace Vino.Core.CMS.WebApi.Controllers.V1.Account
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    public class AccountController : WebApiController
    {
        private IJwtProvider _jwtProvider;
        private JwtConfig _jwtConfig;
        private ICacheService _cacheService;

        public AccountController(IJwtProvider jwtProvider, ICacheService cacheService, IOptions<JwtConfig> jwtConfig)
        {
            _jwtProvider = jwtProvider;
            _cacheService = cacheService;
            this._jwtConfig = jwtConfig.Value;
        }

        [HttpGet("login")]
        [HttpPost("login")]
        [NeedVerifyCode]
        public async Task<JsonResult> Login(string mobile, string password)
        {
            //验证
            var member = new MemberDto { Id = 1, Name = "测试" };
            //生成Token
            var tokenVersion = DateTime.Now.Ticks.ToString();

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Version, tokenVersion)
                ,new Claim(ClaimTypes.NameIdentifier, member.Id.ToString())
                ,new Claim(ClaimTypes.Name, member.Name)
                ,new Claim(ClaimTypes.Role, ((int)MemberRole.Default).ToString())
            };

            var token = _jwtProvider.CreateToken(claims);

            var loginMember = new LoginMember {
                Id = member.Id,
                Name = member.Name
            };

            //如果当前已登陆，则退出当前登录
            DoLogout();

            _cacheService.Add(string.Format(CacheKeyDefinition.ApiMemberToken, member.Id, tokenVersion), loginMember, TimeSpan.FromMinutes(_jwtConfig.ExpiredMinutes));

            return Json(token);
        }

        //退出登录
        private void DoLogout()
        {
            if (User != null && !string.IsNullOrEmpty(User.GetTokenIdOrNull()))
            {
                _cacheService.Add(string.Format(CacheKeyDefinition.ApiExpiredToken, User.GetTokenIdOrNull()),"", TimeSpan.FromMinutes(_jwtConfig.ExpiredMinutes));
                _cacheService.Remove(string.Format(CacheKeyDefinition.ApiMemberToken, User.GetUserIdOrZero(), User.GetVersion()));
            }
        }
    }
}
