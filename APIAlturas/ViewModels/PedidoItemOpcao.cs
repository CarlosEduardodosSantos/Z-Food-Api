using System;

namespace APIAlturas.ViewModels
{
    public class PedidoItemOpcao
    {
        public Guid PedidoItemOpcaoId { get; set; }
        public Guid PedidoItemGuid { get; set; }
        public Guid PedidoGuid { get; set; }
        public int Tipo { get; set; }
        public string Nome { get; set; }
        public int Quantidade { get; set; }
        public decimal Valor { get; set; }
        public string ProdutoPdv { get; set; }
    }
}