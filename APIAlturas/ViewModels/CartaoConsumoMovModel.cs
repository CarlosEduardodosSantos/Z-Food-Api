using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIAlturas.ViewModels
{
    public class CartaoConsumoMovModel
    {
        public int RestauranteId { get; set; }
        public string NumeroCartao { get; set; }
        public DateTime DataMov { get; set; }
        public decimal Valor { get; set; }
        public int TipoMov { get; set; }
        public string Historico { get; set; }

        public int UsuarioId { get; set; }
        public string Login { get; set; }
        public bool Frete { get; set; }
    }
}
