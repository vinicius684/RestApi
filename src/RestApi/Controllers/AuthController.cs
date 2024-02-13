﻿using DevIO.Api.Controllers;
using DevIO.Api.Extensions;
using DevIO.Api.ViewModels;
using DevIO.Business.Intefaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RestApi.Controllers;
using RestApi.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RestApi.Controllers
{
    //[ApiVersion("1.0")]
    [Route("api/conta")]
    public class AuthController : MainController
    {
        private readonly SignInManager<IdentityUser> _signInManager;//fazer a autenticação
        private readonly UserManager<IdentityUser> _userManager;//criar o user e fazer outras manipulações

        public AuthController(INotificador notificador,
                              SignInManager<IdentityUser> signInManager,
                              UserManager<IdentityUser> userManager)
                               : base(notificador)
        {
            _signInManager = signInManager;
            _userManager = userManager;
          
        }

        //[EnableCors("Development")]
        [HttpPost("nova-conta")]
        public async Task<ActionResult> Registrar(RegisterUserViewModel registerUser)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = new IdentityUser
            {
                UserName = registerUser.Email,
                Email = registerUser.Email,
                EmailConfirmed = true
            };

            //Password não faz parte do identity user -> toda vez que usuário fizer login, o password vai ser criptografado e comparado com o hash da criptografia que está na base de dados
            var result = await _userManager.CreateAsync(user, registerUser.Password);//criar
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);//login
                return CustomResponse(registerUser);
                //return CustomResponse(await GerarJwt(user.Email));
            }
            foreach (var error in result.Errors)
            {
                NotificarErro(error.Description);
            }

            return CustomResponse(registerUser);
        }

        [HttpPost("entrar")]
        public async Task<ActionResult> Login(LoginUserViewModel loginUser)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);

            if (result.Succeeded)
            {
                //_logger.LogInformation("Usuario " + loginUser.Email + " logado com sucesso");
                return CustomResponse(loginUser);
                //return CustomResponse(await GerarJwt(loginUser.Email));
            }
            if (result.IsLockedOut)
            {
                NotificarErro("Usuário temporariamente bloqueado por tentativas inválidas");
                return CustomResponse(loginUser);
            }

            NotificarErro("Usuário ou Senha incorretos");
            return CustomResponse(loginUser);
        }

        //private async Task<LoginResponseViewModel> GerarJwt(string email)
        //{
        //    var user = await _userManager.FindByEmailAsync(email);
        //    var claims = await _userManager.GetClaimsAsync(user);
        //    var userRoles = await _userManager.GetRolesAsync(user);

        //    claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
        //    claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
        //    claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        //    claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
        //    claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));
        //    foreach (var userRole in userRoles)
        //    {
        //        claims.Add(new Claim("role", userRole));
        //    }

        //    var identityClaims = new ClaimsIdentity();
        //    identityClaims.AddClaims(claims);

        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        //    var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        //    {
        //        Issuer = _appSettings.Emissor,
        //        Audience = _appSettings.ValidoEm,
        //        Subject = identityClaims,
        //        Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        //    });

        //    var encodedToken = tokenHandler.WriteToken(token);

        //    var response = new LoginResponseViewModel
        //    {
        //        AccessToken = encodedToken,
        //        ExpiresIn = TimeSpan.FromHours(_appSettings.ExpiracaoHoras).TotalSeconds,
        //        UserToken = new UserTokenViewModel
        //        {
        //            Id = user.Id,
        //            Email = user.Email,
        //            Claims = claims.Select(c => new ClaimViewModel { Type = c.Type, Value = c.Value })
        //        }
        //    };

        //    return response;
        //}

        //private static long ToUnixEpochDate(DateTime date)
        //    => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}