using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using APIAlturas.ViewModels;
using Microsoft.Extensions.Configuration;

namespace APIAlturas
{
    public class ProdutoShiftsDAO
    {
        private readonly IConfiguration _configuration;

        public ProdutoShiftsDAO(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<ProdutoShifts> GetByProdId(int produtoId)
        {
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var produtoShifts = conn.Query<ProdutoShifts>("Select * from ProdutoShifts Where produtoId = @produtoId", new { produtoId })
                    .ToList();
                conn.Close();

                return produtoShifts;
            }
        }

        public ProdutoShifts GetByGuid(Guid produtoShiftsGuid)
        {
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                string sql = "SELECT * FROM ProdutoShifts WHERE ProdutoShiftsGuid = @produtoShiftsGuid";

                conn.Open();
                ProdutoShifts prodShifts = conn.Query<ProdutoShifts>(sql, new { produtoShiftsGuid }).First();
                conn.Close();

                return prodShifts;
            }
        }

        public dynamic Create(ProdutoShifts produtoShifts)
        {
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                string sql = "INSERT INTO ProdutoShifts (" +
                    "ProdutoShiftsGuid, " +
                    "ProdutoId, " +
                    "StartTime, " +
                    "EndTime, " +
                    "Monday, " +
                    "Tuesday, " +
                    "Wednesday, " +
                    "Thursday, " +
                    "Friday, " +
                    "Saturday, " +
                    "Sunday" +
                ") VALUES (" +
                    "@produtoShiftsGuid, " +
                    "@produtoId, " +
                    "@startTime, " +
                    "@endTime, " +
                    "@monday, " +
                    "@tuesday, " +
                    "@wednesday, " +
                    "@thursday, " +
                    "@friday, " +
                    "@saturday, " +
                    "@sunday" +
                ")";
                conn.Open();
                dynamic result = conn.Query(sql, produtoShifts);
                conn.Close();
                return result;
            }
        }

        public dynamic Delete(Guid produtoShiftsGuid) {
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                string sql =
                    "DELETE FROM produtoShifts " +
                    "WHERE produtoShiftsGuid = @produtoShiftsGuid";
                conn.Open();
                dynamic result = conn.Query(sql, new { produtoShiftsGuid });
                conn.Close();

                return result;

            }
        }
    }
}
