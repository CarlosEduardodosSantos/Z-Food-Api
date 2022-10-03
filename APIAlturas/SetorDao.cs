using System.Collections.Generic;
using System.Data.SqlClient;
using APIAlturas.ViewModels;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace APIAlturas
{
    public class SetorDao
    {
        private readonly IConfiguration _configuration;

        public SetorDao(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<Setor> ObterTodos()
        {
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                var sql = "Select * From Setores Where Situacao = 1 Order By Sequencia";
                conn.Open();

                var setores = conn.Query<Setor>(sql);
                
                conn.Close();

                return setores;
            }
        }
    }
}