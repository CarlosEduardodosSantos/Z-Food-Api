using System;

namespace APIAlturas.ViewModels
{
    public class PaymentAutenticacao
    {
        public Guid PaymentAutenticacaoId { get; set; }
        public int RetauranteId { get; set; }
        public string usuario { get; set; }
        public string Senha { get; set; }
        public string Token { get; set; }
        public bool Sandbox { get; set; }
        public OperadoraPagamento Operadora { get; set; }
        public bool Padrao { get; set; }
    }

    public enum OperadoraPagamento
    {
        Rede = 1,
        PagSeguro = 2,
        Cielo = 3
    }
}