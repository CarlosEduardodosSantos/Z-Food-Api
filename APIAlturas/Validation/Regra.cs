using APIAlturas.Specification;

namespace APIAlturas.Validation
{
    public class Regra<TEntity> : IRegra<TEntity>
    {
        private readonly ISpecification<TEntity> _specificationRule;
        public string MensagemErro { get; private set; }

        public Regra(ISpecification<TEntity> rule, string mensagemErro)
        {
            this._specificationRule = rule;
            this.MensagemErro = mensagemErro;
        }

        public bool Validar(TEntity entity)
        {
            return this._specificationRule.IsSatisfiedBy(entity);
        }
    }
}