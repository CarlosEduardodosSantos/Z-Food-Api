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
    public class FarmaciasDao
    {
        private readonly IConfiguration _configuration;

        public FarmaciasDao(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<Farmacias> ObterPorId(string Cnpj)
        {

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("Pharma")))
            {
                conn.Open();
                var farmacia = conn
                    .Query<Farmacias>("select * from Conexoes where Cnpj = @Cnpj", new { Cnpj })
                    .ToList();
                conn.Close();

                return farmacia;
            }

        }


        public void Insert(ReceitasPharma receita)
        {
            var sql = "Insert Into ReceitasPharma(Nome, Cpf, Telefone, Receita)" +
                      "Values (@Nome, @Cpf, @Telefone, @Receita)";
            using (SqlConnection conn = new SqlConnection(Farmacias.Conexao))
            {
                conn.Open();
                conn.Query(sql,
                    new
                    {
                        Nome = receita.Nome,
                        Cpf = receita.Cpf,
                        Telefone = receita.Telefone,
                        Receita = receita.Receita
                    }); ;
                conn.Close();
            }
        }





    }
}
