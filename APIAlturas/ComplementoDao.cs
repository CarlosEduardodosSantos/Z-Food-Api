using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using APIAlturas.ViewModels;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace APIAlturas
{
    public class ComplementoDao
    {
        private readonly IConfiguration _configuration;

        public ComplementoDao(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Complemento GetById(int complementoId)
        {
            var sql = "Select * from Complementos Where complementoId = @complementoId";

            using (SqlConnection conexao = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                return conexao.Query<Complemento>(sql, new { complementoId }).FirstOrDefault();
            }
        }

        public IEnumerable<Complemento> GetByCategoriaId(string categoriaId)
        {
            var sql = "Select * from Complementos Where categoriaId = @categoriaId";

            using (SqlConnection conexao = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                return conexao.Query<Complemento>(sql, new { categoriaId });
            }
        }

        public void Insert(Complemento complemento)
        {
            var sql = "Insert Into Complementos(referenciaId, restauranteId, descricao, categoriaId, valor) " +
                      "Values (@referenciaId, @restauranteId, @descricao, @categoriaId, @valor)";
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql,
                    new
                    {
                        referenciaId = complemento.ReferenciaId,
                        restauranteId = complemento.RestauranteId,
                        descricao = complemento.Descricao,
                        CategoriaId = complemento.CategoriaId,
                        valor = complemento.Valor
                    });
                conn.Close();
            }
        }
        public void Delete(Complemento complemento)
        {
            var sql = "Delete Complementos Where complementoId = @complementoId And restauranteId = @restauranteId";
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql,
                    new
                    {
                        complementoId = complemento.ComplementoId,
                        restauranteId = complemento.RestauranteId
                    });
                conn.Close();
            }
        }


    }
}