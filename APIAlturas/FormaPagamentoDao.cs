using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using APIAlturas.ViewModels;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace APIAlturas
{
    public class FormaPagamentoDao
    {
        private readonly IConfiguration _configuration;

        public FormaPagamentoDao(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<FormaPagamento> ObterPorToken(string token)
        {
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();

                var formaPagamentos = conn
                    .Query<FormaPagamento>("Select * From FormaPagamentos Where RestauranteToken = @token And Situacao = 1  Order By Sequencia", new {token})
                    .ToList();
                conn.Close();

                return formaPagamentos;
            }
        }

        public void Insert(FormaPagamento formaPagamento)
        {
            var sql = "Insert Into FormaPagamentos(FormaPagamentoId, Situacao, RestauranteToken, Descricao, IsOnline, IsTroco, TipoCartao, Sequencia, Percentual, Imagem) " +
                      "Values (@FormaPagamentoId, @Situacao, @RestauranteToken, @Descricao, @IsOnline, @IsTroco, @TipoCartao, @Sequencia, @Percentual, @Imagem)";
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql,
                    new
                    {
                        FormaPagamentoId = formaPagamento.FormaPagamentoId,
                        Situacao = formaPagamento.Situacao,
                        RestauranteToken = formaPagamento.RestauranteToken,
                        Descricao = formaPagamento.Descricao,
                        IsOnline = formaPagamento.IsOnline,
                        IsTroco = formaPagamento.IsTroco,
                        TipoCartao = formaPagamento.TipoCartao,
                        Sequencia = formaPagamento.Sequencia,
                        Percentual = formaPagamento.Percentual,
                        Imagem = formaPagamento.Imagem
                    });
                conn.Close();
            }
        }
        public void Alterar(FormaPagamento formaPagamento)
        {
            var sql = new StringBuilder();
            sql.AppendLine("Update FormaPagamentos Set");
            sql.AppendLine("Situacao = @Situacao,");
            sql.AppendLine("Descricao = @Descricao,");
            sql.AppendLine("IsOnline = @IsOnline,");
            sql.AppendLine("IsTroco = @IsTroco,");
            sql.AppendLine("TipoCartao = @TipoCartao,");
            sql.AppendLine("Sequencia = @Sequencia,");
            sql.AppendLine("Percentual = @Percentual,");
            sql.AppendLine("Imagem = @Imagem");
            sql.AppendLine("Where FormaPagamentoId = @FormaPagamentoId");

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql.ToString(),
                    new
                    {
                        FormaPagamentoId = formaPagamento.FormaPagamentoId,
                        Situacao = formaPagamento.Situacao,
                        Descricao = formaPagamento.Descricao,
                        IsOnline = formaPagamento.IsOnline,
                        IsTroco = formaPagamento.IsTroco,
                        TipoCartao = formaPagamento.TipoCartao,
                        Sequencia = formaPagamento.Sequencia,
                        Percentual = formaPagamento.Percentual,
                        Imagem = formaPagamento.Imagem
                    });
                conn.Close();
            }
        }

        public List<EspeciePagamento> ObterEspecies()
        {
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();

                var especies = conn
                    .Query<EspeciePagamento>("Select * From EspeciePagamentos")
                    .ToList();
                conn.Close();

                return especies;
            }
        }
        public void Excluir(string id)
        {
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();

                conn.Query("Delete From FormaPagamentos Where FormaPagamentoId = @id", new {id});
                conn.Close();

            }
        }
    }
}