using System;

namespace APIAlturas.ViewModels
{
    public class Cidade
    {
        public Guid CidadeId { get; set; }
        public Guid EstadoId { get; set; }
        public string cidade { get; set; }
        public string estado { get; set; }
        public string FaixaInicial { get; set; }
        public string FaixaFinal { get; set; }
    }
}