using System;

namespace APIAlturas.ViewModels
{
    public class PedidoMesa
    {
        public PedidoMesa()
        {
            PedidoMesaId = Guid.NewGuid();
            DataHora = DateTime.Now;
            Situacao = 1;
        }
        public Guid PedidoMesaId { get; set; }
        public string MesaId { get; set; }
        public int RestauranteId { get; set; }
        public DateTime DataHora { get; set; }
        public int Situacao { get; set; } //1 = Aberta  2 = Fechada
    }
}