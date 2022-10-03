using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using APIAlturas.Helper;
using APIAlturas.ViewModels;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace APIAlturas
{
    public class GrupoConsumoDao
    {
        private readonly IConfiguration _configuration;

        public GrupoConsumoDao(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<GrupoConsumo> ObterTodos()
        {

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var grupoconsumo = conn
                    .Query<GrupoConsumo>("select * from GrupoConsumo")
                    .ToList();
                conn.Close();

                return grupoconsumo;
            }

        }

        public List<GrupoConsumo> ObterPorId(int id)
        {

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var grupoconsumo = conn
                    .Query<GrupoConsumo>("select * from GrupoConsumo where GrupoId = @id", new { id })
                    .ToList();
                conn.Close();

                return grupoconsumo;
            }

        }

        public void Insert(GrupoConsumo grupo)
        {
            var sql = "Insert Into GrupoConsumo(Descricao)" +
                      "Values (@Descricao)";
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql,
                    new
                    {
                        GrupoId = grupo.GrupoId,
                        Descricao = grupo.Descricao

                    }); ; ;
                conn.Close();
            }
        }

        public void Update(GrupoConsumo grupo)
        {
            var sql = "Update GrupoConsumo set Descricao=@Descricao" +
                      " where GrupoId = @GrupoId";
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql,
                    new
                    {
                        GrupoId = grupo.GrupoId,
                        Descricao = grupo.Descricao


                    });
                conn.Close();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query("Delete from GrupoConsumo where GrupoId = @id", new { id });
                conn.Close();
            }
        }
    }
}
