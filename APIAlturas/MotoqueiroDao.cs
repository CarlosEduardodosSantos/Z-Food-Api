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
    public class MotoqueiroDao
    {
        private readonly IConfiguration _configuration;

        public MotoqueiroDao(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Motoqueiro Find(string email)
        {
            using (SqlConnection conexao = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                return conexao.QueryFirstOrDefault<Motoqueiro>(
                    "SELECT * FROM dbo.Motoqueiros " +
                    "WHERE email = @email", new { email });
            }
        }

        public void Adicionar(Motoqueiro motoqueiro)
        {
            using (var conexao = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                var sql = new StringBuilder();
                sql.AppendLine("Insert Into Motoqueiros(MotoqueiroId, Situacao, Nome, Email, Celular, Senha, ContaBanco,");
                sql.AppendLine("ContaAgencia, ContaNumero, Cpf, PlayerId, Imagem)Values (");

                sql.AppendLine("@MotoqueiroId, @Situacao, @Nome, @Email, @Celular, @Senha, @ContaBanco,");
                sql.AppendLine("@ContaAgencia, @ContaNumero, @Cpf, @PlayerId, @Imagem)");
                var param = new DynamicParameters();
                param.Add("@MotoqueiroId", Guid.NewGuid());
                param.Add("@Situacao", motoqueiro.Situacao);
                param.Add("@Nome", motoqueiro.Nome);
                param.Add("@Email", motoqueiro.Email);
                param.Add("@Celular", motoqueiro.Celular);
                param.Add("@Senha", motoqueiro.Senha);
                param.Add("@ContaBanco", motoqueiro.ContaBanco);

                param.Add("@ContaAgencia", motoqueiro.ContaAgencia);
                param.Add("@ContaNumero", motoqueiro.ContaNumero);
                param.Add("@Cpf", motoqueiro.Cpf);
                param.Add("@PlayerId", motoqueiro.PlayerId);
                param.Add("@Imagem", motoqueiro.Imagem);

                conexao.Query(sql.ToString(), param);
            }
        }

        public void Alterar(Motoqueiro motoqueiro)
        {
            using (SqlConnection conexao = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                var sql = new StringBuilder();
                sql.AppendLine("Update Motoqueiros Set ");
                sql.AppendLine("Situacao = @Situacao,");
                sql.AppendLine("Nome = @Nome,");
                sql.AppendLine("Email = @Email,");
                sql.AppendLine("Celular = @Celular,");
                sql.AppendLine("Senha = @Password,");
                sql.AppendLine("Senha = @Senha,");
                sql.AppendLine("ContaBanco = @ContaBanco,");
                sql.AppendLine("ContaAgencia = @ContaAgencia,");
                sql.AppendLine("ContaNumero = @ContaNumero,");
                sql.AppendLine("Cpf = @Cpf,");
                sql.AppendLine("PlayerId = @PlayerId,");
                sql.AppendLine("Imagem = @Imagem");
                sql.AppendLine("Where  MotoqueiroId = @MotoqueiroId");

                var param = new DynamicParameters();
                param.Add("@MotoqueiroId", Guid.NewGuid());
                param.Add("@Situacao", motoqueiro.Situacao);
                param.Add("@Nome", motoqueiro.Nome);
                param.Add("@Email", motoqueiro.Email);
                param.Add("@Celular", motoqueiro.Celular);
                param.Add("@Senha", motoqueiro.Senha);
                param.Add("@ContaBanco", motoqueiro.ContaBanco);
                param.Add("@ContaAgencia", motoqueiro.ContaAgencia);
                param.Add("@ContaNumero", motoqueiro.ContaNumero);
                param.Add("@Cpf", motoqueiro.Cpf);
                param.Add("@PlayerId", motoqueiro.PlayerId);
                param.Add("@Imagem", motoqueiro.Imagem);

                conexao.Query(sql.ToString(), param);
            }
        }

        public List<Motoqueiro> ObterTodos()
        {
            using (SqlConnection conexao = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                return conexao.Query<Motoqueiro>("SELECT * FROM dbo.Motoqueiros").ToList();
            }
        }
    }
}