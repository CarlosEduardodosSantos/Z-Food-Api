using System.Collections.Generic;
using System.Data.SqlClient;
using APIAlturas.ViewModels;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace APIAlturas
{
    public class LocalizacaoDao
    {
        private readonly IConfiguration _configuration;

        public LocalizacaoDao(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<Estado> ObterEstados()
        {
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var estados = conn.Query<Estado>("Select * From Estados");
                conn.Close();

                return estados;
            }
        }
        public IEnumerable<Cidade> ObterCidades(string estado)
        {
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var cidades = conn.Query<Cidade>("Select * From Cidades Where estado Like @estado", new { estado });
                conn.Close();

                return cidades;
            }
        }
        public IEnumerable<Bairro> ObterBairros(string estado, string cidade)
        {
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var bairros = conn.Query<Bairro>("Select * From Bairros Where estado Like @estado And cidade Like @cidade",
                    new {estado, cidade});
                conn.Close();

                return bairros;
            }
        }
    }
}