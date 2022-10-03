using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIAlturas.ViewModels
{
    public class HospedagemRelatorio
    {
        public DateTime Data { get; set; }
        public int Cafe { get; set; }
        public int Almoco { get; set; }
        public int Janta { get; set; }
    }
}
