using System;

namespace APIAlturas.ViewModels
{
    public class Bairro
    {
        public Guid BairroId { get; set; }
        public Guid CidadeId { get; set; }
        public string estado { get; set; }
        public string cidade { get; set; }
        public string bairro { get; set; }
        public string FaixaInicial { get; set; }
        public string FaixaFinal { get; set; }
    }
}