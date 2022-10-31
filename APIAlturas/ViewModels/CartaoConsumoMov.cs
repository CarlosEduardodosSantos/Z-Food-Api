using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIAlturas.ViewModels
{
    public class CartaoConsumoMov
    {
        public CartaoConsumoMov()
        {
            CartaoConsumoMovId = Guid.NewGuid();
        }
        public Guid CartaoConsumoMovId { get; set; }
        public Guid CartaoConsumoId { get; set; }

        public DateTime DataMov { get; set; }

        public string Historico { get; set; }

        public decimal Saldo { get; set; }
        public int TipoMov { get; set; }

        public int UsuarioId { get; set; }
            
        public string Login { get; set; }
        public int Metodo { get; set; }
    }
}
