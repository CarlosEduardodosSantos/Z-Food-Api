using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using APIAlturas.ViewModels;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace APIAlturas
{
    public class PedidoUserZimmerDao
    {
        private readonly IConfiguration _configuration;

        public PedidoUserZimmerDao(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Insert(UserZimmer userZimmer, Guid pedidoGuid)
        {
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();

                var sqlBuscar = "Select * From UserZimmer Where Fone = @fone Or Email = @email";
                var zimmerExistes = conn.Query<UserZimmer>(sqlBuscar, new {fone = userZimmer.Fone, email = userZimmer.Email}).FirstOrDefault();

                if (zimmerExistes == null)
                {
                    var sqlZimmer = new StringBuilder();
                    sqlZimmer.AppendLine(
                        "Insert Into UserZimmer(UserZimmerId, Nome, Fone, Email, Logradouro, Bairro, Cidade, Estado, CEP, RestauranteId, AceitaMarketing)");
                    sqlZimmer.AppendLine(
                        "Values(@UserZimmerId, @Nome, @Fone, @Email, @Logradouro, @Bairro, @Cidade, @Estado, @CEP, @RestauranteId, @AceitaMarketing)");
                    var zimmerParms = new DynamicParameters();
                    userZimmer.UserZimmerId = Guid.NewGuid();

                    zimmerParms.Add("@UserZimmerId", userZimmer.UserZimmerId);
                    zimmerParms.Add("@Nome", userZimmer.Nome);
                    zimmerParms.Add("@Fone", userZimmer.Fone);
                    zimmerParms.Add("@Email", userZimmer.Email);
                    zimmerParms.Add("@Logradouro", userZimmer.Logradouro);
                    zimmerParms.Add("@Bairro", userZimmer.Bairro);
                    zimmerParms.Add("@Cidade", userZimmer.Cidade);
                    zimmerParms.Add("@Estado", userZimmer.Estado);
                    zimmerParms.Add("@CEP", userZimmer.Cep);
                    zimmerParms.Add("@RestauranteId", userZimmer.RestauranteId);
                    zimmerParms.Add("@AceitaMarketing", userZimmer.AceitaMarketing);

                    conn.Execute(sqlZimmer.ToString(), zimmerParms);

                }
                else
                {
                    userZimmer.UserZimmerId = zimmerExistes.UserZimmerId;
                }

                var insertRelacao =
                    "Insert Into PedidoUserZimmer(PedidoUserZimmerId, PedidoGuid, UserZimmerId) Values (@PedidoUserZimmerId, @PedidoGuid, @UserZimmerId)";
                conn.Execute(insertRelacao, new
                {
                    PedidoUserZimmerId = Guid.NewGuid(),
                    PedidoGuid = pedidoGuid,
                    UserZimmerId = userZimmer.UserZimmerId
                });

                conn.Close();

            }
        }
    }
}