using System;
using System.Text;
using APIAlturas.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Uol.PagSeguro.Constants;
using Uol.PagSeguro.Domain;
using Uol.PagSeguro.Domain.Direct;
using Uol.PagSeguro.Exception;
using Uol.PagSeguro.Resources;
using Uol.PagSeguro.Service;
using RestSharp;
using Newtonsoft.Json.Linq;

//using Uol.PagSeguro.Domain;

namespace APIAlturas.Service.Payment
{
    public class PagSeguroService : IPaymentBase
    {
        private bool _isSandbox;
        private readonly AccountCredentials _credentials;
        public PagSeguroService(PaymentAutenticacao paymentAutenticacao, IHostingEnvironment hostingEnv)
        {
            _isSandbox = paymentAutenticacao.Sandbox;

            var path = hostingEnv.WebRootPath;
            

            PagSeguroConfiguration.UrlXmlConfiguration = $"{path}\\Configuration\\PagSeguroConfig.xml";
            EnvironmentConfiguration.ChangeEnvironment(_isSandbox);

            _credentials = PagSeguroConfiguration.Credentials(_isSandbox);
            _credentials.Email = paymentAutenticacao.usuario;
            _credentials.Token = paymentAutenticacao.Token;

        }

        public PaymentResult Consulta(PaymentResult paymentResult)
        {
           // EnvironmentConfiguration.ChangeEnvironment(_isSandbox);

           // var credentials = PagSeguroConfiguration.Credentials(_isSandbox);


            // Realizando uma consulta de transação a partir do código identificador 
            // para obter o objeto Transaction
            var transaction = TransactionSearchService.SearchByCode(_credentials, paymentResult.CodigoAutorizacao);

            return ProcassarResultado(transaction);
        }

        public PaymentResult Cancelamento(PaymentResult paymentResult)
        {
            throw new NotImplementedException();
        }

        public string GetSession()
        {
            try
            {
                var result = SessionService.CreateSession(_credentials);
                return result.id;
            }
            catch (Exception ex)
            {
                return "";
            }

        }


