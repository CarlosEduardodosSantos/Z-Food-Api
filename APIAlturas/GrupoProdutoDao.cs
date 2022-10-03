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
    public class GrupoProdutoDao
    {
        private readonly IConfiguration _configuration;

        public GrupoProdutoDao(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public GrupoProduto ObterPorId(string grupoId)
        {
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                var sql = new StringBuilder();
                sql.AppendLine("Select Grupos.*, Categorias.* From GrupoCategoriaRelacao");
                sql.AppendLine("Inner Join Grupos On GrupoCategoriaRelacao.GrupoId = Grupos.GupoId");
                sql.AppendLine("Inner Join Categorias On GrupoCategoriaRelacao.categoriaId = Categorias.categoriaId");
                sql.AppendLine("Where Grupos.GupoId = @grupoId");
                sql.AppendLine("Order By Grupos.Sequencia, Categorias.Sequencia");

                var lookup = new Dictionary<Guid, GrupoProduto>();
                conn.Query<GrupoProduto, Categoria, GrupoProduto>(sql.ToString(),
                    (g, c) =>
                    {
                        GrupoProduto shop;
                        if (!lookup.TryGetValue(g.GupoId, out shop))
                        {
                            lookup.Add(g.GupoId, shop = g);
                        }
                        if (shop.Categorias == null)
                            shop.Categorias = new List<Categoria>();
                        if (c != null)
                            shop.Categorias.Add(c);

                        return g;
                    }, new { grupoId }, splitOn: "GrupoId, categoriaId").AsQueryable();

                var grupos = lookup.Values.FirstOrDefault();

                return grupos;
            }
        }

        public IEnumerable<GrupoProduto> ObterPorRestauranteId(string restauranteId)
        {
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                var sql = new StringBuilder();
                sql.AppendLine("Select Grupos.*, Categorias.* From Grupos");
                sql.AppendLine("Left Join GrupoCategoriaRelacao On GrupoCategoriaRelacao.GrupoId = Grupos.GupoId");
                sql.AppendLine("Left Join Categorias On GrupoCategoriaRelacao.categoriaId = Categorias.categoriaId");
                sql.AppendLine("Where Grupos.RestauranteId = @restauranteId And Grupos.Situacao = 1");
                sql.AppendLine("Order By Grupos.Sequencia, Categorias.Sequencia");

                var lookup = new Dictionary<Guid, GrupoProduto>();
                conn.Query<GrupoProduto, Categoria, GrupoProduto>(sql.ToString(),
                    (g, c) =>
                    {
                        GrupoProduto shop;
                        if (!lookup.TryGetValue(g.GupoId, out shop))
                        {
                            lookup.Add(g.GupoId, shop = g);
                        }
                        if (shop.Categorias == null)
                            shop.Categorias = new List<Categoria>();
                        if(c != null)
                            shop.Categorias.Add(c);

                        return g;
                    }, new {restauranteId}, splitOn: "GrupoId, categoriaId").AsQueryable();

                var grupos = lookup.Values;

                return grupos;
            }
        }

        public void Adicionar(GrupoProduto grupo)
        {
            var sql = new StringBuilder();
            sql.AppendLine("Insert Into Grupos(GupoId, RestauranteId, Situacao, Nome, Imagem, ImagemZimmer, Sequencia)");
            sql.AppendLine("Values (@GupoId, @RestauranteId, @Situacao, @Nome, @Imagem, @ImagemZimmer, @Sequencia)");

            var parms = new DynamicParameters();
            parms.Add("@GupoId", Guid.NewGuid());
            parms.Add("@RestauranteId", grupo.RestauranteId);
            parms.Add("@Situacao", 1);
            parms.Add("@Nome", grupo.Nome);
            parms.Add("@Descricao", grupo.Descricao);
            parms.Add("@Imagem", grupo.Imagem);
            parms.Add("@ImagemZimmer", grupo.ImagemZimmer);
            parms.Add("@Sequencia", grupo.Sequencia);
            
            using (var conn = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql.ToString(), parms);
                conn.Close();
            }

        }
        public void Alterar(GrupoProduto grupo)
        {
            var sql = new StringBuilder();
            sql.AppendLine("Update Grupos Set ");
            sql.AppendLine("RestauranteId = @RestauranteId,");
            sql.AppendLine("Situacao = @Situacao,");
            sql.AppendLine("Nome = @Nome,");
            sql.AppendLine("Descricao = @Descricao,");
            sql.AppendLine("Imagem = @Imagem,");
            sql.AppendLine("ImagemZimmer = @ImagemZimmer,");
            sql.AppendLine("Sequencia = @Sequencia");
            sql.AppendLine("Where GupoId = @GupoId");

            var parms = new DynamicParameters();
            parms.Add("@GupoId", grupo.GupoId);
            parms.Add("@RestauranteId", grupo.RestauranteId);
            parms.Add("@Situacao", 1);
            parms.Add("@Nome", grupo.Nome);
            parms.Add("@Descricao", grupo.Descricao);
            parms.Add("@Imagem", grupo.Imagem);
            parms.Add("@ImagemZimmer", grupo.ImagemZimmer);
            parms.Add("@Sequencia", grupo.Sequencia);

            using (var conn = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql.ToString(), parms);
                conn.Close();
            }

        }

        public void Excluir(string id)
        {
            var sql = new StringBuilder();
            sql.AppendLine("Delete From Grupos Where GupoId = @GupoId");


            using (var conn = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql.ToString(), new { GupoId  = id});
                conn.Close();
            }

        }

        public void RelacaoGrupoCategoria(GrupoCategoriaRelacao relacao)
        {
            var sql = new StringBuilder();
            sql.AppendLine("Insert Into GrupoCategoriaRelacao(GrupoCategoriaId, CategoriaId, GrupoId)");
            sql.AppendLine("Values (@GrupoCategoriaId, @CategoriaId, @GrupoId)");

            var parms = new DynamicParameters();
            parms.Add("@GrupoCategoriaId", Guid.NewGuid());
            parms.Add("@CategoriaId", relacao.CategoriaId);
            parms.Add("@GrupoId", relacao.GrupoId);


            using (var conn = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql.ToString(), parms);
                conn.Close();
            }

        }

        public void ExcluirRelacao(string id)
        {
            var sql = new StringBuilder();
            sql.AppendLine("Delete From GrupoCategoriaRelacao Where GrupoCategoriaId = @GrupoCategoriaId");


            using (var conn = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql.ToString(), new { GrupoCategoriaId = id });
                conn.Close();
            }

        }

        public IEnumerable<GrupoProduto> getGroupsByRestId(int restId, int deepLevel = 0)
        {
            string sql =
                "SELECT * FROM grupos " +
                "WHERE restauranteId = @restId";

            using(SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                IEnumerable<GrupoProduto> grupos = conn.Query<GrupoProduto>(sql, new { restId });

                if (deepLevel == 0) return grupos;

                string sqlCategories =
                    "SELECT * FROM categorias " +
                    "WHERE categoriaId in (" +
                        "SELECT categoriaId FROM grupoCategoriaRelacao " +
                        "WHERE grupoId = @grupoId " +
                    ")";

                foreach(GrupoProduto grupo in grupos)
                {
                    IEnumerable<Categoria> categories = conn.Query<Categoria>(sqlCategories, new { grupoId = grupo.GupoId });
                    grupo.Categorias = categories.ToList();

                    if (deepLevel == 1) continue;

                    string sqlProducts =
                        "SELECT *, (" +
                            "SELECT COUNT(ProdutosOpcaoId) FROM ProdutosOpcaoTipoRelacao AS tr WHERE tr.produtoId = produtos.produtoId" +
                        ") AS optionsCount " +
                        "FROM produtos " +
                        "WHERE categoriaId = @categoriaId AND Situacao = 1 " +
                        "AND Dbo.fnEstaDisponivel(Produtos.produtoId) = 1 " +
                        "Order by produtos.Sequencia";

                    foreach (Categoria category in categories)
                    {
                        IEnumerable<Produto> products = conn.Query<Produto>(sqlProducts, new { categoriaId = category.CategoriaId });
                        category.produtos = products;
                    }
                }

                return grupos;
            }
        }
    }
}