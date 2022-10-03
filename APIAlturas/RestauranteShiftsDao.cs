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
    public class RestauranteShiftsDao
    {
        private readonly IConfiguration _configuration;

        public RestauranteShiftsDao(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<RestauranteShifts> GetByRestauranteId(int restauranteId)
        {
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var restauranteShifts = conn.Query<RestauranteShifts>("Select * from RestauranteShifts Where RestauranteId = @restauranteId", new { restauranteId })
                    .ToList();
                conn.Close();

                return restauranteShifts;
            }
        }


        public void Adicionar(RestauranteShifts restauranteShifts)
        {
            var sql = new StringBuilder();
            sql.AppendLine("Insert Into RestauranteShifts(RestauranteShiftsId, RestauranteId, StartTime, EndTime, monday, tuesday, wednesday, thursday,");
            sql.AppendLine("friday, saturday, sunday)");
            sql.AppendLine("Values (@RestauranteShiftsId, @RestauranteId, @StartTime, @EndTime, @monday, @tuesday, @wednesday, @thursday,");
            sql.AppendLine("@friday, @saturday, @sunday)");

            var parms = new DynamicParameters();
            parms.Add("@RestauranteShiftsId", Guid.NewGuid());
            parms.Add("@RestauranteId", restauranteShifts.RestauranteId);
            parms.Add("@StartTime", restauranteShifts.StartTime);
            parms.Add("@EndTime", restauranteShifts.EndTime);
            parms.Add("@monday", restauranteShifts.monday);
            parms.Add("@tuesday", restauranteShifts.tuesday);
            parms.Add("@wednesday", restauranteShifts.wednesday);
            parms.Add("@thursday", restauranteShifts.thursday);
            parms.Add("@friday", restauranteShifts.friday);
            parms.Add("@saturday", restauranteShifts.saturday);
            parms.Add("@sunday", restauranteShifts.sunday);


            using (var conn = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql.ToString(), parms);
                conn.Close();
            }

        }

        public void Remover(string restauranteShiftsId)
        {
            using (var conn = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query("Delete From RestauranteShifts Where restauranteShiftsId = @restauranteShiftsId", new { restauranteShiftsId });
                conn.Close();
            }
        }

    }
}
