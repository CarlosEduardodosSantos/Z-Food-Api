using System;
using System.Collections.Generic;

namespace APIAlturas.ViewModels
{
    public class GrupoProduto
    {
        public Guid GupoId { get; set; }
        public int RestauranteId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Imagem { get; set; }
        public string ImagemBase64 { get; set; }
        public string ImagemZimmer { get; set; }
        public int Sequencia { get; set; }
        public int Situacao { get; set; }
        public List<Categoria> Categorias { get; set; }
        public List<Produto> Produtos { get; set; }
    }
}