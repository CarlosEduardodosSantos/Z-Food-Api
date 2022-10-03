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
    public class CupomDao
    {
        private readonly IConfiguration _configuration;

        public CupomDao(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Adicionar(Cupom cupom)
        {
            var sql = new StringBuilder();
            sql.AppendLine("Insert Into Cupons(CupomId, RestauranteId, Situacao, Tipo, Quantidade, QuantidadeUso, Valor, Percentual, Descricao,");
            sql.AppendLine("Nome, ValorMinimo, DataHora, Validade, UserId, Codigo, NoLImited)");
            sql.AppendLine("Values (@CupomId, @RestauranteId, @Situacao, @Tipo, @Quantidade, @QuantidadeUso, @Valor, @Percentual, @Descricao,");
            sql.AppendLine("@Nome, @ValorMinimo, @DataHora, @Validade, @UserId, @Codigo, @NoLImited)");

            var parms = new DynamicParameters();
            parms.Add("@CupomId", Guid.NewGuid());
            parms.Add("@RestauranteId", cupom.RestauranteId);
            parms.Add("@Situacao", 1);
            parms.Add("@Tipo", cupom.Tipo);
            parms.Add("@Quantidade", cupom.Quantidade);
            parms.Add("@QuantidadeUso", cupom.QuantidadeUso);
            parms.Add("@Valor", cupom.Valor);
            parms.Add("@Percentual", cupom.Percentual);
            parms.Add("@Nome", cupom.Nome);
            parms.Add("@Descricao", cupom.Descricao);
            parms.Add("@ValorMinimo", cupom.ValorMinimo);
            parms.Add("@DataHora", cupom.DataHora);
            parms.Add("@Validade", cupom.Validade);
            parms.Add("@UserId", cupom.UserId);
            parms.Add("@Codigo", cupom.Codigo);
            parms.Add("@NoLImited", cupom.NoLImited);

            using (var conn = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql.ToString(), parms);
                conn.Close();
            }

        }

        public void Remover()
        {
            using (var conn = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query("Delete From Cupons Where CupomId = @CupomId");
                conn.Close();
            }
        }

        public void AlteraSituacao(string cupomId, int situacao)
        {
            using (var conn = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query("Update Cupons Set  Situacao = @situacao Where CupomId = @cupomId", new { cupomId, situacao });
                conn.Close();
            }
        }

        public List<Cupom> ObterDisponiveis(string usuarioId, int restauranteId)
        {
            var cupons = new List<Cupom>();

            using (var conn = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                //Cupom Primeira Compra
                var sqlTp1 = new StringBuilder();
                sqlTp1.AppendLine("Select * From Cupons");
                sqlTp1.AppendLine("Where RestauranteId = @restauranteId  And Tipo = 1 And Situacao = 1 And Isnull(Codigo, '') = ''");
                sqlTp1.AppendLine("And (NoLImited = 1 Or @usuarioId Not In (Select UserId From CupomMovimentacoes Where Cupons.CupomId = CupomMovimentacoes.CupomId))");

                //Cupom por quantidade
                var sqlTp2 = new StringBuilder();
                sqlTp2.AppendLine("Select * From Cupons");
                sqlTp2.AppendLine("Where RestauranteId = @restauranteId And Validade >= Getdate()");
                sqlTp2.AppendLine("And (Quantidade-QuantidadeUso) > 0 And Tipo = 2 And Isnull(Codigo, '') = ''");
                sqlTp2.AppendLine("And (NoLImited = 1 Or @usuarioId Not In (Select UserId From CupomMovimentacoes Where Cupons.CupomId = CupomMovimentacoes.CupomId))");


                //Cupom por Validade
                var sqlTp3 = new StringBuilder();
                sqlTp3.AppendLine("Select * From Cupons");
                sqlTp3.AppendLine("Where RestauranteId = @restauranteId And Validade >= Getdate()");
                sqlTp3.AppendLine("And Situacao = 1 And Tipo = 3 And Isnull(Codigo, '') = ''");
                sqlTp3.AppendLine("And (NoLImited = 1 Or @usuarioId Not In (Select UserId From CupomMovimentacoes Where Cupons.CupomId = CupomMovimentacoes.CupomId))");

                //Cupom Frete Gratis
                var sqlTp4 = new StringBuilder();
                sqlTp4.AppendLine("Select * From Cupons");
                sqlTp4.AppendLine("Where RestauranteId = @restauranteId And Tipo = 4 And Situacao = 1");
                sqlTp4.AppendLine("And (NoLImited = 1 Or @usuarioId Not In (Select UserId From CupomMovimentacoes Where Cupons.CupomId = CupomMovimentacoes.CupomId))");

                //Cupom por usuario
                var sqlTp5 = new StringBuilder();
                sqlTp5.AppendLine("Select * From Cupons");
                sqlTp5.AppendLine("Where RestauranteId = @restauranteId And Tipo = 5  And Situacao = 1");
                sqlTp5.AppendLine("And UserId = @usuarioId And Isnull(Codigo, '') = ''");
                sqlTp5.AppendLine("And (NoLImited = 1 Or @usuarioId Not In (Select UserId From CupomMovimentacoes Where Cupons.CupomId = CupomMovimentacoes.CupomId))");


                conn.Open();
                var cupons1 = conn.Query<Cupom>(sqlTp1.ToString(), new { usuarioId, restauranteId }).ToList();
                var cupons2 = conn.Query<Cupom>(sqlTp2.ToString(), new { usuarioId, restauranteId }).ToList();
                var cupons3 = conn.Query<Cupom>(sqlTp3.ToString(), new { usuarioId, restauranteId }).ToList();
                var cupons4= conn.Query<Cupom>(sqlTp4.ToString(), new { usuarioId, restauranteId }).ToList();
                var cupons5 = conn.Query<Cupom>(sqlTp5.ToString(), new { usuarioId, restauranteId }).ToList();
                conn.Close();

                cupons.AddRange(cupons1);
                cupons.AddRange(cupons2);
                cupons.AddRange(cupons3);
                cupons.AddRange(cupons4);
                cupons.AddRange(cupons5);

                return cupons;


            }
        }

        public Cupom ObterCodigo(string usuarioId, int restauranteId, string codigo)
        {

            using (var conn = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                //Cupom Primeira Compra
                var sqlTp1 = new StringBuilder();
                sqlTp1.AppendLine("Select * From Cupons");
                sqlTp1.AppendLine("Where RestauranteId = @restauranteId  And Tipo = 1 And Situacao = 1 And Isnull(Codigo, '') = @codigo");
                sqlTp1.AppendLine("And (NoLImited = 1 Or @usuarioId Not In (Select UserId From CupomMovimentacoes Where Cupons.CupomId = CupomMovimentacoes.CupomId))");

                //Cupom por quantidade
                var sqlTp2 = new StringBuilder();
                sqlTp2.AppendLine("Select * From Cupons");
                sqlTp2.AppendLine("Where RestauranteId = @restauranteId And Validade >= Getdate()");
                sqlTp2.AppendLine("And (Quantidade-QuantidadeUso) > 0  And Situacao = 1  And Tipo = 2 And Isnull(Codigo, '') = @codigo");
                sqlTp2.AppendLine("And (NoLImited = 1 Or @usuarioId Not In (Select UserId From CupomMovimentacoes Where Cupons.CupomId = CupomMovimentacoes.CupomId))");


                //Cupom por Validade
                var sqlTp3 = new StringBuilder();
                sqlTp3.AppendLine("Select * From Cupons");
                sqlTp3.AppendLine("Where RestauranteId = @restauranteId And Validade >= Getdate()");
                sqlTp3.AppendLine("And Situacao = 1 And Tipo = 3 And Isnull(Codigo, '') = @codigo");
                sqlTp3.AppendLine("And (NoLImited = 1 Or @usuarioId Not In (Select UserId From CupomMovimentacoes Where Cupons.CupomId = CupomMovimentacoes.CupomId))");

                //Cupom Frete Gratis
                var sqlTp4 = new StringBuilder();
                sqlTp4.AppendLine("Select * From Cupons");
                sqlTp4.AppendLine("Where RestauranteId = @restauranteId And Tipo = 4 And Situacao = 1 And Isnull(Codigo, '') = @codigo");
                sqlTp4.AppendLine("And (NoLImited = 1 Or @usuarioId Not In (Select UserId From CupomMovimentacoes Where Cupons.CupomId = CupomMovimentacoes.CupomId))");

                //Cupom por usuario
                var sqlTp5 = new StringBuilder();
                sqlTp5.AppendLine("Select * From Cupons");
                sqlTp5.AppendLine("Where RestauranteId = @restauranteId And Tipo = 5  And Situacao = 1");
                sqlTp5.AppendLine("And UserId = @usuarioId And Isnull(Codigo, '') = @codigo");
                sqlTp5.AppendLine("And (NoLImited = 1 Or @usuarioId Not In (Select UserId From CupomMovimentacoes Where Cupons.CupomId = CupomMovimentacoes.CupomId))");


                conn.Open();
                var cupons1 = conn.Query<Cupom>(sqlTp1.ToString(), new {usuarioId, restauranteId, codigo });
                var cupons2 = conn.Query<Cupom>(sqlTp2.ToString(), new {usuarioId, restauranteId, codigo });
                var cupons3 = conn.Query<Cupom>(sqlTp3.ToString(), new {usuarioId, restauranteId, codigo });
                var cupons4 = conn.Query<Cupom>(sqlTp4.ToString(), new {usuarioId, restauranteId, codigo });
                var cupons5 = conn.Query<Cupom>(sqlTp5.ToString(), new {usuarioId, restauranteId, codigo });
                conn.Close();

                if (cupons1.Any())
                    return cupons1.FirstOrDefault();
                if (cupons2.Any())
                    return cupons2.FirstOrDefault();
                if (cupons3.Any())
                    return cupons3.FirstOrDefault();
                if (cupons4.Any())
                    return cupons4.FirstOrDefault();
                if (cupons5.Any())
                    return cupons5.FirstOrDefault();
                else
                    return null;
            }

        }

            public List<Cupom> ObterPorRestaurante(int restauranteId)
        {
            using (var conn = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                var sqlTp1 = new StringBuilder();
                sqlTp1.AppendLine("Select * From Cupons Where RestauranteId = @restauranteId");
                sqlTp1.AppendLine("");

                conn.Open();
                var cupons = conn.Query<Cupom>(sqlTp1.ToString(), new { restauranteId }).ToList();
                conn.Close();

                return cupons;

            }
        }

        public List<Cupom> ObterPorCodigo(int restauranteId, string codigo)
        {
            using (var conn = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                var sqlTp1 = new StringBuilder();
                sqlTp1.AppendLine("Select * From Cupons Where RestauranteId = @restauranteId And Codigo = @codigo");
                sqlTp1.AppendLine("");

                conn.Open();
                var cupons = conn.Query<Cupom>(sqlTp1.ToString(), new { restauranteId }).ToList();
                conn.Close();

                return cupons;

            }
        }

        public List<CupomMovimentacao> ObterCupomMovimentacaos(string cupomId)
        {
            using (var conn = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                var sqlTp1 = new StringBuilder();
                sqlTp1.AppendLine("Select * From CupomMovimentacoes");
                sqlTp1.AppendLine("Inner Join Users On CupomMovimentacoes.UserId = Users.userID");
                sqlTp1.AppendLine("Where CupomMovimentacoes.CupomId = @cupomId");

                conn.Open();
                var cupons = conn.Query<CupomMovimentacao, User, CupomMovimentacao>(sqlTp1.ToString(),
                    (c, u) =>
                    {
                        c.Cliente = u.Nome;
                        return c;
                    }, new { cupomId }, splitOn: "CupomMovimentacaoId, userID").ToList();
                conn.Close();

                return cupons;

            }
        }

    }
}