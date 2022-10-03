using System;
using System.Data.SqlClient;
using System.Text;
using APIAlturas.ViewModels;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace APIAlturas
{
    public class PlayersClickDAO
    {
        private readonly IConfiguration _configuration;

        public PlayersClickDAO(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void AddPlayerRestaurante(PlayersClick playersClick)
        {
            using (var conexao = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                var sql = new StringBuilder();
                sql.AppendLine("Insert Into PlayersRestaurante(PlayerRestauranteId, playersID, DataHora, RestauranteId)");
                sql.AppendLine("Values (@PlayerRestauranteId, @playersID, @DataHora, @RestauranteId)");
                var param = new DynamicParameters();
                param.Add("@PlayerRestauranteId", Guid.NewGuid());
                param.Add("@playersID", playersClick.PlayersId);
                param.Add("@DataHora", DateTime.Now);
                param.Add("@RestauranteId", playersClick.RestauranteId);

                conexao.Query(sql.ToString(), param);
            } 

        }
        public void AddPlayerProduto(PlayersClick playersClick)
        {
            using (var conexao = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                var sql = new StringBuilder();
                sql.AppendLine("Insert Into PlayersProduto(PlayerProdutoId, playersID, DataHora, ProdutoId)");
                sql.AppendLine("Values (@PlayerProdutoId, @playersID, @DataHora, @ProdutoId)");
                var param = new DynamicParameters();
                param.Add("@PlayerProdutoId", Guid.NewGuid());
                param.Add("@playersID", playersClick.PlayersId);
                param.Add("@DataHora", DateTime.Now);
                param.Add("@ProdutoId", playersClick.ProdutoId);

                conexao.Query(sql.ToString(), param);
            }

        }
    }
}