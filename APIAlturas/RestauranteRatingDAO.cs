using APIAlturas.ViewModels;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace APIAlturas
{
    public class RestauranteRatingDAO
    {
        private IConfiguration _configuration;
        public RestauranteRatingDAO (
            IConfiguration configuration
        ) {
            _configuration = configuration;
        }

        public RestauranteRating GetByGuid(Guid restRatingGuid)
        {
            using(SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                string sql =
                    "SELECT * FROM RestauranteRating " +
                    "WHERE RestauranteRatingGuid = @restRatingGuid";

                conn.Open();
                RestauranteRating restRating = conn.Query<RestauranteRating>(sql, new { restRatingGuid }).First();
                return restRating;
            }
        }

        public IEnumerable<RestauranteRating> GetByRestId(int restId)
        {
            using(SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                string sql =
                    "SELECT * FROM RestauranteRating " +
                    "WHERE RestauranteId = @restId";
                conn.Open();
                IEnumerable<RestauranteRating> restRatings = conn.Query<RestauranteRating>(sql, new { restId });
                return restRatings;
            }
        }

        public RestauranteRating Create(RestauranteRating restRating)
        {
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                string sql =
                    "INSERT INTO RestauranteRating (" +
                        "RestauranteRatingGuid, " +
                        "Name, " +
                        "Phone, " +
                        "Value, " +
                        "Suggestion, " +
                        "DataHora, " +
                        "RestauranteId" +
                    ") VALUES (" +
                        "@restauranteRatingGuid, " +
                        "@name, " +
                        "@phone, " +
                        "@value, " +
                        "@suggestion, " +
                        "@dataHora, " +
                        "@restauranteId" +
                    ")";

                conn.Open();
                conn.Query(sql, restRating);
                return restRating;
            }
        } 

        public object Update(RestauranteRating restRating)
        {
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                string sql = 
                    "UPDATE RestauranteRating " +
                    "SET " +
                        "RestauranteRatingGuid = @restauranteRatingGuid, " +
                        "Name = @name, " +
                        "Phone = @phone, " +
                        "Value = @value, " +
                        "Suggestion = @suggestion, " +
                        "DataHora = @dataHora, " +
                        "RestauranteId = @restauranteId " +
                    "WHERE RestauranteRatingGuid = @restauranteRatingGuid";

                conn.Open();
                return conn.Query(sql, restRating);
            }
        }

        public object Delete(Guid restRatingGuid)
        {
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                string sql =
                    "DELETE FROM RestauranteRating " +
                    "WHERE RestauranteRatingGuid = @restRatingGuid";

                conn.Open();
                return conn.Query(sql, new { restRatingGuid });
            }
        }
    }
}
