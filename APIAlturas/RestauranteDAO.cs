using System;
using System.Collections.Generic;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using APIAlturas.ViewModels;

namespace APIAlturas
{
    public class RestauranteDAO
    {
        private IConfiguration _configuration;

        public RestauranteDAO(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void AlterarRestaurante(Restaurante restaurante)
        {
            var sql = new StringBuilder();
            sql.AppendLine("Update Restaurantes Set");
            sql.AppendLine("TempoEntrega = @TempoEntrega,");
            sql.AppendLine("AbreAs = @AbreAs,");
            sql.AppendLine("FechaAs = @FechaAs,");
            sql.AppendLine("AceitaRetira = @AceitaRetira,");
            sql.AppendLine("PedidoMinimo = @PedidoMinimo,");
            sql.AppendLine("Zimmer = @Zimmer,");
            sql.AppendLine("Imagem = @Imagem");
            sql.AppendLine("Where RestauranteId = @RestauranteId");

            var param = new DynamicParameters();
            param.Add("@RestauranteId", restaurante.RestauranteId);
            param.Add("@TempoEntrega", restaurante.TempoEntrega);
            param.Add("@AbreAs", restaurante.AbreAs);
            param.Add("@FechaAs", restaurante.FechaAs);
            param.Add("@AceitaRetira", restaurante.AceitaRetira);
            param.Add("@PedidoMinimo", restaurante.PedidoMinimo);
            param.Add("@Zimmer", restaurante.Zimmer);
            param.Add("@Imagem", restaurante.Imagem);

            using (var conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql.ToString(), param);
                conn.Close();
            }
        }

        public IEnumerable<Restaurante>  Find(string cep, string setorId)
        {
            using (SqlConnection conexao = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                var sql =
                    "select *, " +
                        "dbo.fnIsUsaGrupo(restauranteId) as IsGrupoPage," +
                        "dbo.fnEstaAtendendo(restauranteId) as IsOperation," +
                        "[dbo].[fnValorEntrega](restauranteId, @cep) as valorEntrega, " +
                        "[dbo].[fnAtendeLocal](restauranteId, @cep) as AtendeLocal " +
                    "From restaurantes " +
                    "Where restauranteId in (select restauranteId From Restaurantes Where Situacao in (0,1) And SituacaoApp = 1) " +
                    "And (@setorId = '00000000-0000-0000-0000-000000000000' Or Cast(SetorId as Varchar(50)) = @setorId)" +
                    " Order By avaliacaoRating desc, nome";
                return conexao.Query<Restaurante>(sql, new {cep, setorId});
            }
        }
        public IEnumerable<Restaurante> GetByToken(string cep, string token)
        {
            using (SqlConnection conexao = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                var sql =
                    "select *, " +
                     "dbo.fnIsUsaGrupo(restauranteId) as IsGrupoPage," +
                        "dbo.fnEstaAtendendo(restauranteId) as IsOperation," +
                        "[dbo].[fnValorEntrega](restauranteId, @cep) as valorEntrega, " +
                        "[dbo].[fnAtendeLocal](restauranteId, @cep) as AtendeLocal " +
                    "From restaurantes " +
                    "Where token = @token And SituacaoApp in (1,2)" +
                    " Order By avaliacaoRating desc, nome";
                return conexao.Query<Restaurante>(sql, new { cep, token = Guid.Parse(token) });
            }
        }
        public IEnumerable<Restaurante> FindWishlist(string usuarioId, string cep)
        {
            using (SqlConnection conexao = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                var sql =
                    "select *, " +
                     "dbo.fnIsUsaGrupo(restauranteId) as IsGrupoPage," +
                        "dbo.fnEstaAtendendo(restauranteId) as IsOperation," +
                        "[dbo].[fnValorEntrega](restauranteId, @cep) as valorEntrega, " +
                        "[dbo].[fnAtendeLocal](restauranteId, @cep) as AtendeLocal " +
                    "From restaurantes " +
                    "Inner Join Wishlists On Restaurantes.restauranteId = Wishlists.RestauranteId " +
                    "And Wishlists.UsuarioId = @usuarioId " +
                    "Where restauranteId in (select restauranteId From AtendimentoLocais Where faixaCep like substring(@cep,1,5))" +
                    " Order By avaliacaoRating desc, nome";
                return conexao.Query<Restaurante>(sql, new { usuarioId, cep });
            }
        }

