using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace APIAlturas.ViewModels
{
    public class Pedido
    {
        public Guid PedidoGuid { get; set; }
        public int PedidoId { get; set; }
        public Guid UsuarioGuid { get; set; }
        public User User { get; set; }
        public DateTime DataHora { get; set; }
        public int RestauranteId { get; set; }
        public Restaurante Restaurante { get; set; }
        public int Situacao { get; set; }
        public string DescricaoSituacao => GetDescricaoSituacao();
        public bool IsEntrega { get; set; }
        public int IndPag { get; set; }
        public string FormaPagamento { get; set; }
        public decimal Subtotal { get; set; }
        public decimal DescontoTotal { get; set; }
        public decimal EntregaTotal { get; set; }
        public decimal Total { get; set; }
        public decimal TotalOnline { get; set; }
        public decimal Troco { get; set; }
        public string Coupom { get; set; }
        public string Cpf { get; set; }
        public string Observacao { get; set; }
        public bool IsPagamentoOnline { get; set; }
        private string _orderType;
        public string OrderType
        {
            get => _orderType;
            set => _orderType = value;
        }
        public string TotenId { get; set; }
        public int RepresentanteId { get; set; }
        public string Referencia { get; set; }
        public string ComandaId { get; set; }
        public PedidoStatusEnumView PedidoStatus { get; set; }
        public PedidoEntrega PedidoEntrega { get; set; }
        public ICollection<PedidoItem> PedidoItens { get; set; }
        public ICollection<PedidoItemOpcao> PedidoItemOpcao { get; set; }


        private string GetDescricaoSituacao()
        {
            return GetEnumDescription((PedidoSituacaoViewEnum)Situacao);
        }

        private string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attributes != null && attributes.Any())
            {
                return attributes.First().Description;
            }

            return value.ToString();
        }
    }
    public class PedidoItem
    {
        public Guid PedidoItemGuid { get; set; }
        public Guid PedidoGuid { get; set; }
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }
        public string Observacao { get; set; }
        public decimal Quantidade { get; set; }
        public decimal ValorUnitario => (Valorprodutos - PedidoItemOpcoes.Sum(t => t.Valor));
        public decimal Valorprodutos { get; set; }
        public decimal Valortotal { get; set; }
        public decimal TotalSubs => PedidoItemOpcoes.Sum(t => t.Valor);
        public List<PedidoComplemento> PedidoComplementos { get; set; }
        public List<PedidoMeioMeio> PedidoMeioMeios { get; set; }
        public List<PedidoItemOpcao> PedidoItemOpcoes { get; set; }
    }
    public class PedidoComplemento
    {
        public int PedidoComplementoId { get; set; }
        public Guid PedidoItemGuid { get; set; }
        public Guid PedidoGuid { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
    }
    public class PedidoMeioMeio
    {
        public int PedidoMeioMeioId { get; set; }
        public int ProdutoId { get; set; }
        public Guid PedidoItemGuid { get; set; }
        public Guid PedidoGuid { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public Guid MeioMeioGuid { get; set; }

        public List<PedidoMeioMeioComplemento> PedidoMeioMeioComplementos { get; set; }
    }

    public class PedidoMeioMeioComplemento
    {
        public int PedidoMeioMeioComplementoID { get; set; }
        public Guid MeioMeioGuid { get; set; }
        public string Complemento { get; set; }
        public decimal Valor { get; set; }
        public int Quantidade { get; set; }
        public Guid PedidoGuid { get; set; }
    }
    public class PedidoEntrega
    {
        public Guid PedidoEntregaGuid { get; set; }
        public Guid PedidoGuid { get; set; }
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Localidade { get; set; }
        public string Uf { get; set; }
        public string Unidade { get; set; }
        public string Numero { get; set; }
    }

    
    
}