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
                    .Query<CartaoConsumo>("select * from CartaoConsumo where RestauranteId = @RestauranteId order by nome", new { RestauranteId })
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
            var sql = "Insert Into CartaoConsumo(CartaoConsumoId, Numero, Descricao, Valor, Validade, Cpf, Desconto, Nome, RestauranteId, SaldoAtual, Grupo, RegistradoPor, Frete)" +
                      "Values (@CartaoConsumoId, @Numero, @Descricao, @Valor, @Validade, @Cpf, @Desconto, @Nome, @RestauranteId, @SaldoAtual, @Grupo, @RegistradoPor, @Frete)";
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
                        RegistradoPor = consumo.RegistradoPor,
                        Frete = consumo.Frete

                    }); ;
                conn.Close();
            }
        }

        public void Update(CartaoConsumo consumo)
        {
            var sql = "Update CartaoConsumo set Numero = @Numero, Descricao =@Descricao, Valor =@Valor, Validade =@Validade, Cpf=@Cpf, Desconto=@Desconto, Nome=@Nome, RestauranteId=@RestauranteId, SaldoAtual=@SaldoAtual, Grupo=@Grupo, Frete=@Frete " +
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
                        RegistradoPor = consumo.RegistradoPor,
                        Frete = consumo.Frete
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
             var sql = "Insert Into CartaoConsumoMov(CartaoConsumoMovId, CartaoConsumoId, DataMov, Historico, Saldo, TipoMov, UsuarioId, Login, Metodo) " +
                       "Values (@CartaoConsumoMovId, @CartaoConsumoId, @DataMov, @Historico, @Saldo, @TipoMov, @UsuarioId, @Login, @Metodo)";
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
                         Login = consumo.Login,
                         Metodo = consumo.Metodo

                     });
                 conn.Close();
             }
         }

         public void UpdateMov(CartaoConsumoMov consumo)
         {
             var sql = "Update CartaoConsumoMov set  DataMov = @DataMov, Historico = @Historico, Saldo = @Saldo, TipoMov = @TipoMov, UsuarioId = @UsuarioId, Login = @Login, Metodo = @Metodo" +
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
                         Login = consumo.Login,
                         Metodo = consumo.Metodo

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

        //Caixa1

        public List<Caixa1> ObterTodosCx1(DateTime data, string login, int resId)
        {

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var appcaixa = conn
                    .Query<Caixa1>("select * from CaixaCartao where RestauranteId = @resId and Fechado = 0 and Dia = @data and login = @login order by data desc", new { data, login, resId })
                    .ToList();
                conn.Close();

                return appcaixa;
            }
        }

        public List<Caixa1> ObterCx1PorId(int Nro)
        {

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var caixa = conn
                    .Query<Caixa1>("select * from CaixaCartao where Nro = @Nro", new { Nro })
                    .ToList();
                conn.Close();

                return caixa;
            }

        }

        public void InsertCx1(Caixa1 caixa)
        {
            var sql = "Insert Into CaixaCartao(Nro, Historico, Login, Data, Fechado, Valor, Metodo, Dia, MovId, RestauranteId)" +
                      "Values (@Nro, @Historico, @Login, @Data, @Fechado, @Valor, @Metodo, @Dia, @MovId, @RestauranteId)";
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql,
                    new
                    {

                        Nro = caixa.Nro,
                        Historico = caixa.Historico,
                        Login = caixa.Login,
                        Data = DateTime.Now,
                        Fechado = 0,
                        Valor = caixa.Valor,
                        Metodo = caixa.Metodo,
                        Dia = DateTime.Now,
                        MovId = caixa.MovId,
                        RestauranteId = caixa.RestauranteId

                    });
                conn.Close();
            }
        }

        public void FecharCx1(DateTime data, string login, int resId)
        {
            var sql = "Update CaixaCartao set DataFechamento=@DataFechamento, Fechado= 1" +
                      " where Dia = @Data and login = @Login and RestauranteId = @resId";
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql,
                    new
                    {
                        Data = data,
                        DataFechamento = DateTime.Now,
                        Login = login,
                        resId = resId
                    });
                conn.Close();
            }
        }

        public void DeleteCaix(string id)
        {
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query("Delete from CaixaCartao where MovId = @CartaoConsumoMovId",
                    new
                    {
                        CartaoConsumoMovId = Guid.Parse(id)
                    });
                conn.Close();
            }
        }

        //auditoria

        public List<AuditoriaConsumo> ObterAuditoriaDia(DateTime data, int resId)
        {

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var appcaixa = conn
                    .Query<AuditoriaConsumo>("select * from AuditoriaConsumo where Dia = @data and RestauranteId = @resId order by Data desc", new { data, resId })
                    .ToList();
                conn.Close();

                return appcaixa;
            }
        }

        public List<AuditoriaConsumo> ObterAuditoriaTodos(int resId)
        {

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var appcaixa = conn
                    .Query<AuditoriaConsumo>("select * from AuditoriaConsumo where RestauranteId = @resId order by Data desc", new { resId })
                    .ToList();
                conn.Close();

                return appcaixa;
            }
        }

        public void InsertAuditoria(AuditoriaConsumo caixa)
        {
            var sql = "Insert Into AuditoriaConsumo(Historico, Login, Dia, Data, Valor, RestauranteId)" +
                      "Values (@Historico, @Login, @Dia, @Data, @Valor, @RestauranteId)";
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql,
                    new
                    {
                        Historico = caixa.Historico,
                        Login = caixa.Login,
                        Dia = DateTime.Now,
                        Data = DateTime.Now,
                        Valor = caixa.Valor,
                        RestauranteId = caixa.RestauranteId

                    });
                conn.Close();
            }
        }

        public void ZerarCartao(string consumo)
        {
            var sql = "Update CartaoConsumo set SaldoAtual= 0" +
                      " where CartaoConsumoId = @CartaoConsumoId";
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql,
                    new
                    {
                        CartaoConsumoId = consumo
                    });
                conn.Close();
            }
        }
    }
}
