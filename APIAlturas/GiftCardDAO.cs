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
    public class GiftCardDAO {
        private IConfiguration _configuration;
        public GiftCardDAO (
            IConfiguration configuration
        ) {
            _configuration = configuration;
        }

        public GiftCard GetByGuid(Guid giftCardGuid)
        {
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                string sql =
                    "SELECT * FROM GiftCards " +
                    "WHERE GiftCardGuid = @giftCardGuid";
                conn.Open();
                GiftCard giftCard = conn.Query<GiftCard>(sql, new { giftCardGuid }).First();
                return giftCard;
            }
        }

        public IEnumerable<GiftCard> GetByUserId(Guid userId)
        {
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                string sql =
                    "SELECT * FROM GiftCards " +
                    "WHERE UserId = @userId";
                conn.Open();
                IEnumerable<GiftCard> giftCards = conn.Query<GiftCard>(sql, new { userId });
                return giftCards;
            }
        }

        public GiftCard Create(GiftCard giftCard)
        {
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                string sql =
                    "INSERT INTO GiftCards (" +
                        "GiftCardGuid, " +
                        "Value, " +
                        "DataHora, " +
                        "UserId, " +
                        "RestauranteId" +
                    ") VALUES (" +
                        "@giftCardGuid, " +
                        "@value, " +
                        "@dataHora, " +
                        "@userId, " +
                        "@restauranteId" +
                    ")";
                conn.Open();
                conn.Query(sql, giftCard);
                return giftCard;
            }
        }

        public object Update(GiftCard giftCard)
        {
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                string sql =
                    "UPDATE GiftCards SET " +
                        "Value = @value, " +
                        "DataHora = @dataHora, " +
                        "UserId = @userId, " +
                        "RestauranteId = @restauranteId " +
                    "WHERE GiftCardGuid = @giftCardGuid";
                conn.Open();
                return conn.Query(sql, giftCard);
            }
        }

        public object Delete(Guid giftCardGuid)
        {
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                string sql =
                    "DELETE FROM GiftCards " +
                    "WHERE GiftCardGuid = @giftCardGuid";
                conn.Open();
                return conn.Query(sql, new { giftCardGuid });
            }
        }
    }
}
