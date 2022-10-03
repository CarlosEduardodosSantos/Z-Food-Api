namespace APIAlturas.Validation
{
    public interface IRegra<in TEntity>
    {
        string MensagemErro { get; }
        bool Validar(TEntity entity);
    }
}