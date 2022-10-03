using System.Collections.Generic;
using System.Data.SqlClient;
using APIAlturas.ViewModels;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace APIAlturas
{
    public class ZcashMovimentacaoDao
    {
        private readonly IConfiguration _configuration;

        public ZcashMovimentacaoDao(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<ZcashMovimentacao> ObterPorUsuarioId(string usuarioId)
        {
            using (var conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                var sql = "select * from ZcashMovimentacao Where UsuarioId = @usuarioId";
                conn.Open();
                var movs = conn.Query<ZcashMovimentacao>(sql, new {usuarioId});
                conn.Close();
                return movs;
            }
        }
    }
}