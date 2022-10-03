using System;
using APIAlturas.Validation;

namespace APIAlturas.ViewModels
{
    public class ProdutoInsertViewModel : ISelfValidator
    {
        public int ProdutoId { get; set; }
        public int ReferenciaId { get; set; }
        public int RestauranteId { get; set; }
        public Guid Token { get; set; }
        public string Nome { get; set; }
        public int CategoriaId { get; set; }
        public decimal ValorVenda { get; set; }
        public decimal ValorRegular { get; set; }
        public decimal ValorPromocao { get; set; }
        public string Imagem { get; set; }
        public string Descricao { get; set; }
        public int NumberMeioMeio { get; set; }
        public bool IsPartMeioMeio { get; set; }
        public bool IsControlstock { get; set; }
        public int Stock { get; set; }
        public ValidationResult ResultadoValidacao { get; private set; }

        public bool IsValid()
        {
            var fiscal = new ValidaProduto();

            ResultadoValidacao = fiscal.Validar(this);

            return ResultadoValidacao.IsValid;
        }
    }
}