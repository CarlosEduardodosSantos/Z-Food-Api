using System;

namespace APIAlturas.ViewModels
{
    public class GrupoCategoriaRelacao
    {
        public Guid GrupoCategoriaId { get; set; }
        public int CategoriaId { get; set; }
        public Guid GrupoId { get; set; }
    }
}