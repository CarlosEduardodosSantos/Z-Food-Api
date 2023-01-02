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

        public List<GrupoConsumo> ObterTodos(int resId)
        {

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var grupoconsumo = conn
                    .Query<GrupoConsumo>("select * from GrupoConsumo where RestauranteId = @resId", new { resId})
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

        public List<GrupoConsumo> ObterPorNome(string nome, int resId)
        {

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var grupoconsumo = conn
                    .Query<GrupoConsumo>("select * from GrupoConsumo where Descricao = @nome and RestauranteId =@resId", new { nome, resId })
                    .ToList();
                conn.Close();

                return grupoconsumo;
            }

        }

        public void Insert(GrupoConsumo grupo)
        {
            var sql = "Insert Into GrupoConsumo(Descricao, RestauranteId, Frete)" +
                      "Values (@Descricao, @RestauranteId, @Frete)";
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql,
                    new
                    {
                        GrupoId = grupo.GrupoId,
                        Descricao = grupo.Descricao,
                        RestauranteId = grupo.RestauranteId,
                        Frete = grupo.Frete

                    }); ; ;
                conn.Close();
            }
        }

        public void Update(GrupoConsumo grupo)
        {
            var sql = "Update GrupoConsumo set Descricao=@Descricao, Frete=@Frete" +
                      " where GrupoId = @GrupoId";
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql,
                    new
                    {
                        GrupoId = grupo.GrupoId,
                        Descricao = grupo.Descricao,
                        Frete = grupo.Frete

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

        public void AtualizarGrupoNota(string grupo, int resId, bool frete)
        {
            var sql = "Update CartaoConsumo set Frete = @Frete" +
                      " where Grupo = @Grupo and RestauranteId = @resId";
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql,
                    new
                    {
                        Frete = frete,
                        Grupo = grupo,
                        resId = resId
                    });
                conn.Close();
            }
        }
    }
}
