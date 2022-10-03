using System;

namespace APIAlturas.ViewModels
{
    public class Cupom
    {
        public Guid CupomId { get; set; }
        public int RestauranteId { get; set; }
        public int Situacao { get; set; }
        public int Tipo { get; set; }
        public int Quantidade { get; set; }
        public int QuantidadeUso { get; set; }
        public decimal Valor { get; set; }
        public decimal Percentual { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal ValorMinimo { get; set; }
        public DateTime DataHora { get; set; }
        public DateTime Validade { get; set; }
        public Guid? UserId { get; set; }
        public string Codigo { get; set; }
        public bool NoLImited { get; set; }
    }
}