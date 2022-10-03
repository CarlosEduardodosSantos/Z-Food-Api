using System;

namespace APIAlturas.ViewModels
{
    public class Cartao
    {
        public Guid CartaoId { get; set; }
        public Guid UserId { get; set; }
        public string num { get; set; }
        public string cvv { get; set; }
        public string monthExp { get; set; }
        public string yearExp { get; set; }
        public string titular { get; set; }
        public string cpf { get; set; }
        public string image { get; set; }
        public string brand { get; set; }
    }
}