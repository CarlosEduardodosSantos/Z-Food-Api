using System.ComponentModel;

namespace APIAlturas.ViewModels
{
    public enum OperacaoEnumView
    {
        [Description("Pedido pele App")]
        PedidoApp = 1,
        [Description("Pedido pele parceiro")]
        PedidoParceiro = 2,
        [Description("Resgate pelo App")]
        ResgateApp = 3,
        [Description("Resgate pelo Parceiro")]
        ResgateParceiro = 4,
        [Description("Pedido Cancelamento")]
        PedidoCancelamento = 5,
        [Description("Outros")]
        Outros = 99,
    }
}