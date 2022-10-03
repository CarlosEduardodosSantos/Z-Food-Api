using System;

namespace APIAlturas.ViewModels
{
    public class ZcashMovimentacao
    {
        public Guid ZCashMovimentacaoId { get; set; }
        public Guid PedidoGuid { get; set; }
        public int RestauranteId { get; set; }
        public Guid UsuarioId { get; set; }
        public int TipoOperacao { get; set; }
        public DateTime DataHora { get; set; }
        public decimal ValorPedido { get; set; }
    }
}