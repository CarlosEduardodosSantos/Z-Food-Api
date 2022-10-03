using System;
using System.Reflection;
using System.Collections.Generic;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIAlturas.ViewModels;

namespace APIAlturas
{
    public class FlyerDAO
    {
        private IConfiguration _configuration;
        public FlyerDAO(
            IConfiguration configuration
        ) {
            _configuration = configuration;
        }

        public Flyer GetById(Guid flyerGuid)
        {
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                string sql =
                    "SELECT * FROM flyer " +
                    "WHERE flyerGuid = @flyerGuid";
                Flyer flyer = conn.Query<Flyer>(sql, new { flyerGuid }).First();

                if (flyer.ProdutoId != 0)
                {
                    string sqlProd =
                        "SELECT * FROM produtos WHERE produtoId = @produtoId";
                    flyer.Produto = conn.Query<Produto>(sqlProd, new { produtoId = flyer.ProdutoId }).First();
                }

                return flyer;
            }
        }

        public IEnumerable<Flyer> getByRestId(int restId)
        {
            using (SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("ViPFood")))
            {
                string sql =
                "SELECT * FROM flyer " +
                "WHERE restauranteId = @restId";

                conn.Open();
                IEnumerable<Flyer> flyers = conn.Query<Flyer>(sql, new { restId });
                foreach (Flyer flyer in flyers)
                {
                    flyer.Produto = GetById(flyer.FlyerGuid).Produto;
                }

                return flyers;
            }
        }

        public object Update(Flyer flyer)
        {
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                PropertyInfo[] propertyList = flyer.GetType().GetProperties();
                List<PropertyInfo> propertiesToUpdate = new List<PropertyInfo>();

                StringBuilder sql = new StringBuilder("UPDATE flyer SET ");
                DynamicParameters queryParams = new DynamicParameters();

                foreach(PropertyInfo prop in propertyList)
                {
                    Type t = prop.PropertyType;
                    if (t.IsPrimitive || t == typeof(string) || t == typeof(decimal) || t == typeof(Guid))
                    {
                        sql.AppendLine(prop.Name + " = @" + prop.Name + " , ");
                        queryParams.Add("@" + prop.Name, prop.GetValue(flyer));
                    }
                }
                sql = sql.Remove(sql.Length - 5, 2);
                System.Diagnostics.Debug.Write(sql);
                sql.AppendLine("WHERE flyerGuid = @flyerGuid");
              
                conn.Open();
                return conn.Query(sql.ToString(), queryParams);

            }
        }

        public object Create(Flyer flyer)
        {
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                PropertyInfo[] propertyList = flyer.GetType().GetProperties();
                List<PropertyInfo> properties = new List<PropertyInfo>();

                string sql = 
                    "INSERT INTO flyer " +
                    "(" +
                        "flyerGuid," +
                        "title," +
                        "details," +
                        "picture," +
                        "restauranteId," +
                        "produtoId" +
                    ") " +
                    "VALUES(" +
                        "@flyerGuid," +
                        "@title," +
                        "@details," +
                        "@picture," +
                        "@restauranteId," +
                        "@produtoId" +
                    ")";

                conn.Open();
                conn.Query(sql, flyer);
                return flyer;
            }
        }

        public object Delete(Guid flyerGuid)
        {
            using(SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("ViPFood")))
            {
                string sql =
                    "DELETE FROM flyer WHERE flyerGuid = @flyerGuid";
                conn.Open();
                return conn.Query(sql, new { flyerGuid });
            }
        }
    }

}
