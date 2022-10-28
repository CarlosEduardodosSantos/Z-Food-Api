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
    public class CartaoConsumoDAO
    {
        private readonly IConfiguration _configuration;

        public CartaoConsumoDAO(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<CartaoConsumo> ObterPorId(string CartaoConsumoId)
        {

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var consumo = conn
                    .Query<CartaoConsumo>("select * from CartaoConsumo where CartaoConsumoId = @CartaoConsumoId", new { CartaoConsumoId })
                    .ToList();
                conn.Close();

                return consumo;
            }

        }
        public List<CartaoConsumo> ObterPorNumeroOrNome(int RestauranteId, string Numero, string Nome)
        {

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var consumo = conn
                    .Query<CartaoConsumo>("select * from CartaoConsumo where (Numero = @Numero or Nome like '%'+@Nome+'%' or Grupo like '%'+@Nome+'%') and RestauranteId = @RestauranteId order by nome", new { RestauranteId, Numero, Nome })
                    .ToList();
                conn.Close();

                return consumo;
            }

        }

        public List<CartaoConsumo> ObterPorNr(int RestauranteId, string Numero)
        {

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var consumo = conn
                    .Query<CartaoConsumo>("select * from CartaoConsumo where Numero = @Numero and RestauranteId = @RestauranteId", new {RestauranteId,Numero })
                    .ToList();
                conn.Close();

                return consumo;
            }

        }

        public List<CartaoConsumo> ObterPorResId(int RestauranteId)
        {

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var consumo = conn
                    .Query<CartaoConsumo>("select * from CartaoConsumo where RestauranteId = @RestauranteId", new { RestauranteId })
                    .ToList();
                conn.Close();

                return consumo;
            }

        }

        public List<CartaoConsumo> ObterTodosConsu()
        {

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var consumo = conn
                    .Query<CartaoConsumo>("select * from CartaoConsumo")
                    .ToList();
                conn.Close();

                return consumo;
            }

        }


        public void Insert(CartaoConsumo consumo)
        {
            var sql = "Insert Into CartaoConsumo(CartaoConsumoId, Numero, Descricao, Valor, Validade, Cpf, Desconto, Nome, RestauranteId, SaldoAtual, Grupo, RegistradoPor)" +
                      "Values (@CartaoConsumoId, @Numero, @Descricao, @Valor, @Validade, @Cpf, @Desconto, @Nome, @RestauranteId, @SaldoAtual, @Grupo, @RegistradoPor)";
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql,
                    new
                    {
                        CartaoConsumoId = Guid.NewGuid(),
                        Numero = consumo.Numero,
                        Descricao = consumo.Descricao,
                        Valor = consumo.Valor,
                        Validade = consumo.Validade,
                        Cpf = consumo.Cpf,
                        Desconto = consumo.Desconto,
                        Nome = consumo.Nome,
                        RestauranteId = consumo.RestauranteId,
                        SaldoAtual = consumo.SaldoAtual,
                        Grupo = consumo.Grupo,
                        RegistradoPor = consumo.RegistradoPor

                    }); ;
                conn.Close();
            }
        }

        public void Update(CartaoConsumo consumo)
        {
            var sql = "Update CartaoConsumo set Numero = @Numero, Descricao =@Descricao, Valor =@Valor, Validade =@Validade, Cpf=@Cpf, Desconto=@Desconto, Nome=@Nome, RestauranteId=@RestauranteId, SaldoAtual=@SaldoAtual, Grupo=@Grupo " +
                      " where CartaoConsumoId = @CartaoConsumoId";
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql,
                    new
                    {
                        CartaoConsumoId = consumo.CartaoConsumoId,
                        Numero = consumo.Numero,
                        Descricao = consumo.Descricao,
                        Valor = consumo.Valor,
                        Validade = consumo.Validade,
                        Cpf = consumo.Cpf,
                        Desconto = consumo.Desconto,
                        Nome =consumo.Nome,
                        RestauranteId = consumo.RestauranteId,
                        SaldoAtual = consumo.SaldoAtual,
                        Grupo = consumo.Grupo,

                    });
                conn.Close();
            }
        }

        public void Delete(string id)
        {
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query("Delete from CartaoConsumo where CartaoConsumoId = @CartaoConsumoId",
                    new
                    {
                        CartaoConsumoId = Guid.Parse(id)
                    });
                conn.Close();
            }
        }

        //Mov

        public List<CartaoConsumoMov> ObterPorMovId(string CartaoConsumoMovId)
        {
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var consumo = conn
                    .Query<CartaoConsumoMov>("select * from CartaoConsumoMov where CartaoConsumoMovId = @CartaoConsumoMovId", new { CartaoConsumoMovId })
                    .ToList();
                conn.Close();

                return consumo;
            }
        }

        public List<CartaoConsumoMov> ObterMovPositiva(string CartaoConsumoId)
        {
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var consumo = conn
                    .Query<CartaoConsumoMov>("select * from CartaoConsumoMov where CartaoConsumoId = @CartaoConsumoId and TipoMov = 1 order by dataMov desc", new { CartaoConsumoId })
                    .ToList();
                conn.Close();

                return consumo;
            }
        }
        //

        public List<CartaoConsumoMov> ObterMovNegativa(string CartaoConsumoId)
        {
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var consumo = conn
                    .Query<CartaoConsumoMov>("select * from CartaoConsumoMov where CartaoConsumoId = @CartaoConsumoId and TipoMov = 2 order by dataMov desc", new { CartaoConsumoId })
                    .ToList();
                conn.Close();

                return consumo;
            }
        }

        public List<CartaoConsumoMov> ObterMovPorConsuId(string CartaoConsumoId)
        {
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var consumo = conn
                    .Query<CartaoConsumoMov>("select * from CartaoConsumoMov where CartaoConsumoId = @CartaoConsumoId order by dataMov desc", new { CartaoConsumoId })
                    .ToList();
                conn.Close();

                return consumo;
            }
        }


        public void InsertMov(CartaoConsumoMov consumo)
         {
             var sql = "Insert Into CartaoConsumoMov(CartaoConsumoMovId, CartaoConsumoId, DataMov, Historico, Saldo, TipoMov, UsuarioId, Login, Frete) " +
                       "Values (@CartaoConsumoMovId, @CartaoConsumoId, @DataMov, @Historico, @Saldo, @TipoMov, @UsuarioId, @Login, @Frete)";
             using (SqlConnection conn = new SqlConnection(
                 _configuration.GetConnectionString("ViPFood")))
             {
                 conn.Open();
                 conn.Query(sql,
                     new
                     {
                         CartaoConsumoMovId = Guid.NewGuid(),
                         CartaoConsumoId = consumo.CartaoConsumoId,
                         DataMov = DateTime.Now,
                         Historico = consumo.Historico,
                         Saldo = consumo.Saldo,
                         TipoMov = consumo.TipoMov,
                         UsuarioId = consumo.UsuarioId,
                         Login = consumo.Login,
                         Frete = consumo.Frete

                     });
                 conn.Close();
             }
         }

         public void UpdateMov(CartaoConsumoMov consumo)
         {
             var sql = "Update CartaoConsumoMov set  DataMov = @DataMov, Historico = @Historico, Saldo = @Saldo, TipoMov = @TipoMov, UsuarioId = @UsuarioId, Login = @Login" +
                       " where CartaoConsumoMovId = @CartaoConsumoMovId";
             using (SqlConnection conn = new SqlConnection(
                 _configuration.GetConnectionString("ViPFood")))
             {
                 conn.Open();
                 conn.Query(sql,
                     new
                     {
                         CartaoConsumoMovId = consumo.CartaoConsumoMovId,
                         CartaoConsumoId = consumo.CartaoConsumoId,
                         DataMov = DateTime.Now,
                         Historico = consumo.Historico,
                         Saldo = consumo.Saldo,
                         TipoMov = consumo.TipoMov,
                         UsuarioId = consumo.UsuarioId,
                         Login = consumo.Login

                     });
                 conn.Close();
             }
         }


        public void DeleteMov(string id)
        {
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query("Delete from CartaoConsumoMov where CartaoConsumoMovId = @CartaoConsumoMovId",
                    new
                    {
                        CartaoConsumoMovId = Guid.Parse(id)
                    });
                conn.Close();
            }
        }

        public void DeleteAllMovByCartId(string id)
        {
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query("Delete from CartaoConsumoMov where CartaoConsumoId = @CartaoConsumoId",
                    new
                    {
                        CartaoConsumoId = Guid.Parse(id)
                    });
                conn.Close();
            }
        }
    }
}
