using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using APIAlturas.ViewModels;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace APIAlturas
{
    public class CategoriaDAO
    {
        private readonly IConfiguration _configuration;

        public CategoriaDAO(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<Categoria> GetByRestauranteToken(string token)
        {
            var sql = "Select * from Categorias Where restauranteId In (Select restauranteId From Restaurantes Where token = @token)";
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var categorias = conn.Query<Categoria>(sql, new { token });
                conn.Close();

                return categorias;
            }
        }
        public IEnumerable<Categoria> GetByGrupoId(string restauranteId, string[] categoriaIds)
        {
            var sql = "Select * from Categorias Where restauranteId = @restauranteId  And CategoriaId in @categoriaIds And Situacao = 1 Order By Sequencia";
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var categorias = conn.Query<Categoria>(sql, new { restauranteId, categoriaIds });
                conn.Close();

                return categorias;
            }
        }

        public void Insert(Categoria categoria)
        {
            var sql = "Insert Into Categorias(referenciaId, descricao, RestauranteToken, restauranteId, Situacao) " +
                      "Values (@referenciaId, @descricao, @restauranteToken, @restauranteId, @Situacao)";
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql,
                    new
                    {
                        referenciaId = categoria.ReferenciaId,
                        descricao = categoria.Descricao,
                        restauranteToken = categoria.RestauranteToken,
                        restauranteId = categoria.RestauranteId,
                        Situacao = 1
                    });
                conn.Close();
            }
        }
        public void Alterar(Categoria categoria)
        {
            var sql = new StringBuilder();
            sql.AppendLine("Update Categorias Set");
            sql.AppendLine("Situacao = @Situacao,");
            sql.AppendLine("Descricao = @Descricao,");
            sql.AppendLine("Sequencia = @Sequencia");
            sql.AppendLine("Where CategoriaId = @CategoriaId");

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql.ToString(),
                    new
                    {
                        categoria.CategoriaId,
                        categoria.Situacao,
                        categoria.Descricao,
                        categoria.Sequencia
                    });
                conn.Close();
            }
        }

        public IEnumerable<Categoria> GetByDescricao(string descricao, int restauranteId)
        {
            var sql = "Select * from Categorias Where Descricao like @descricao And RestauranteId = @restauranteId";
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var categoria = conn.Query<Categoria>(sql, new { descricao, restauranteId });
                conn.Close();

                return categoria;
            }
        }
        public IEnumerable<Categoria> GetById(int categoriaId)
        {
            var sql = "Select * from Categorias Where categoriaId = @categoriaId";
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var categoria = conn.Query<Categoria>(sql, new { categoriaId });
                conn.Close();

                return categoria;
            }
        }
    }
}