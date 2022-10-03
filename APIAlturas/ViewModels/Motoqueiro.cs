using System;

namespace APIAlturas.ViewModels
{
    public class Motoqueiro
    {
        public Guid MotoqueiroId { get; set; }
        public int Situacao { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Celular { get; set; }
        public string ContaBanco { get; set; }
        public string ContaAgencia { get; set; }
        public string ContaNumero { get; set; }
        public string Cpf { get; set; }
        public string PlayerId { get; set; }
        public string Imagem { get; set; }
    }
}