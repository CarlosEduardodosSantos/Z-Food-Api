using System;

namespace APIAlturas.ViewModels
{
    public class PedidoMotoqueiro
    {
        public Guid PedidoMotoqueiroId { get; set; }
        public PedidoMotoqueiroSituacaoEnumView Situacao { get; set; }
        public DateTime DataHora { get; set; }
        public Guid PedidoId { get; set; }
        public int RestauranteId { get; set; }
        public Guid MotoqueiroId { get; set; }
        public int TempoPreparo { get; set; }
    }
}