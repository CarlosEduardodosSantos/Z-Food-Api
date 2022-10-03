using APIAlturas.Specification;
using APIAlturas.ViewModels;

namespace APIAlturas.Validation
{
    public class ValidaProduto : FiscalBase<ProdutoInsertViewModel>
    {
        private readonly ProdutoDao _produtoDao;
        public ValidaProduto()
        {
            //_produtoDao = ServiceLocator.Current.GetInstance<ProdutoDao>();
            base.AdicionarRegra(new Regra<ProdutoInsertViewModel>(new ProdutoJaExiste(_produtoDao), "Produto ja existente em seu cadastro"));
        }
    }
}