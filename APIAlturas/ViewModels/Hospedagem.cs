using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIAlturas.ViewModels
{
    public class Hospedagem
    {
        public Guid Nro { get; set; }
        public string Nome { get; set; }
        public int Apto { get; set; }
        public string NomeHotel { get; set; }
        public DateTime DataCheckin { get; set; }
        public DateTime DataCheckout { get; set; }
        public int QtdeA { get; set; }
        public int QtdeC { get; set; }
        public int QtdeN { get; set; }
        public bool Cafe { get; set; }
        public bool Almoco { get; set; }
        public bool Janta { get; set; }
    }
}
