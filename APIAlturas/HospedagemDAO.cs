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
    public class HospedagemDAO
    {
        private readonly IConfiguration _configuration;

        public HospedagemDAO(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<Hospedagem> ObterTodos()
        {

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var hospedagem = conn
                    .Query<Hospedagem>("select * from Hospedagem")
                    .ToList();
                conn.Close();

                return hospedagem;
            }

        }

        public List<Hospedagem> ObterPorId(string Nro)
        {

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var hospedagem = conn
                    .Query<Hospedagem>("select * from Hospedagem where Nro = @Nro", new { Nro })
                    .ToList();
                conn.Close();

                return hospedagem;
            }

        }
        public void Insert(Hospedagem hospedagem)
        {
            var sql = "Insert Into Hospedagem(Nro, Nome, Apto, NomeHotel, DataCheckin, DataCheckout, QtdeA, QtdeC, QtdeN, Cafe, Almoco, Janta)" +
                      "Values (@Nro,@Nome ,@Apto, @NomeHotel, @DataCheckin, @DataCheckout, @QtdeA, @QtdeC, @QtdeN, @Cafe, @Almoco, @Janta)";
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql,
                    new
                    {
                        Nro = Guid.NewGuid(),
                        Nome= hospedagem.Nome,
                        Apto = hospedagem.Apto,
                        NomeHotel = hospedagem.NomeHotel,
                        DataCheckin = hospedagem.DataCheckin,
                        DataCheckout = hospedagem.DataCheckout,
                        QtdeA = hospedagem.QtdeA,
                        QtdeC = hospedagem.QtdeC,
                        QtdeN = hospedagem.QtdeN,
                        Cafe = hospedagem.Cafe,
                        Almoco = hospedagem.Almoco,
                        Janta = hospedagem.Janta,

                    });; ;
                conn.Close();
            }
        }

        public void Update(Hospedagem hospedagem)
        {
            var sql = "Update Hospedagem set Apto =@Apto, Nome=@Nome,NomeHotel =@NomeHotel, DataCheckin =@DataCheckin, DataCheckout=@DataCheckout, QtdeA=@QtdeA, QtdeC=@QtdeC, QtdeN=@QtdeN, Cafe=@Cafe, Almoco=@Almoco, Janta=@Janta " +
                      " where Nro = @Nro";
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql,
                    new
                    {
                        Nro = hospedagem.Nro,
                        Nome = hospedagem.Nome,
                        Apto = hospedagem.Apto,
                        NomeHotel = hospedagem.NomeHotel,
                        DataCheckin = hospedagem.DataCheckin,
                        DataCheckout = hospedagem.DataCheckout,
                        QtdeA = hospedagem.QtdeA,
                        QtdeC = hospedagem.QtdeC,
                        QtdeN = hospedagem.QtdeN,
                        Cafe = hospedagem.Cafe,
                        Almoco = hospedagem.Almoco,
                        Janta = hospedagem.Janta,


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
                conn.Query("Delete from Hospedagem where Nro = @Nro",
                    new
                    {
                        Nro = Guid.Parse(id)
                    }) ;
                conn.Close();
            }
        }


        //HospedagemAlimentacao

        public List<HospedagemAlimentacao> ObterTodosAlimentacao()
        {

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var hospedagem = conn
                    .Query<HospedagemAlimentacao>("select * from HospedagemAlimentacao")
                    .ToList();
                conn.Close();

                return hospedagem;
            }

        }
        public List<HospedagemAlimentacao> ObterAlimentacaoPorId(string Id)
        {

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var hospedagem = conn
                    .Query<HospedagemAlimentacao>("select * from HospedagemAlimentacao where AlimentacaoId = @Id", new { Id })
                    .ToList();
                conn.Close();

                return hospedagem;
            }

        }

        public List<HospedagemAlimentacao> ObterAlimentacaoPorData(DateTime data)
        {

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var hospedagem = conn
                    .Query<HospedagemAlimentacao>("select * from HospedagemAlimentacao where data = @data", new { data })
                    .ToList();
                conn.Close();

                return hospedagem;
            }

        }

        public List<HospedagemAlimentacao> ObterAlimentacaoPorNome(DateTime data, string nome)
        {
            int.TryParse(nome, out var apto);
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var hospedagem = conn
                    .Query<HospedagemAlimentacao>("select * from HospedagemAlimentacao where data = @data and nome like '%'+@nome+'%' or data = @data and apto = @apto order by nome", new { data, nome, apto })
                    .ToList();
                conn.Close();

                return hospedagem;
            }

        }

        public List<HospedagemAlimentacao> ObterAlimentacaoPorNro(string Nro)
        {

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var hospedagem = conn
                    .Query<HospedagemAlimentacao>("select * from HospedagemAlimentacao where Nro = @Nro", new { Nro })
                    .ToList();
                conn.Close();

                return hospedagem;
            }

        }

        public List<HospedagemAlimentacao> ObterAlimentacaoPorCafe(bool cafe, DateTime data)
        {

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var hospedagem = conn
                    .Query<HospedagemAlimentacao>("select * from HospedagemAlimentacao where Cafe = @cafe and Data=@data", new { cafe, data })
                    .ToList();
                conn.Close();

                return hospedagem;
            }

        }

        public List<HospedagemAlimentacao> ObterAlimentacaoPorAlmoco(bool almoco, DateTime data)
        {

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var hospedagem = conn
                    .Query<HospedagemAlimentacao>("select * from HospedagemAlimentacao where Almoco = @almoco and Data=@data", new { almoco, data })
                    .ToList();
                conn.Close();

                return hospedagem;
            }

        }

        public List<HospedagemAlimentacao> ObterAlimentacaoPorJanta(bool janta, DateTime data)
        {

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var hospedagem = conn
                    .Query<HospedagemAlimentacao>("select * from HospedagemAlimentacao where Janta = @janta and Data=@data", new { janta, data })
                    .ToList();
                conn.Close();

                return hospedagem;
            }

        }

        public void InsertAlimentacao(HospedagemAlimentacao hospedagem)
        {
            var sql = "Insert Into HospedagemAlimentacao(AlimentacaoId, Nro, Data, Cafe, Almoco, Janta, CConsumido, AConsumido, JConsumido)" +
                      "Values (@AlimentacaoId, @Nro, @Data, @Cafe, @Almoco, @Janta, @CConsumido, @AConsumido, @JConsumido)";
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql,
                    new
                    {
                        AlimentacaoId = Guid.NewGuid(),
                        Nro = hospedagem.Nro,
                        Data = hospedagem.Data,
                        Cafe = hospedagem.Cafe,
                        Almoco = hospedagem.Almoco,
                        Janta = hospedagem.Janta,
                        CConsumido = hospedagem.CConsumido,
                        AConsumido = hospedagem.AConsumido,
                        JConsumido = hospedagem.JConsumido

                    }); ;
                conn.Close();
            }
        }

        public void UpdateAlimentacao(HospedagemAlimentacao hospedagem)
        {
            var sql = "Update HospedagemAlimentacao set Nro=@Nro, Data=@Data, Cafe=@Cafe, Almoco=@Almoco, Janta=@Janta,  CConsumido=@CConsumido,  AConsumido=@AConsumido, JConsumido=@JConsumido" +
                      " where AlimentacaoId = @AlimentacaoID";
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql,
                    new
                    {
                        AlimentacaoId = hospedagem.AlimentacaoId,
                        Nro = hospedagem.Nro,
                        Data = hospedagem.Data,
                        Cafe = hospedagem.Cafe,
                        Almoco = hospedagem.Almoco,
                        Janta = hospedagem.Janta,
                        CConsumido = hospedagem.CConsumido,
                        AConsumido = hospedagem.AConsumido,
                        JConsumido = hospedagem.JConsumido

                    });
                conn.Close();
            }
        }

        public void DeleteAlimentacao(string id)
        {
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query("Delete from HospedagemAlimentacao where AlimentacaoId = @AlimentacaoId",
                    new
                    {
                        AlimentacaoId = Guid.Parse(id)
                    });
                conn.Close();
            }
        }

        public List<HospedagemRelatorio> ObterPorRelatorio(DateTime dataini, DateTime datafim)
        {

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var hospedagem = conn
                    .Query<HospedagemRelatorio>("select * from HospedagemRelatorio where data between @dataini and @datafim order by data", new { dataini, datafim })
                    .ToList();
                conn.Close();

                return hospedagem;
            }

        }


    }
}
