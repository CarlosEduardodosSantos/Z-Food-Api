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
    public class ProdutoOpcaoDao
    {
        private readonly IConfiguration _configuration;

        public ProdutoOpcaoDao(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<ProdutoOpcaoTipo> GetByGestor(string restauranteId, int produtoId)
        {
            var sql = new StringBuilder();
            sql.AppendLine("Select ProdutosOpcaoTipos.*, ProdutosOpcoes.* From ProdutosOpcaoTipos");
            sql.AppendLine("Left Join ProdutosOpcoes On ProdutosOpcaoTipos.ProdutosOpcaoTipoId = ProdutosOpcoes.ProdutosOpcaoTipoId");
            sql.AppendLine("Left Join ProdutosOpcaoTipoRelacao On ProdutosOpcoes.ProdutosOpcaoId = ProdutosOpcaoTipoRelacao.ProdutosOpcaoId");
            sql.AppendLine("Where ProdutosOpcoes.RestauranteId = @restauranteId And ProdutosOpcaoTipoRelacao.ProdutoId = @produtoId");
            sql.AppendLine("Order By ProdutosOpcaoTipos.Sequencia, ProdutosOpcoes.Sequencia");

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();

                var lookup = new Dictionary<int, ProdutoOpcaoTipo>();
                conn.Query<ProdutoOpcaoTipo, ProdutoOpcao, ProdutoOpcaoTipo>(sql.ToString(),
                    (pot, po) =>
                    {
                        ProdutoOpcaoTipo shop;
                        if (!lookup.TryGetValue(pot.ProdutosOpcaoTipoId, out shop))
                        {
                            lookup.Add(pot.ProdutosOpcaoTipoId, shop = pot);
                        }

                        if (shop.ProdutoOpcaos == null)
                            shop.ProdutoOpcaos = new List<ProdutoOpcao>();

                        if (po != null)
                            shop.ProdutoOpcaos.Add(po);

                        return shop;
                    }, new { restauranteId, produtoId }, splitOn: "ProdutosOpcaoTipoId, ProdutosOpcaoId").AsQueryable();
                var opcao = lookup.Values;

                conn.Close();

                return opcao;
            }


        }

        public IEnumerable<ProdutoOpcaoTipo> GetByProdutoId(string restauranteId, int produtoId)
        {
            var sql = new StringBuilder();
            sql.AppendLine("Select ProdutosOpcaoTipos.*, ProdutosOpcoes.* From ProdutosOpcaoTipos");
            sql.AppendLine("Inner Join ProdutosOpcoes On ProdutosOpcaoTipos.ProdutosOpcaoTipoId = ProdutosOpcoes.ProdutosOpcaoTipoId");
            sql.AppendLine("Inner Join ProdutosOpcaoTipoRelacao On ProdutosOpcoes.ProdutosOpcaoId = ProdutosOpcaoTipoRelacao.ProdutosOpcaoId");
            sql.AppendLine("Where ProdutosOpcoes.RestauranteId = @restauranteId And ProdutosOpcaoTipoRelacao.ProdutoId = @produtoId");
            sql.AppendLine("And Isnull(ProdutosOpcoes.Situacao,1) = 1");
            sql.AppendLine("Order By ProdutosOpcaoTipos.Sequencia, ProdutosOpcoes.Sequencia");

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();

                var lookup = new Dictionary<int, ProdutoOpcaoTipo>();
                conn.Query<ProdutoOpcaoTipo, ProdutoOpcao, ProdutoOpcaoTipo>(sql.ToString(),
                    (pot, po) =>
                    {
                        ProdutoOpcaoTipo shop;
                        if (!lookup.TryGetValue(pot.ProdutosOpcaoTipoId, out shop))
                        {
                            lookup.Add(pot.ProdutosOpcaoTipoId, shop = pot);
                        }

                        if (shop.ProdutoOpcaos == null)
                            shop.ProdutoOpcaos = new List<ProdutoOpcao>();

                        if (po != null)
                            shop.ProdutoOpcaos.Add(po);

                        return shop;
                    }, new { restauranteId, produtoId }, splitOn: "ProdutosOpcaoTipoId, ProdutosOpcaoId").AsQueryable();
                var opcao = lookup.Values;

                conn.Close();

                return opcao;
            }


        }

        public IEnumerable<ProdutoOpcaoTipo> ObterTipoByRestaurante(string restauranteId)
        {
            using (var conn = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                var sqlTp1 = new StringBuilder();
                sqlTp1.AppendLine("Select ProdutosOpcaoTipos.*, ProdutosOpcoes.* From ProdutosOpcaoTipos");
                sqlTp1.AppendLine("Left Join ProdutosOpcoes On ProdutosOpcaoTipos.ProdutosOpcaoTipoId = ProdutosOpcoes.ProdutosOpcaoTipoId");
                //sqlTp1.AppendLine("Left Join ProdutosOpcaoTipoRelacao On ProdutosOpcoes.ProdutosOpcaoId = ProdutosOpcaoTipoRelacao.ProdutosOpcaoId");
                sqlTp1.AppendLine("Where ProdutosOpcaoTipos.RestauranteId = @restauranteId");

                conn.Open();
                var lookup = new Dictionary<int, ProdutoOpcaoTipo>();
                conn.Query<ProdutoOpcaoTipo, ProdutoOpcao, ProdutoOpcaoTipo>(sqlTp1.ToString(),
                    (p1, p2) =>
                    {
                        ProdutoOpcaoTipo shop;
                        if (!lookup.TryGetValue(p1.ProdutosOpcaoTipoId, out shop))
                        {
                            lookup.Add(p1.ProdutosOpcaoTipoId, shop = p1);
                        }


                        if (shop.ProdutoOpcaos == null)
                            shop.ProdutoOpcaos = new List<ProdutoOpcao>();
                        if (p2 != null)
                            shop.ProdutoOpcaos.Add(p2);

                        return shop;

                    }, new { restauranteId }, splitOn: "ProdutosOpcaoTipoId, ProdutosOpcaoId").AsQueryable();
                var resultList = lookup.Values;
                conn.Close();

                return resultList;
            }

        }

        public void Insert(ProdutoOpcao produtoOpcao)
        {
            var sql = "Insert Into ProdutosOpcoes(ProdutosOpcaoId, ProdutosOpcaoTipoId, RestauranteId, ProdutoId, Nome, Valor, Sequencia, ProdutoPdv, Situacao, Replicar) " +
                      "Values (@ProdutosOpcaoId, @ProdutosOpcaoTipoId, @RestauranteId, @ProdutoId, @Nome, @Valor, @Sequencia, @ProdutoPdv, @Situacao, @Replicar)";
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql,
                    new
                    {
                        ProdutosOpcaoId = produtoOpcao.ProdutosOpcaoId,
                        ProdutosOpcaoTipoId = produtoOpcao.ProdutosOpcaoTipoId,
                        RestauranteId = produtoOpcao.RestauranteId,
                        ProdutoId = produtoOpcao.ProdutoId,
                        Nome = produtoOpcao.Nome,
                        Valor = produtoOpcao.Valor,
                        Sequencia = produtoOpcao.Sequencia,
                        ProdutoPdv = produtoOpcao.ProdutoPdv,
                        Situacao = 1,
                        Replicar = produtoOpcao.Replicar
                    });
                conn.Close();

            }
        }
        public void Relacionar(ProdutosOpcaoTipoRelacao produtosOpcaoTipoRelacao)
        {
            var sql = "Insert Into ProdutosOpcaoTipoRelacao(Id, ProdutosOpcaoId, ProdutoId) " +
                      "Values (@Id, @ProdutosOpcaoId, @ProdutoId)";
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql,
                    new
                    {
                        Id = Guid.NewGuid(),
                        ProdutosOpcaoId = produtosOpcaoTipoRelacao.ProdutosOpcaoId,
                        ProdutoId = produtosOpcaoTipoRelacao.ProdutoId
                    });
                conn.Close();
            }
        }

        public void Alterar(ProdutoOpcao produtoOpcao)
        {
            var sql = new StringBuilder();
            sql.AppendLine("Update ProdutosOpcoes Set");
            sql.AppendLine("ProdutosOpcaoTipoId = @ProdutosOpcaoTipoId,");
            sql.AppendLine("RestauranteId = @RestauranteId,");
            sql.AppendLine("Situacao = @Situacao,");
            sql.AppendLine("ProdutoId = @ProdutoId,");
            sql.AppendLine("Nome = @Nome,");
            sql.AppendLine("Valor = @Valor,");
            sql.AppendLine("Sequencia = @Sequencia,");
            sql.AppendLine("ProdutoPdv = @ProdutoPdv,");
            sql.AppendLine("Replicar = @Replicar");
            sql.AppendLine("Where ProdutosOpcaoId = @ProdutosOpcaoId");

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql.ToString(),
                    new
                    {
                        ProdutosOpcaoId = produtoOpcao.ProdutosOpcaoId,
                        ProdutosOpcaoTipoId = produtoOpcao.ProdutosOpcaoTipoId,
                        RestauranteId = produtoOpcao.RestauranteId,
                        ProdutoId = produtoOpcao.ProdutoId,
                        Nome = produtoOpcao.Nome,
                        Valor = produtoOpcao.Valor,
                        Sequencia = produtoOpcao.Sequencia,
                        ProdutoPdv = produtoOpcao.ProdutoPdv,
                        Situacao = produtoOpcao.Situacao,
                        Replicar = produtoOpcao.Replicar
                    });
                conn.Close();
            }
        }

        public void Deletar(string id)
        {
            var sql = new StringBuilder();
            sql.AppendLine("Delete ProdutosOpcoes Where ProdutosOpcaoId = @ProdutosOpcaoId");

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql.ToString(), new { ProdutosOpcaoId = id });
                conn.Close();
            }
        }

        public void DeletarRelacao(string produtosOpcaoId, int produtoId)
        {
            var sql = new StringBuilder();
            sql.AppendLine("Delete ProdutosOpcaoTipoRelacao Where ProdutosOpcaoId = @produtosOpcaoId And ProdutoId = @produtoId");

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql.ToString(), new { produtosOpcaoId, produtoId });
                conn.Close();
            }
        }

        public void InsertTipo(ProdutoOpcaoTipo produtoOpcaoTipo)
        {
            var sql = "Insert Into ProdutosOpcaoTipos(RestauranteId, Nome, Quantidade, QtdeMax, Obrigatorio, Sequencia, Tipo, Situacao) " +
                      "Values (@RestauranteId, @Nome, @Quantidade, @QtdeMax, @Obrigatorio, @Sequencia, @Tipo, @Situacao)";
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql,
                    new
                    {
                        RestauranteId = produtoOpcaoTipo.RestauranteId,
                        Nome = produtoOpcaoTipo.Nome,
                        Quantidade = produtoOpcaoTipo.Quantidade,
                        QtdeMax = produtoOpcaoTipo.QtdeMax,
                        Obrigatorio = produtoOpcaoTipo.Obrigatorio,
                        Sequencia = produtoOpcaoTipo.Sequencia,
                        Tipo = produtoOpcaoTipo.Tipo,
                        Situacao = 999
                    });
                conn.Close();
            }
        }

        public void AlterarTipo(ProdutoOpcaoTipo produtoOpcaoTipo)
        {
            var sql = new StringBuilder();
            sql.AppendLine("Update ProdutosOpcaoTipos Set");
            sql.AppendLine("RestauranteId = @RestauranteId,");
            sql.AppendLine("Tipo = @Tipo,");
            sql.AppendLine("Situacao = @Situacao,");
            sql.AppendLine("Nome = @Nome,");
            sql.AppendLine("Quantidade = @Quantidade,");
            sql.AppendLine("QtdeMax = @QtdeMax,");
            sql.AppendLine("Obrigatorio = @Obrigatorio,");
            sql.AppendLine("Sequencia = @Sequencia");
            sql.AppendLine("Where ProdutosOpcaoTipoId = @ProdutosOpcaoTipoId");

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql.ToString(),
                    new
                    {
                        RestauranteId = produtoOpcaoTipo.RestauranteId,
                        Tipo = produtoOpcaoTipo.Tipo,
                        Nome = produtoOpcaoTipo.Nome,
                        Quantidade = produtoOpcaoTipo.Quantidade,
                        QtdeMax = produtoOpcaoTipo.QtdeMax,
                        Obrigatorio = produtoOpcaoTipo.Obrigatorio,
                        Sequencia = produtoOpcaoTipo.Sequencia,
                        ProdutosOpcaoTipoId = produtoOpcaoTipo.ProdutosOpcaoTipoId,
                        Situacao = produtoOpcaoTipo.Situacao
                    });
                conn.Close();
            }
        }

        public IEnumerable<int> ObterProdutoOpcaoRelacao(int produtosOpcaoTipoId)
        {
            var sql = new StringBuilder();
            sql.AppendLine("select Distinct ProdutosOpcaoTipoRelacao.ProdutoId from ProdutosOpcaoTipoRelacao");
            sql.AppendLine("Inner Join ProdutosOpcoes On  ProdutosOpcaoTipoRelacao.ProdutosOpcaoId = ProdutosOpcoes.ProdutosOpcaoId");
            sql.AppendLine("Inner Join ProdutosOpcaoTipos On ProdutosOpcoes.ProdutosOpcaoTipoId =  ProdutosOpcaoTipos.ProdutosOpcaoTipoId");
            sql.AppendLine("Where ProdutosOpcaoTipos.ProdutosOpcaoTipoId = @produtosOpcaoTipoId");
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var relacoes = conn.Query<int>(sql.ToString(), new { produtosOpcaoTipoId });
                conn.Close();

                return relacoes;
            }
        }

    }
}