using System;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Microsoft.IdentityModel.Tokens;

namespace APIAlturas
{
    public class User
    {
        public Guid UserID { get; set; }
        public string AccessKey { get; set; }
        public string Email { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Fone { get; set; }
        public string Senha { get; set; }
        public string PlayersId { get; set; }
        public string Imagem { get; set; }
        public string Facebook { get; set; }
        public string Cpf { get; set; }
        public int RestauranteId { get; set; }
        public bool IsValid => ValidaUsuario();
        public decimal ZCash { get; set; }

        private bool ValidaUsuario()
        {
            if (string.IsNullOrEmpty(Email)) return false;

            var emailValid = new Regex(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$");
            return emailValid.IsMatch(Email);
        }
    }

    public class TokenConfigurations
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int Seconds { get; set; }
    }

    public class SigningConfigurations
    {
        public SecurityKey Key { get; }
        public SigningCredentials SigningCredentials { get; }

        public SigningConfigurations()
        {
            using (var provider = new RSACryptoServiceProvider(2048))
            {
                Key = new RsaSecurityKey(provider.ExportParameters(true));
            }

            SigningCredentials = new SigningCredentials(
                Key, SecurityAlgorithms.RsaSha256Signature);
        }
    }
}