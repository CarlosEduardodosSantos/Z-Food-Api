using System;
using System.Linq;
using System.Text.RegularExpressions;
using APIAlturas.ViewModels;
using CreditCardValidator;

namespace APIAlturas.Service.Payment
{
    public class CardBrandService
    {
        public enum CardType
        {
            MasterCard,
            Visa,
            AmericanExpress,
            Amex,
            BCGlobal,
            CarteBlanch,
            DinersClub,
            InstaPaymentCard,
            JCBCard,
            KoreanLocal,
            LaserCard,
            Maestro,
            SwitchCard,
            UnionPay,
            Discover,
            Solo,
            NotFormatted,
            Unknown
        };

        public static CardBrandViewModel FindType(string cardNumber)
        {
            var cardBrand = new CardBrandViewModel();

           
            return cardBrand;

        }
    }
}