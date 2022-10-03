using System;

namespace APIAlturas.ViewModels
{
    public class UserZimmer
    {
        public Guid UserZimmerId { get; set; }
        public string Nome { get; set; }
        public string Fone { get; set; }
        public string Email { get; set; }
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Cep { get; set; }
        public string RestauranteId { get; set; }
        public bool AceitaMarketing { get; set; }
    }
}