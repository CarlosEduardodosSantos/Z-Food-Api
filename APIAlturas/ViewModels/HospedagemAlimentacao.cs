using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIAlturas.ViewModels
{
    public class HospedagemAlimentacao
    {
        public Guid AlimentacaoId { get; set; }
        public Guid Nro { get; set; }
        public  DateTime Data { get; set; }
        public bool Cafe { get; set; }
        public bool Almoco { get; set; }
        public bool Janta { get; set; }
        public string Nome { get; set; }
        public int Apto { get; set; }
        public int QtdeA { get; set; }
        public int QtdeC { get; set; }
        public int QtdeN { get; set; }
        public bool CConsumido { get; set; }
        public bool AConsumido { get; set; }
        public bool JConsumido { get; set; }
    }
}
