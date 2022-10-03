using System;

namespace APIAlturas.ViewModels
{
    public class FormaPagamento
    {
        public Guid FormaPagamentoId { get; set; }
        public int Situacao { get; set; }
        public Guid RestauranteToken { get; set; }
        public string Descricao { get; set; }
        public bool IsOnline { get; set; }
        public bool IsTroco { get; set; }
        public int TipoCartao { get; set; }
        public int Sequencia { get; set; }
        public decimal Percentual { get; set; }
        public string Imagem { get; set; }
    }
}