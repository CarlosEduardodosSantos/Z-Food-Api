namespace APIAlturas.ViewModels
{
    public enum PedidoStatusParceiroEnumView
    {
        //Indica um pedido foi colocado no sistema.
        PLACED = 1,
        //Indica um pedido confirmado.
        confirmation = 2,
        //Indica um pedido que foi recebido pelo e-PDV.
        integration = 3,
        //Indica um pedido que foi cancelado.
        cancelled = 4,
        //Indica um pedido que foi despachado ao cliente.
        dispatch = 5,
        //Indica um pedido que foi entregue.
        delivery = 6,
        //Indica um pedido que foi concluído (Em até duas horas do fluxo normal)*
        CONCLUDED = 7,
        //Pedido pronto para retirada
        readyToDeliver = 8
    }
}