        public void AddWishlist(Wishlist wishlist)
        {
            using (SqlConnection conexao = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                var sql = new StringBuilder();
                sql.AppendLine("Insert Into Wishlists(UsuarioId, RestauranteId)");
                sql.AppendLine("Values (");
                sql.AppendLine("@UsuarioId, @RestauranteId)");

                var param = new DynamicParameters();
                param.Add("@UsuarioId", wishlist.UsuarioId);
                param.Add("@RestauranteId", wishlist.RestauranteId);

                conexao.Query(sql.ToString(), param);
            }
        }
        public void RemoveWishlist(Wishlist wishlist)
        {
            using (SqlConnection conexao = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                var sql = new StringBuilder();
                sql.AppendLine("Delete From Wishlists Where UsuarioId = @usuarioId And RestauranteId = @restauranteId");

                var param = new DynamicParameters();
                param.Add("@UsuarioId", wishlist.UsuarioId);
                param.Add("@RestauranteId", wishlist.RestauranteId);

                conexao.Query(sql.ToString(), param);
            }
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
        public Restaurante FindByToken(string token)
        {
            using (SqlConnection conexao = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                var sql =
                    "SELECT *, " +
                        "dbo.fnIsUsaGrupo(restauranteId) as IsGrupoPage, " +
                        "dbo.fnEstaAtendendo(restauranteId) as IsOperation " +
                    "FROM restaurantes " +
                    "Where Token = @token";
                return conexao.Query<Restaurante>(sql, new { token }).FirstOrDefault();
            }
        }
        public Restaurante FindById(int restauranteId)
        {
            using (SqlConnection conexao = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                var sql =
                    "SELECT *, " +
                        "dbo.fnIsUsaGrupo(restauranteId) as IsGrupoPage," +
                        "dbo.fnEstaAtendendo(restauranteId) as IsOperation " +
                    "FROM restaurantes " + 
                    "Where restauranteId = @restauranteId And SituacaoApp in (1,2)";
                return conexao.Query<Restaurante>(sql, new { restauranteId }).FirstOrDefault();
            }
        }
        public Restaurante FindByCnpj(string cnpj)
        {
            using (SqlConnection conexao = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                var sql =
                    "SELECT *, " +
                        "dbo.fnIsUsaGrupo(restauranteId) as IsGrupoPage," +
                        "dbo.fnEstaAtendendo(restauranteId) as IsOperation " +
                    "FROM restaurantes " +
                    "WHERE Cnpj = @cnpj " +
                    "ORDER BY avaliacaoRating DESC, nome";
                return conexao.Query<Restaurante>(sql, new { cnpj }).FirstOrDefault();
            }
        }
        public string ObterSituacao(string token)
        {
            using (SqlConnection conexao = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                var sql = "select Situacao From Restaurantes Where Token = @token And SituacaoApp in (1,2)";
                var situacao = conexao.Query<int>(sql, new { token }).FirstOrDefault();
                return ((RestauranteSituacaoEnum) situacao).ToString();

            }
        }

        public void AlterarSituacao(string token, RestauranteSituacaoEnum situacao)
        {
            using (SqlConnection conexao = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                var sql = "Update Restaurantes Set Situacao = @situacao Where Token = @token";
                conexao.Query(sql, new { token, situacao = (int)situacao });
            }
        }
        public void AdministracaoDePoolin()
        {
            var sql = "Update Restaurantes Set Situacao = 0 Where dateadd(MINUTE, 2, DataHoraPooling) <= getdate() And Situacao = 1";
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql);
                conn.Close();
            }
        }

        public IEnumerable<LocalAtendimento> ObterLocalAtendimentoPorToken(int restauranteId)
        {
            using (var conexao = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                var sql = "select faixaInicial = FaixaCep,  * From AtendimentoLocais Where restauranteId = @restauranteId";
                return conexao.Query<LocalAtendimento>(sql, new { restauranteId });
            }
        }

        public void AdicionarLocalAtendimento(LocalAtendimento localAtendimento)
        {
            using (var conexao = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                var sql = new StringBuilder();
                sql.AppendLine("Insert Into AtendimentoLocais(descricao, restauranteId, faixaCep, faixaFinal, valorEntrega)");
                sql.AppendLine("Values (@descricao, @restauranteId, @faixaCep, @faixaFinal, @valorEntrega)");
                var param = new DynamicParameters();
                param.Add("@descricao", localAtendimento.descricao);
                param.Add("@restauranteId", localAtendimento.restauranteId);
                param.Add("@faixaCep", localAtendimento.faixaInicial);
                param.Add("@faixaFinal", localAtendimento.faixaFinal);
                param.Add("@valorEntrega", localAtendimento.valorEntrega);

                conexao.Query(sql.ToString(), param);
            }
        }

        public void AlterarLocalAtendimento(LocalAtendimento localAtendimento)
        {
            using (var conexao = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                var sql = new StringBuilder();
                sql.AppendLine("Update AtendimentoLocais Set");
                sql.AppendLine("faixaCep = @faixaCep,");
                sql.AppendLine("faixaFinal = @faixaFinal,");
                sql.AppendLine("valorEntrega = @valorEntrega");
                sql.AppendLine("Where restauranteId = @restauranteId And descricao = @descricao");

                var param = new DynamicParameters();
                param.Add("@descricao", localAtendimento.descricao);
                param.Add("@restauranteId", localAtendimento.restauranteId);
                param.Add("@faixaCep", localAtendimento.faixaInicial);
                param.Add("@faixaFinal", localAtendimento.faixaFinal);
                param.Add("@valorEntrega", localAtendimento.valorEntrega);

                conexao.Query(sql.ToString(), param);
            }
        }

        public void DeleteLocalAtendimento(int atendimentoLocalId)
        {
            using (var conexao = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                conexao.Open();
                conexao.Query("Delete From AtendimentoLocais Where atendimentoLocalId = @atendimentoLocalId",
                    new {atendimentoLocalId});
                conexao.Close();
            }
        }

        public void AlterarRestauranteValorEst(Restaurante restaurante)
        {
            var sql = new StringBuilder();
            sql.AppendLine("Update Restaurantes Set");
            sql.AppendLine("ValorEst = @ValorEst");
            sql.AppendLine("Where RestauranteId = @RestauranteId");

            var param = new DynamicParameters();
            param.Add("@RestauranteId", restaurante.RestauranteId);
            param.Add("@ValorEst", restaurante.ValorEst);

            using (var conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql.ToString(), param);
                conn.Close();
            }
        }
    }
}
