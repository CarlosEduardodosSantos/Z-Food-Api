namespace APIAlturas.Specification
{
    public interface ISpecification<in T>
    {
        bool IsSatisfiedBy(T entity);
    }
}