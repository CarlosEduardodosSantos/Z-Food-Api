using APIAlturas.ViewModels;

namespace APIAlturas.Validation
{
    public interface IFiscal<in TEntity>
    {
        ValidationResult Validar(TEntity entity);
    }
}