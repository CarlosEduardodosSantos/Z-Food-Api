
using System;
using APIAlturas.ViewModels;
using eRede;
using eRede.Service.Error;
using Environment = eRede.Environment;

namespace APIAlturas.Service.Payment
{
    public class RedePagamentoService : IPaymentBase
    {
        private readonly string _pdv;
        private readonly string _token;
        private readonly Environment _environment;
        public RedePagamentoService(PaymentAutenticacao paymentAutenticacao)
        {
            var sandbox = paymentAutenticacao.Sandbox;
            _pdv = paymentAutenticacao.usuario;//"10005840";
            _token = paymentAutenticacao.Token;// "20ef411cd4804911ba99a4b1c996c5ee";
            _environment = sandbox ? Environment.Sandbox() : Environment.Production();
        }

        public PaymentResult Consulta(PaymentResult paymentResult)
        {
            var store = new Store(_pdv, _token, _environment);
            try
            {
                var response = new eRede.eRede(store).get(paymentResult.CodigoAutorizacao);

                return ProcassarResultado(response);
            }
            catch (RedeException e)
            {
                throw new Exception($"Opz[{e.error.returnCode}]: {e.error.returnMessage}");
            }
        }

        public PaymentResult Cancelamento(PaymentResult paymentResult)
        {
            var store = new Store(_pdv, _token, _environment);
            var transaction = new Transaction();
            transaction.authorizationCode = paymentResult.CodigoAutorizacao;
            transaction.nsu = paymentResult.Nsu;

            try
            {
                var response = new eRede.eRede(store).cancel(transaction);

                return ProcassarResultado(response);
            }
            catch (RedeException e)
            {
                throw new Exception($"Opz[{e.error.returnCode}]: {e.error.returnMessage}");
            }
        }

        public string GetSession()
        {
            //Method não necessário para REDE
            return String.Empty;
        }


        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public PaymentResult Pagar(PaymentModelView pagamento)
        {
           
            var store = new Store(_pdv, _token, _environment);
            var transaction = new Transaction
            {
                amount = Convert.ToInt32((pagamento.total * 100)),
                reference = "pedido" + new Random().Next(200, 10000)
            };
            if (pagamento.method == "CREDIT_CARD")
            {
                transaction.CreditCard(
                    pagamento.Cartao.num,
                    pagamento.Cartao.cvv,
                    pagamento.Cartao.monthExp,
                    pagamento.Cartao.yearExp,
                    pagamento.Cartao.titular
                ).Capture(true);
            }

            else
            {
                if (pagamento.method == "DEBIT_CARD")
                    transaction.DebitCard(
                        pagamento.Cartao.num,
                        pagamento.Cartao.cvv,
                        pagamento.Cartao.monthExp,
                        pagamento.Cartao.yearExp,
                        pagamento.Cartao.titular
                    );

                transaction.AddUrl("http://example.org/success", Url.THREE_D_SECURE_SUCCESS);
                transaction.AddUrl("http://example.org/failure", Url.THREE_D_SECURE_FAILURE);
            }




            try
            {
                var response = new eRede.eRede(store).create(transaction);

                return ProcassarResultado(response);
            }
            catch (RedeException e)
            {
                //e.error.returnMessage
                throw new Exception($"Erro codigo : [{e.error.returnCode}]: {MessageByCodeErro(e.error.returnCode)} ");
            }
        }

        private PaymentResult ProcassarResultado(TransactionResponse transactionResponse)
        {
            if (transactionResponse.returnCode == "220")
            {
                Console.WriteLine("Tudo certo. Redirecione o cliente para autenticação\n{0}", transactionResponse.threeDSecure.url);
            }

            return new PaymentResult()
            {
                PaymentResultId = Guid.NewGuid(),
                CodigoAutorizacao = transactionResponse.tid,
                DataHora = DateTime.Parse(transactionResponse.dateTime),
                ReferenciaId = transactionResponse.reference,
                Status = transactionResponse.returnCode,
                Menssage = MessageByCode(transactionResponse.returnCode),
                Autorizado = transactionResponse.returnCode == "00",
                Nsu = transactionResponse.nsu,
                Pendente = false
            };
        }

