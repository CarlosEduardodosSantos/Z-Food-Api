using System;
using System.Data.SqlClient;
using System.Linq;
using APIAlturas.ViewModels;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace APIAlturas
{
    public class PasswordChangeRequestDao
    {
        private readonly IConfiguration _configuration;

        public PasswordChangeRequestDao(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Adicionar(PasswordChangeRequest passwordChangeRequest)
        {
            using (var conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                var sql =
                    "Insert Into passwordChangeRequests(PasswordChangeRequestId, UserId, DataHoraExpira, Reset) Values(@PasswordChangeRequestId, @UserId, @DataHoraExpira, @Reset) ";

                var parms = new DynamicParameters();
                parms.Add("@PasswordChangeRequestId", passwordChangeRequest.PasswordChangeRequestId);
                parms.Add("@UserId", passwordChangeRequest.UserId);
                parms.Add("@DataHoraExpira", passwordChangeRequest.DataHoraExpira);
                parms.Add("@Reset", passwordChangeRequest.Reset);

                conn.Open();
                conn.Query(sql, parms);
                conn.Close();
            }
        }

        public PasswordChangeRequest GetById(Guid id)
        {
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                var sql = "Select * From passwordChangeRequests Where passwordChangeRequestId = @id";
                conn.Open();
                var pass = conn.Query<PasswordChangeRequest>(sql, new {id}).FirstOrDefault();
                conn.Close();

                return pass;
            }
        }
    }
}