using System;

namespace APIAlturas.ViewModels
{
    public class ProdutosOpcaoTipoRelacao
    {
        public Guid Id { get; set; }
        public Guid ProdutosOpcaoId { get; set; }
        public int ProdutoId { get; set; }
    }
}