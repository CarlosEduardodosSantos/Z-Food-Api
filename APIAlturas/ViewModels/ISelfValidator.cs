namespace APIAlturas.ViewModels
{
    public interface ISelfValidator
    {
        ValidationResult ResultadoValidacao { get; }
        bool IsValid();
    }
}