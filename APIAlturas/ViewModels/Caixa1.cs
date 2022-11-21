using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIAlturas.ViewModels
{
    public class Caixa1
    {
        public int Id { get; set; }
        public string Nro { get; set; }
        public string Historico { get; set; }
        public string Login { get; set; }
        public DateTime Data { get; set; }
        public  DateTime ? DataFechamento { get; set; }
        public bool Fechado { get; set; }
        public decimal Valor { get; set; }
        public int Metodo { get; set; }
        public DateTime Dia { get; set; }
        public Guid MovId { get; set; }
        public int RestauranteId { get; set; }
    }
}
