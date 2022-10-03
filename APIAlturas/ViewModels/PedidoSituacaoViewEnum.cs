using System.ComponentModel;

namespace APIAlturas.ViewModels
{
    public enum PedidoSituacaoViewEnum
    {
        [Description("Pendente")]
        Pendente = 0,
        [Description("Pedido Aguardando confirmação")]
        Confirmado = 1,
        [Description("Pedido em preparo")]
        Preparando = 2,
        [Description("Pedido integrado")]
        Integrado = 3,
        [Description("Pedido cancelado")]
        Cancelado = 4,
        [Description("Pedido saiu pra entrega")]
        Entregando = 5,
        [Description("Pedido entregue")]
        Entregue = 6,
        [Description("Pedido concluido")]
        Finalizado = 7,
        [Description("Pronto para retirar")]
        ReadyToDeliver = 8,
        [Description("Pedido com produtos não encontrados")]
        ErroProdutoNaoEncontrado = 90,
        [Description("Pedido com senha/ID de toten não encontrado")]
        ErroSenhaNaoEncontrada = 91,
        [Description("Pedido aguardando pagamento")]
        AguardandoPagamento = 92,
        [Description("Pedido com erro desconhecido - Contate o suporte")]
        ErroDesconhecido = 95,
        [Description("Pedido realizado em mesa fechada")]
        MesaFechada = 96,
        [Description("Erro da OpMesa1 - Contate o suporte")]
        ErroOpMesa1 = 97,
        [Description("Erro ao inserir item no pedido")]
        ErroInserirItem = 98,
        [Description("Erro da OpMesa2 - Contate o suporte")]
        ErroOpMesa2 = 99,
        [Description("Pedido com ID de comanda não encontrada")]
        ErroComandaNaoEncontrada = 999
    }
}