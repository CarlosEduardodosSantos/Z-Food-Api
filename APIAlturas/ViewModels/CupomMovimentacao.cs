using System;

namespace APIAlturas.ViewModels
{
    public class CupomMovimentacao
    {
        public Guid CupomMovimentacaoId { get; set; }
        public Guid CupomId { get; set; }
        public Guid UserId { get; set; }
        public Guid PedidoId { get; set; }
        public DateTime DataHora { get; set; }
        public decimal Valor { get; set; }
        public string Cliente { get; set; }
    }
}