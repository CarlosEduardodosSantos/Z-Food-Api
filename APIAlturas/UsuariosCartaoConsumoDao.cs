using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using APIAlturas.ViewModels;
using Microsoft.Extensions.Configuration;
using System.Text;
using System;

namespace APIAlturas
{
    public class UsuariosCartaoConsumoDao
    {
        private readonly IConfiguration _configuration;

        public UsuariosCartaoConsumoDao(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<UsuariosCartaoConsumoModel> ObterUserPorSenha(string Login, string Senha)
        {

            using (var conn = new SqlConnection(
                   _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var login = conn
                    .Query<UsuariosCartaoConsumoModel>("select * from UsuariosCartaoConsumo where Login = @Login and Senha = @Senha", new { Login, Senha })
                    .ToList();
                conn.Close();

                return login;
            }

        }

        public List<UsuariosCartaoConsumoModel> ObterUserPorCodigo(int UsuarioId, int ResId)
        {

            using (var conn = new SqlConnection(
                    _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var login = conn
                    .Query<UsuariosCartaoConsumoModel>("select * from UsuariosCartaoConsumo where UsuarioId = @UsuarioId and RestauranteId = @ResId", new { UsuarioId, ResId })
                    .ToList();
                conn.Close();

                return login;
            }

        }

        public List<UsuariosCartaoConsumoModel> ObterTodos(int ResId)
        {

            using (var conn = new SqlConnection(
                   _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var login = conn
                    .Query<UsuariosCartaoConsumoModel>("select * from UsuariosCartaoConsumo where RestauranteId = @ResId order by Login", new { ResId})
                    .ToList();
                conn.Close();

                return login;
            }

        }

        public void Insert(UsuariosCartaoConsumoModel usuario)
        {
            var sql = "Insert Into UsuariosCartaoConsumo(Login, Senha, Tipo, RestauranteId)" +
                      "Values (@Login, @Senha, @Tipo, @RestauranteId)";
            using (var conn = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql,
                    new
                    {
                        Login = usuario.Login,
                        Senha = usuario.Senha,
                        Tipo = usuario.Tipo,
                        RestauranteId = usuario.RestauranteId
                    });
                conn.Close();
            }
        }

        public void Update(UsuariosCartaoConsumoModel usuario)
        {
            var sql = "Update UsuariosCartaoConsumo set Login = @Login, Senha =@Senha, Tipo =@Tipo, RestauranteId =@RestauranteId" +
                      " where UsuarioId = @UsuarioId";
            using (var conn = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql,
                    new
                    {
                        UsuarioId = usuario.UsuarioId,
                        Login = usuario.Login,
                        Senha = usuario.Senha,
                        Tipo = usuario.Tipo,
                        RestauranteId = usuario.RestauranteId
                    });
                conn.Close();
            }
        }

        public void Delete(int usuarioid)
        {
            using (var conn = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query("Delete from UsuariosCartaoConsumo where UsuarioId = @UsuarioId",
                    new
                    {
                        UsuarioId = usuarioid
                    });
                conn.Close();
            }
        }

    }
}
