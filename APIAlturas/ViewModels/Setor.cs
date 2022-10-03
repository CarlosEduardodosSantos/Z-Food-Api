using System;

namespace APIAlturas.ViewModels
{
    public class Setor
    {
        public Guid SetorId { get; set; }
        public string Descricao { get; set; }
        public string Imagem { get; set; }
        public int Situacao { get; set; }
    }
}