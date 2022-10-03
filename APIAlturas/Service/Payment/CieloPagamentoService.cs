using System;
using APIAlturas.ViewModels;
using CieloEcommerce;


namespace APIAlturas.Service.Payment
{
    public class CieloPagamentoService : IPaymentBase
    {
        private readonly Cielo _cielo;
        public CieloPagamentoService()
        {
            string mid = "1006993069";
            string key = "25fbb99741c739dd84d7b06ec78c9bac718838630f30b112d033ce2e621b34f3";

           _cielo = new Cielo(mid, key, Cielo.TEST);
        }
        public void Dispose()
        {
            //
        }

        public PaymentResult Pagar(PaymentModelView pagamento)
        {
            Holder holder = _cielo.holder(pagamento.Cartao.num, pagamento.Cartao.yearExp, pagamento.Cartao.monthExp, pagamento.Cartao.cvv);
            holder.name = pagamento.Titular;

            Random randomOrder = new Random();

            Order order = _cielo.order(randomOrder.Next(1000, 10000).ToString(), Convert.ToInt32((pagamento.total * 100)));
            //PaymentMethod paymentMethod = _cielo.paymentMethod(PaymentMethod.VISA, PaymentMethod.CREDITO_A_VISTA);
            PaymentMethod paymentMethod = _cielo.paymentMethod(PaymentMethod.VISA, PaymentMethod.DEBITO);



            try
            {
                Transaction transaction = _cielo.transactionRequest(
                    holder,
                    order,
                    paymentMethod,
                    "http://localhost/cielo",
                    //Transaction.AuthorizationMethod.AUTHORIZE_WITHOUT_AUTHENTICATION,
                    Transaction.AuthorizationMethod.AUTHORIZE_IF_AUTHENTICATED,
                    false
                );

                return ProcassarResultado(transaction);
            }
            catch (CieloException e)
            {
                throw new Exception($"Erro codigo : [{e.Code}]: {e.Message} ");
            }
        }

        public PaymentResult Consulta(PaymentResult paymentResult)
        {
            try
            {
                var tId = paymentResult.CodigoAutorizacao;
                var transaction = _cielo.consultationRequest(tId); // tid da transação

                return ProcassarResultado(transaction);
            }
            catch (CieloException e)
            {
                throw new Exception($"Erro codigo : [{e.Code}]: {e.Message} ");
            }
        }

        public PaymentResult Cancelamento(PaymentResult paymentResult)
        {
            try
            {
                var tId = paymentResult.CodigoAutorizacao;
                var transaction = _cielo.consultationRequest(tId); // tid da transação

                transaction = _cielo.cancellationRequest(transaction);
                return ProcassarResultado(transaction);
            }
            catch (CieloException e)
            {
                throw new Exception($"Erro codigo : [{e.Code}]: {e.Message} ");
            }
        }

        public string GetSession()
        {
            throw new System.NotImplementedException();
        }

        private PaymentResult ProcassarResultado(Transaction transactionResponse)
        {

            return new PaymentResult()
            {
                PaymentResultId = Guid.NewGuid(),
                CodigoAutorizacao = transactionResponse.tid,
                DataHora = DateTime.Now,
                ReferenciaId = transactionResponse.tid,
                Status = transactionResponse.authorization.lr,
                Menssage = transactionResponse.authorization.message,
                Autorizado = transactionResponse.authorization.lr == "00",
                Nsu = transactionResponse.tid,
                Pendente = transactionResponse.authorization == null
            };
        }

    }
}