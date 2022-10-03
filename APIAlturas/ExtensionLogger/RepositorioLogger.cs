using System.Data.SqlClient;
using APIAlturas.ViewModels;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace APIAlturas.ExtensionLogger
{
    public class RepositorioLogger
    {
        private readonly IConfiguration _configuration;

        public RepositorioLogger(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public bool InsertLog(LogEvento log)
        {
            var command = $@"INSERT INTO [dbo].[EventLog] ([Id],[LogLevel],[Message],[CreatedTime]) VALUES (@Id, @LogLevel, @Message, @CreatedTime)";
            var paramList = new DynamicParameters();
            paramList.Add("@Id", log.Id);
            paramList.Add("@LogLevel", log.LogLevel);
            paramList.Add("@Message", log.Message);
            paramList.Add("@CreatedTime", log.CreatedTime);


            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(command, paramList);
                conn.Close();

                return true;
            }
            
        }
    }
}