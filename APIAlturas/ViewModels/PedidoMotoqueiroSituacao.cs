using System;

namespace APIAlturas.ViewModels
{
    public class PedidoMotoqueiroSituacao
    {
        public Guid PedidoMotoqueiroSituacaoId { get; set; }
        public Guid PedidoMotoqueiroId { get; set; }
        public PedidoMotoqueiroSituacaoEnumView Situacao { get; set; }
        public DateTime DataHora { get; set; }
        public Guid MotoqueiroId { get; set; }
    }
}