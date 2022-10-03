using System;
using System.Collections.Generic;

namespace APIAlturas.ViewModels
{
    public class CartTotal
    {
        public double SubTotal { get; set; }
        public decimal TaxTotal { get; set; }
        public decimal TotalTax { get; set; }
        public decimal DiscountTotal { get; set; }
        public decimal ShippingTotal { get; set; }
        public decimal Total { get; set; }
        public decimal TotalOnline { get; set; }
    }

    public class CartComplemento
    {
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        
    }
    public class CartItemOpcao
    {
        public int Tipo { get; set; }
        public string Nome { get; set; }
        public int Quantidade { get; set; }
        public decimal Valor { get; set; }
        public string ProdutoPdv { get; set; }

    }
    public class CartMeioMeio
    {
        public string MeioMeioId { get; set; }
        public string ProdutoId { get; set; }
        public Guid PedidoGuid { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public Guid MeioMeioGuid { get; set; }
        public List<CartComplementosMeioMeio> CartComplementosMeioMeios { get; set; }

    }

    public class CartComplementosMeioMeio
    {
        public int PedidoMeioMeioComplementoID { get; set; }
        public Guid MeioMeioGuid { get; set; }
        public string Complemento { get; set; }
        public decimal Valor { get; set; }
        public int Quantidade { get; set; }
        public Guid PedidoGuid { get; set; }

    }
    public class CartItem
    {
        public Guid PedidoItemGuid { get; set; }
        public Produto Produto { get; set; }
        public decimal Quantidade { get; set; }
        public string Observacao { get; set; }
        public decimal Valorentrega { get; set; }
        public decimal Valorprodutos { get; set; }
        public decimal Valortotal { get; set; }
        public List<CartComplemento> CartComplementos { get; set; }
        public List<CartMeioMeio> CartMeioMeios { get; set; }
        public List<CartItemOpcao> CartItemOpcoes { get; set; }
    }

    public class CartContent
    {
        public Restaurante Restaurante { get; set; }
        public ICollection<CartItem> CartItens { get; set; }
        public decimal Subtotal { get; set; }
        public decimal DescontoTotal { get; set; }
        public decimal EntregaTotal { get; set; }
        public decimal Total { get; set; }
        public decimal Troco { get; set; }
        public string Observacao { get; set; }
        public string FormaPagamento { get; set; }
        
    }

    public class Adress
    {
        public string cep { get; set; }
        public string logradouro { get; set; }
        public string complemento { get; set; }
        public string bairro { get; set; }
        public string localidade { get; set; }
        public string uf { get; set; }
        public string unidade { get; set; }
        public string ibge { get; set; }
        public string gia { get; set; }
        public string numero { get; set; }
    }

    public class Carrinho
    {
        public Guid CarrinhoIdGuid { get; set; }
        public Guid UsuarioGuid { get; set; }
        public CartTotal CartTotal { get; set; }
        public List<CartItem> CartItens { get; set; }
        public bool AppliedCoupons { get; set; }
        public Cupom Cupom { get; set; }
        public string Coupom { get; set; }
        public int Count { get; set; }
        public string Cpf { get; set; }
        public bool IsEntrega { get; set; }
        public Adress Adress { get; set; }
        public Restaurante Restaurante { get; set; }
        public decimal Troco { get; set; }
        public string Observacao { get; set; }
        public string FormaPagamento { get; set; }
        public bool IsPagamentoOnline { get; set; }
        public string PaymentNsu { get; set; }
        public int RepresentanteId { get; set; }
        public string Referencia { get; set; }
        public UserZimmer UserZimmer { get; set; }
        public string OrderType { get; set; }
        public string TotenId { get; set; }
        public string ComandaId { get; set; }
    }
}