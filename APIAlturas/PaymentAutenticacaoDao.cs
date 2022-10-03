using System.Data.SqlClient;
using System.Linq;
using APIAlturas.ViewModels;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace APIAlturas
{
    public class PaymentAutenticacaoDao
    {
        private readonly IConfiguration _configuration;

        public PaymentAutenticacaoDao(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Adicionar(PaymentAutenticacao paymentAutenticacao)
        {
            var sql = "Insert Into PaymentAutenticacao(PaymentAutenticacaoId, RetauranteId, usuario, Senha, Token, Sandbox, Operadora, Padrao) " +
                      "Values (@PaymentAutenticacaoId, @RetauranteId, @usuario, @Senha, @Token, @Sandbox, @Operadora, @Padrao)";
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql,
                    new
                    {
                        PaymentAutenticacaoId = paymentAutenticacao.PaymentAutenticacaoId,
                        RetauranteId = paymentAutenticacao.RetauranteId,
                        usuario = paymentAutenticacao.usuario,
                        Senha = paymentAutenticacao.Senha,
                        Token = paymentAutenticacao.Token,
                        Sandbox = paymentAutenticacao.Sandbox,
                        Operadora = paymentAutenticacao.Operadora,
                        Padrao = true
                    });
                conn.Close();
            }
        }

        public PaymentAutenticacao ObterPorRestauranteId(string restauranteId)
        {
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var autenticacao = conn
                    .Query<PaymentAutenticacao>("Select * From PaymentAutenticacao Where Padrao = 1 And RetauranteId = @restauranteId", new { restauranteId })
                    .FirstOrDefault();
                conn.Close();

                return autenticacao;
            }
        }
    }
}