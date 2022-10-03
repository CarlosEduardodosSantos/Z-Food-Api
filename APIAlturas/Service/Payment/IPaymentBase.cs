using System;
using APIAlturas.ViewModels;

namespace APIAlturas.Service.Payment
{
    public interface IPaymentBase : IDisposable
    {
        PaymentResult Pagar(PaymentModelView pagamento);
        PaymentResult Consulta(PaymentResult paymentResult);
        PaymentResult Cancelamento(PaymentResult paymentResult);
        string GetSession();
    }
}