        private string MessageByCode(string code)
        {
            var message = string.Empty;
            switch (code)
            {
                case "00":
                    message = "Autorizado";
                    break;
                case "58":
                    message = "Não autorizado. Contactar emissor";
                    break;
                case "101":
                    message = "Não autorizado. Problemas no cartão, entre em contato com o emissor.";
                    break;
                case "102":
                    message = "Não autorizado. Verifique a situação da loja com o emissor.";
                    break;
                case "103":
                    message = "Não autorizado. Por favor, tente novamente.";
                    break;
                case "104":
                    message = "Não autorizado. Por favor, tente novamente.";
                    break;
                case "105":
                    message = "Não autorizado. Cartao restrito.";
                    break;
                case "106":
                    message = "Erro no processamento do emissor.Por favor, tente novamente.";
                    break;
                case "107":
                    message = "Não autorizado. Por favor, tente novamente.";
                    break;
                case "108":
                    message = "Não autorizado. Valor não permitido para este tipo de cartão.";
                    break;
                case "109":
                    message = "Não autorizado. Cartão inexistente.";
                    break;
                case "110":
                    message = "Não autorizado. Tipo de transação não permitido para este cartão.";
                    break;
                case "111":
                    message = "Não autorizado. Fundos insuficientes.";
                    break;
                case "112":
                    message = "Não autorizado. A data de validade expirou.";
                    break;
                case "113":
                    message = "Não autorizado. Risco moderado identificado pelo emissor.";
                    break;
                case "114":
                    message = "Não autorizado. O cartão não pertence à rede de pagamento.";
                    break;
                case "115":
                    message = "Não autorizado. Excedeu o limite de transações permitido no período.";
                    break;
                case "116":
                    message = "Não autorizado. Entre em contato com o emissor do cartão.";
                    break;
                case "117":
                    message = "Transação não encontrada.";
                    break;
                case "118":
                    message = "Não autorizado. Cartão bloqueado.";
                    break;
                case "119":
                    message = "Não autorizado. Código de segurança inválido";
                    break;
                case "121":
                    message = "Erro no processamento. Por favor, tente novamente";
                    break;
                case "122":
                    message = "Transação enviada anteriormente";
                    break;
                case "123":
                    message = "Não autorizado. O portador solicitou o fim das recorrências no emissor.";
                    break;
                case "124":
                    message = "Não autorizado. Entre em contato com a Rede.";
                    break;
                case "170":
                    message = "Transação com dólar zero não permitida para este cartão.";
                    break;
                case "174":
                    message = "Sucesso de transação em dólar zero.";
                    break;
                case "175":
                    message = "Transação em dólar zero negada.";
                    break;

            }

            return message;
        }

        private string MessageByCodeErro(string code)
        {
            var message = string.Empty;
            switch (code)
            {
                case "1":
                    message = "Ano de validade informado incorretamente.";
                    break;
                case "2":
                    message = "Ano de validade informado incorretamente.";
                    break;
                case "3":
                    message = "Ano de validade informado incorretamente.";
                    break;
                case "4":
                    message = "Codigo de segurança informado incorretamente.";
                    break;
                case "5":
                    message = "Codigo de segurança informado incorretamente.";
                    break;
                case "12":
                    message = "Documento invalido.";
                    break;
                case "13":
                    message = "Documento invalido.";
                    break;
                case "14":
                    message = "Documento invalido.";
                    break;
                case "35":
                    message = "Mês de validade informado incorretamente.";
                    break;
                case "36":
                    message = "Numero do Cartão informado incorretamente.";
                    break;
                case "37":
                    message = "Numero do Cartão informado incorretamente.";
                    break;
                case "38":
                    message = "Numero do Cartão informado incorretamente.";
                    break;
                case "49":
                    message = "O valor da transação excede o autorizado.";
                    break;
                case "50":
                    message = "Parcelas: formato de parâmetro inválido.";
                    break;
                case "51":
                    message = "Produto ou serviço desativado para este comerciante. Entre em contato com a Rede";
                    break;
                case "53":
                    message = "Transação não permitida para o emissor. Entre em contato com a Rede.";
                    break;
                case "55":
                    message = "Nome do titular invalido.";
                    break;
                case "56":
                    message = "Erro nos dados relatados. Tente novamente.";
                    break;
                case "59":
                    message = "Nome do titular invalido.";
                    break;
                case "86":
                    message = "Cartão expirado";
                    break;
                case "150":
                    message = "Tempo limite excedido.";
                    break;
                case "153":
                    message = "Documento não é válido";
                    break;
                case "123":
                    message = "Não autorizado. O portador solicitou o fim das recorrências no emissor.";
                    break;
                case "124":
                    message = "Não autorizado. Entre em contato com a Rede.";
                    break;
                case "170":
                    message = "Transação com dólar zero não permitida para este cartão.";
                    break;
                case "174":
                    message = "Sucesso de transação em dólar zero.";
                    break;
                default:
                    message = "Erro não indentificado.";
                    break;

            }

            return message;
        }
    }
}