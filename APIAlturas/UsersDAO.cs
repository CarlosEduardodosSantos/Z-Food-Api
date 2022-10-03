using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;
using Dapper;

namespace APIAlturas
{
    public class UsersDAO
    {
        private readonly IConfiguration _configuration;

        public UsersDAO(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public User Find(string email, int restauranteId)
        {
            using (SqlConnection conexao = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                return conexao.QueryFirstOrDefault<User>(
                    "SELECT * FROM dbo.Users " +
                    "WHERE email = @email And (Isnull(restauranteId,0) = 0 Or restauranteId = @restauranteId)", new { email, restauranteId });
            }
        }

        public List<User> Find(int restauranteId)
        {
            using (SqlConnection conexao = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                return conexao.Query<User>("SELECT * FROM dbo.Users WHERE restauranteId = @restauranteId", new { restauranteId }).ToList();
            }
        }
        public User GetByPlayerId(string playerId)
        {
            using (SqlConnection conexao = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                return conexao.QueryFirstOrDefault<User>(
                    "SELECT * FROM dbo.Users " +
                    "WHERE playersID = @playerId", new { playerId });
            }
        }
        public User GetById(string id)
        {
            using (SqlConnection conexao = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                return conexao.QueryFirstOrDefault<User>(
                    "SELECT * FROM dbo.Users " +
                    "WHERE userID = @id", new { id });
            }
        }
        public void Registar(User usuario)
        {
            using (SqlConnection conexao = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                var sql = new StringBuilder();
                sql.AppendLine("Insert Into Users(UserID, AccessKey, Email, Nome, Sobrenome, Fone, Senha, playersID,");
                sql.AppendLine("imagem, Facebook, RestauranteId)Values (");
                sql.AppendLine("@UserID, @AccessKey, @Email, @Nome, @Sobrenome, @Fone, @Password, @PlayersId,");
                sql.AppendLine("@imagem, @Facebook, @RestauranteId)");
                var param = new DynamicParameters();
                param.Add("@UserID", Guid.NewGuid());
                param.Add("@AccessKey", "");
                param.Add("@Email", usuario.Email);
                param.Add("@Nome", usuario.Nome);
                param.Add("@Sobrenome", usuario.Sobrenome);
                param.Add("@Fone", usuario.Fone);
                param.Add("@Password", usuario.Senha);
                param.Add("@PlayersId", usuario.PlayersId);
                param.Add("@imagem", usuario.Imagem);
                param.Add("@Facebook", usuario.Facebook);
                param.Add("@RestauranteId", usuario.RestauranteId);

                conexao.Query(sql.ToString(), param);
            }
        }
        public void Alterar(User usuario)
        {
            using (SqlConnection conexao = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                var sql = new StringBuilder();
                sql.AppendLine("Update Users Set ");
                sql.AppendLine("Nome = @Nome,");
                sql.AppendLine("Sobrenome = @Sobrenome,");
                sql.AppendLine("Fone = @Fone,");
                sql.AppendLine("Senha = @Password,");
                sql.AppendLine("imagem = @imagem,");
                sql.AppendLine("playersID = @playersID,");
                sql.AppendLine("RestauranteId = @RestauranteId,");
                sql.AppendLine("Facebook = @Facebook");
                sql.AppendLine("Where  UserID = @UserID");

                var param = new DynamicParameters();
                param.Add("@UserID",usuario.UserID);
                param.Add("@Nome", usuario.Nome);
                param.Add("@Sobrenome", usuario.Sobrenome);
                param.Add("@Fone", usuario.Fone);
                param.Add("@Password", usuario.Senha);
                param.Add("@imagem", usuario.Imagem);
                param.Add("@PlayersId", usuario.PlayersId);
                param.Add("@RestauranteId", usuario.RestauranteId);
                param.Add("@Facebook", usuario.Facebook);

                conexao.Query(sql.ToString(), param);
            }
        }
    }
}