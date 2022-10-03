using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using APIAlturas.Helper;
using APIAlturas.Service;
using APIAlturas.ViewModels;
using Microsoft.IdentityModel.Tokens;

namespace APIAlturas.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        [AllowAnonymous]
        [HttpPost]
        public object Post(
            [FromBody]User usuario,
            [FromServices]UsersDAO usersDAO,
            [FromServices]SigningConfigurations signingConfigurations,
            [FromServices]TokenConfigurations tokenConfigurations)
        {
            var usuarioBase = new User();
            bool credenciaisValidas = false;
            if (usuario != null && !String.IsNullOrWhiteSpace(usuario.Email))
            {
                usuarioBase = usersDAO.Find(usuario.Email, usuario.RestauranteId);
                credenciaisValidas = (usuarioBase != null &&
                    usuario.Email.ToLower() == usuarioBase.Email.ToLower() &&
                    usuario.Senha.ToLower() == usuarioBase.Senha.ToLower());
            }

            if (credenciaisValidas)
            {
                ClaimsIdentity identity = new ClaimsIdentity(
                    new GenericIdentity(usuario.UserID.ToString(), "Login"),
                    new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, usuario.UserID.ToString())
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

                if (usuario.PlayersId != usuarioBase.PlayersId || usuario.RestauranteId == 0)
                {
                    usuarioBase.PlayersId = usuario.PlayersId;
                    usersDAO.Alterar(usuarioBase);

                }

                return new
                {
                    authenticated = true,
                    created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                    expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
                    accessToken = token,
                    message = "OK",
                    usuario =  usuarioBase
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


        [AllowAnonymous]
        [HttpPost("registar")]
        public object Registrar(
            [FromBody] User usuario,
            [FromServices] UsersDAO usersDAO,
            [FromServices]SigningConfigurations signingConfigurations,
            [FromServices]TokenConfigurations tokenConfigurations)
        {
            var usuarioBase = usersDAO.Find(usuario.Email, usuario.RestauranteId);
            if (usuarioBase == null)
            {
                usersDAO.Registar(usuario);
                return new
                {
                    errors = false,
                    message = "Cadastro efetuado com sucesso."
                };
            }
            else
            {
                return new
                {
                    errors = true,
                    message = "Usuário ja cadastrado."
                };
            }

        }


        [HttpPut("novaSenha")]
        public object NovaSenha(
            [FromBody] UserChangePassword userChangePassword,
            [FromServices] IEmailService emailService,
            [FromServices] UsersDAO usersDAO)
        {
            
            
            var usuarioBase = usersDAO.Find(userChangePassword.Email, userChangePassword.RestauranteId);
            if (usuarioBase != null)
            {

                if (FuncaoIteis.SoNumeros(usuarioBase.Fone) != FuncaoIteis.SoNumeros(userChangePassword.Celular))
                    return new
                    {
                        errors = true,
                        message = "Ops... o fone cadastrado para o email solicitado não é esse!"
                    };

                usuarioBase.Senha = GeraSenhaAleatoria();

                try
                {
                    var email = usuarioBase.Email;
                    var subject = "Solicitação de alteração de senha do App";
                    var mensage =
                        $"<p>Olá {usuarioBase.Nome}, sua solicitação de alteração de senha foi concluida com sucesso.</p>" +
                        $"<h3>Sua senha temporária é: {usuarioBase.Senha}</h3>";

                    emailService.SendEmailAsync(email, subject, mensage).Wait();

                    usersDAO.Alterar(usuarioBase);

                    return new
                    {
                        errors = false,
                        message = "Senha enviada com sucesso para seu mail."
                    };

                }
                catch (Exception e)
                {
                    return new
                    {
                        errors = true,
                        message = "Ocorreu um erro inesperado. tente novamente mais tarde."
                    };
                }
                
            }


            return new
            {
                errors = true,
                message = "Usuário não encontrado."
            };
        }

        [HttpPut("trocarSenha")]
        public object TrocarSenha(
            [FromBody] UserChangePassword userChangePassword,
            [FromServices] UsersDAO usersDAO)
        {

            var usuarioBase = usersDAO.GetById(userChangePassword.UserId);
            if (usuarioBase != null)
            {
                usuarioBase.Senha = userChangePassword.NewPassword;
                usersDAO.Alterar(usuarioBase);
                return new
                {
                    errors = false,
                    message = "Senha enviada com sucesso para seu mail."
                };
            }
            else
            {
                return new
                {
                    errors = true,
                    message = "Usuário não encontrado."
                };
            }
        }

        // Gera a senha conforme as definições
        private string GeraSenhaAleatoria()
        {
            // Gera uma senha com 6 caracteres entre numeros e letras
            string chars = "abcdefghjkmnpqrstuvwxyz023456789";
            string _senha = "";
            Random random = new Random();
            for (int f = 0; f < 6; f++)
            {
                _senha = _senha + chars.Substring(random.Next(0, chars.Length - 1), 1);
            }
            return _senha;

        }
    }



}