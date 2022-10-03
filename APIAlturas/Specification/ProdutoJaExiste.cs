using System.Linq;
using APIAlturas.ViewModels;

namespace APIAlturas.Specification
{
    public class ProdutoJaExiste : ISpecification<ProdutoInsertViewModel>
    {
        private readonly ProdutoDao _produtoDao;

        public ProdutoJaExiste(ProdutoDao produtoDao)
        {
            _produtoDao = produtoDao;
        }

        public bool IsSatisfiedBy(ProdutoInsertViewModel entity)
        {
            return _produtoDao.GetByInsert(entity).Any();
        }
    }
}