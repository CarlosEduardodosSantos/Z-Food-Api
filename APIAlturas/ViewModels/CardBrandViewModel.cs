using CreditCardValidator;

namespace APIAlturas.ViewModels
{
    public class CardBrandViewModel
    {
        public CardBrandViewModel()
        {
            IsValid = false;
            Brand = CardIssuer.Unknown;
        }
        public string CardNumber { get; set; }
        public CardIssuer Brand { get; set; }
        public string BrandName { get; set; }
        public bool IsValid { get; set; }
        public string Image { get; set; }
    }
}