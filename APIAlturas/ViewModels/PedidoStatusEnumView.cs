namespace APIAlturas.ViewModels
{
    public enum PedidoStatusEnumView
    {
        INCLUD = 0,
        //Indica um pedido foi colocado no sistema.
        PLACED = 1,
        //Indica um pedido confirmado.
        CONFIRMED = 2,
        //Indica um pedido que foi recebido pelo e-PDV.
        INTEGRATED = 3,
        //Indica um pedido que foi cancelado.
        CANCELLED = 4,
        //Indica um pedido que foi despachado ao cliente.
        DISPATCHED = 5,
        //Indica um pedido que foi entregue.
        DELIVERED = 6,
        //Indica um pedido que foi concluído (Em até duas horas do fluxo normal)*
        CONCLUDED = 7,
        //Pedido pronto para retirada
        READYTODELIVER = 8
    }
}