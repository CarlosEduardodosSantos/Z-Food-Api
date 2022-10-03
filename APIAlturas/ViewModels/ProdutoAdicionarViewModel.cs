using System;

namespace APIAlturas.ViewModels
{
    public class ProdutoAdicionarViewModel
    {
        public int ProdutoId { get; set; }
        public int Situacao { get; set; }
        public string TokenRestaurante { get; set; }
        public int ReferenciaId { get; set; }
        public int RestauranteId { get; set; }
        public string Nome { get; set; }
        public decimal ValorVenda { get; set; }
        public decimal ValorRegular { get; set; }
        public decimal ValorPromocao { get; set; }
        public string Imagem { get; set; }
        public string ImagemBase64 { get; set; }
        public string ImagemPath { get; set; }
        public int CategoriaId { get; set; }
        public string Descricao { get; set; }
        public int NumberMeiomeio { get; set; }
        public bool IsPartMeioMeio { get; set; }
        public int TamanhoId { get; set; }
        public Guid ProdutoGuid { get; set; }
        public bool IsControlstock { get; set; }
        public int Stock { get; set; }
    }
}