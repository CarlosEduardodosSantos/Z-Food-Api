using System;
using System.Collections.Generic;

namespace APIAlturas.ViewModels
{
    public class Produto
    {
        public int ProdutoId { get; set; }
        public Guid ProdutoGuid { get; set; }
        public int Situacao { get; set; }
        public string TokenRestaurante { get; set; }
        public int ReferenciaId { get; set; }
        public int RestauranteId { get; set; }
        public string Nome { get; set; }
        public decimal ValorVenda { get; set; }
        public decimal ValorRegular { get; set; }
        public decimal ValorPromocao { get; set; }
        private string _imagem;
        public string Imagem {
            get => _imagem.IndexOf("http") != -1 ? _imagem : "https://api.zclub.com.br/api/" + _imagem;
            set => _imagem = value;
        }
        public float AvaliacaoRating { get; set; }
        public int CategoriaId { get; set; }
        public string categoriaNome { get; set; }
        public string Descricao { get; set; }
        public int NumberMeiomeio { get; set; }
        public bool IsPartMeioMeio { get; set; }
        public int TamanhoId { get; set; }
        public int Sequencia { get; set; }
        public bool IsControlstock { get; set; }
        public int Stock { get; set; }
        public int optionsCount { get; set; }
        
    }

    public class ProdutosGrupo
    {
        public ProdutosGrupo()
        {
            categoria = new Categoria();
            complemento = new List<Complemento>();
            produtos = new List<Produto>();
        }
        public Categoria categoria { get; set; }
        public ICollection<Complemento> complemento { get; set; }
        public ICollection<Produto> produtos { get; set; }
    }

    public class Categoria
    {
        public int CategoriaId { get; set; }
        public int ReferenciaId { get; set; }
        public int RestauranteId { get; set; }
        public Guid RestauranteToken { get; set; }
        public int Situacao { get; set; }
        public string Descricao { get; set; }
        public int Sequencia { get; set; }
        public IEnumerable<Produto> produtos { get; set; }
    }

    public class Complemento
    {
        public int ComplementoId { get; set; }
        public Guid TokenRestaurante { get; set; }
        public int ReferenciaId { get; set; }
        public int RestauranteId { get; set; }
        public string Descricao { get; set; }
        public int CategoriaId { get; set; }
        public decimal Valor { get; set; }
    }
}