using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIAlturas.ViewModels
{
    public class Estacionamento
    {
        public int Nro { get; set; }
        public string Empresa { get; set; }
        public int Apto { get; set; }
        public string Placa { get; set; }
        public string Modelo { get; set; }
        public DateTime Entrada { get; set; }
        public DateTime Saida { get; set; }
        public decimal ValorTotal { get; set; }
        public int Metodo { get; set; }

        public DateTime DataReg = DateTime.Now;
        public bool Fechado { get; set; }
    }
}
