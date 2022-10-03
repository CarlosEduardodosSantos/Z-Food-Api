using System.Collections.Generic;

namespace APIAlturas.ViewModels
{
    public class PaymentModelView
    {
        public User user { get; set; }
        public string restauranteId { get; set; }
        public string token { get; set; }
        public string hash { get; set; }
        public string method { get; set; }
        public decimal total { get; set; }
        public decimal entrega { get; set; }
        public string Titular { get; set; }
        public string Cpf { get; set; }
        public Cartao Cartao { get; set; }
        public string transactionCode { get; set; }
    }
}