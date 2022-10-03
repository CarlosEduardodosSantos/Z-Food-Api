using System.Data.SqlClient;
using APIAlturas.ViewModels;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace APIAlturas
{
    public class PaymentRetornoDao
    {
        private readonly IConfiguration _configuration;

        public PaymentRetornoDao(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Adicionar(PaymentResult paymentRetorno)
        {
            var sql = "Insert Into PaymentRetorno(PaymentResultId, UserId, Status, CodigoAutorizacao, DataHora, ReferenciaId, Menssage, Nsu) " +
                      "Values (@PaymentResultId, @UserId, @Status, @CodigoAutorizacao, @DataHora, @ReferenciaId, @Menssage, @Nsu)";
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql,
                    new
                    {
                        PaymentResultId = paymentRetorno.PaymentResultId,
                        UserId = paymentRetorno.UserId,
                        Status = paymentRetorno.Status,
                        CodigoAutorizacao = paymentRetorno.CodigoAutorizacao,
                        DataHora = paymentRetorno.DataHora,
                        ReferenciaId = paymentRetorno.ReferenciaId,
                        Menssage = paymentRetorno.Menssage,
                        Nsu = paymentRetorno.Nsu
                    });
                conn.Close();
            }
        }
    }
}