        public PaymentResult Pagar(PaymentModelView pagamento)
        {
            try
            {

                var checkout = new CreditCardCheckout();

                // Sets the payment mode
                checkout.PaymentMode = PaymentMode.DEFAULT;
                checkout.PaymentMethod = "CREDIT_CARD";

                // Sets the receiver e-mail should will get paid
                checkout.ReceiverEmail = _credentials.Email;

                // Sets the currency
                checkout.Currency = Currency.Brl;

                checkout.Items.Add(new Item("001","ZFood Delivery", 1, pagamento.total));
                // Add items

                //checkout.Items.Add(new Item("0002", "Notebook Rosa", 2, 150.99m));

                // Sets a reference code for this checkout, it is useful to identify this payment in future notifications.
                checkout.Reference = "REF1234" + new Random().Next(200, 10000);

                // Sets shipping information.
                checkout.Shipping = new Shipping();
                checkout.Shipping.ShippingType = ShippingType.NotSpecified;
                checkout.Shipping.Cost = 0;
                checkout.Shipping.Address = new Address(
                    "BRA",
                    "SP",
                    "Ribeirão Preto",
                    "Jardim Paulistanoa",
                    "14000000",
                    "Rua Lidio de Oliveira Valada",
                    "465",
                    ""
                );

                // Sets shipping information.
                checkout.Billing = new Billing();
                checkout.Billing.Address = new Address(
                    "BRA",
                    "SP",
                    "Ribeirão Preto",
                    "Jardim Paulistano",
                    "14000000",
                    "Rua Lidio de Oliveira Valada",
                    "465",
                    ""
                );

                var foneNumeros = Sonumeros(pagamento.user.Fone);
                var fone = foneNumeros.Length == 11
                    ? foneNumeros.Substring(2, foneNumeros.Length - 2) : foneNumeros.Substring(0, 8);

                // Sets credit card holder information.
                checkout.Holder = new Holder(
                    pagamento.Titular,
                    new Phone("16", fone),
                    new HolderDocument(Documents.GetDocumentByType("CPF"), pagamento.Cpf),
                    "01/01/1980"
                );

                // Sets your customer information.
                // If you using SANDBOX you must use an email @sandbox.pagseguro.com.br
                
                var emailSender = _isSandbox ? "comprador@sandbox.pagseguro.com.br" : pagamento.user.Email;
                checkout.Sender = new Sender(
                    pagamento.Titular,
                    //pagamento.user.Email,
                    emailSender,
                    new Phone("16", fone)
                );
                
                checkout.Sender.Hash = pagamento.hash;
                SenderDocument senderCPF = new SenderDocument(Documents.GetDocumentByType("CPF"), pagamento.Cpf);
                checkout.Sender.Documents.Add(senderCPF);

                //Create Credit Card Token
                var url = "https://df.uol.com.br/v2/cards/?email="+_credentials.Email+"&token="+_credentials.Token;
                var client = new RestClient(url);
                var request = new RestRequest(Method.POST);
                request.AddHeader("Accept", "application/xml");
                request.AddHeader("Content-Type", "application/json");
                JObject jObjectbody = new JObject();
                jObjectbody.Add("sessionId", GetSession());
                jObjectbody.Add("amount", pagamento.total );
                jObjectbody.Add("cardNumber", pagamento.Cartao.num);
                jObjectbody.Add("cardBrand", pagamento.Cartao.brand);
                jObjectbody.Add("cardCvv", pagamento.Cartao.cvv);
                jObjectbody.Add("cardExpirationMonth", pagamento.Cartao.monthExp);
                jObjectbody.Add("cardExpirationYear", pagamento.Cartao.yearExp);
                request.AddParameter("application/json", jObjectbody,ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                // Sets a credit card token.
                checkout.Token = response.Content.ToString();

                //Sets the installments information
                checkout.Installment = new Installment(1, pagamento.total);

                // Sets the notification url
                checkout.NotificationURL = "http://www.zipsoftware.com.br";
                var result = TransactionService.CreateCheckout(_credentials, checkout);
                return ProcassarResultado(result);


            }
            catch (PagSeguroServiceException exception)
            {
                var errors = new StringBuilder();
                foreach (var exceptionError in exception.Errors)
                {
                    errors.AppendLine(exceptionError.Message);
                }

                throw new Exception(errors.ToString());

            }
        }

        private PaymentResult ProcassarResultado(Transaction transactionResponse)
        {
            return new PaymentResult()
            {
                PaymentResultId = Guid.NewGuid(),
                CodigoAutorizacao = transactionResponse.Code,
                DataHora = transactionResponse.Date,
                ReferenciaId = transactionResponse.Reference,
                Status = transactionResponse.TransactionStatus.ToString(),
                Menssage = MessageByCode(transactionResponse.TransactionStatus),
                Autorizado = transactionResponse.TransactionStatus == 3,
                Pendente = transactionResponse.TransactionStatus <= 2
            };
        }

        private string MessageByCode(int code)
        {
            var message = string.Empty;
            switch (code)
            {
                case 1:
                    message = "Aguardando pagamento";
                    break;
                case 2:
                    message = "Pagamento Em análise.";
                    break;
                case 3:
                    message = "Pagamento Autorizado";
                    break;
                case 4:
                    message = "Pagamento Disponível";
                    break;
                case 5:
                    message = " Pagamento Em disputa";
                    break;
                case 6:
                    message = "Pagamento Devolvido";
                    break;
                case 7:
                    message = "Pagamento cancelado pela sua operadora! Este valor não será cobrado em seu cartão";
                    break;
               

            }

            return message;
        }

        private string Sonumeros(string valor)
        {
            return String.Join("", System.Text.RegularExpressions.Regex.Split(valor, @"[^\d]"));
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

    }
}