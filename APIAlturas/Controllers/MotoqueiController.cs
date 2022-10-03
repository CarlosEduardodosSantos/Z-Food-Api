using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using APIAlturas.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace APIAlturas.Controllers
{
    [Route("api/[controller]")]
    public class MotoqueiController : Controller
    {
        private readonly MotoqueiroDao _motoqueiroDao;

        public MotoqueiController(MotoqueiroDao motoqueiroDao)
        {
            _motoqueiroDao = motoqueiroDao;
        }

        [AllowAnonymous]
        [HttpPost]
        public object Login([FromBody]Motoqueiro motoqueiro,
            [FromServices]SigningConfigurations signingConfigurations,
            [FromServices]TokenConfigurations tokenConfigurations)
        {
            var motoqueiroBase = new Motoqueiro();
            bool credenciaisValidas = false;
            if (motoqueiro != null && !string.IsNullOrWhiteSpace(motoqueiro.Email))
            {
                motoqueiroBase = _motoqueiroDao.Find(motoqueiro.Email);
                credenciaisValidas = (motoqueiroBase != null &&
                                      motoqueiro.Email.ToLower() == motoqueiroBase.Email.ToLower() &&
                                      motoqueiro.Senha.ToLower() == motoqueiroBase.Senha.ToLower());
            }

            if (credenciaisValidas)
            {
                ClaimsIdentity identity = new ClaimsIdentity(
                    new GenericIdentity(motoqueiro.MotoqueiroId.ToString(), "Login"),
                    new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, motoqueiro.MotoqueiroId.ToString())
                    }
                );

                DateTime dataCriacao = DateTime.Now;
                DateTime dataExpiracao = dataCriacao +
                                         TimeSpan.FromSeconds(tokenConfigurations.Seconds);

                var handler = new JwtSecurityTokenHandler();
                var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                {
                    Issuer = tokenConfigurations.Issuer,
                    Audience = tokenConfigurations.Audience,
                    SigningCredentials = signingConfigurations.SigningCredentials,
                    Subject = identity,
                    NotBefore = dataCriacao,
                    Expires = dataExpiracao
                });
                var token = handler.WriteToken(securityToken);

                if (motoqueiro.PlayerId != motoqueiroBase.PlayerId)
                {
                    motoqueiroBase.PlayerId = motoqueiro.PlayerId;
                    _motoqueiroDao.Alterar(motoqueiroBase);

                }

                return new
                {
                    authenticated = true,
                    created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                    expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
                    accessToken = token,
                    message = "OK",
                    usuario = motoqueiroBase
                };
            }
            else
            {
                return new
                {
                    authenticated = false,
                    message = "Falha ao autenticar"
                };
            }
        }

        [Authorize("Bearer")]
        [HttpPost("Adicionar")]
        public object Adicionar([FromBody]Motoqueiro motoqueiro)
        {
            _motoqueiroDao.Alterar(motoqueiro);

            return new RootResult()
            {
                TotalPage = 1,
                Results = new List<object>()
            };
        }

        //[Authorize("Bearer")]
        [HttpPost("Alterar")]
        public object Alterar([FromBody]Motoqueiro motoqueiro)
        {
            _motoqueiroDao.Alterar(motoqueiro);

            return new RootResult()
            {
                TotalPage = 1,
                Results = new List<object>()
            };
        }


        //[Authorize("Bearer")]
        [HttpGet("EntregasPendentes")]
        public object EntregasPendentes()
        {
            var motoqueiro = User.Identity.Name;

            return new RootResult()
            {
                TotalPage = 1,
                Results = new List<object>()
            };
        }

        //[Authorize("Bearer")]
        [HttpGet("Disponiveis")]
        public object Disponiveis()
        {
            return new RootResult()
            {
                TotalPage = 1,
                Results = new List<object>()
            };
        }

        [HttpGet("todas/{limit}/{page}")]
        public RootResult ObterCategoriasByRestauranteId(int limit, int page)
        {
            var data = _motoqueiroDao.ObterTodos();
            var totalPage = (int)Math.Ceiling((double)data.Count / limit);
            return new RootResult()
            {
                TotalPage = totalPage,
                Results = data.Skip((page - 1) * limit).Take(limit).ToList<object>()
            };
        }
    }
}