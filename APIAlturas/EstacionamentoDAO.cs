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
    public class EstacionamentoDAO
    {
        private readonly IConfiguration _configuration;

        public EstacionamentoDAO(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<Estacionamento> ObterTodos()
        {

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var estacionamento = conn
                    .Query<Estacionamento>("select * from Estacionamento order by Entrada")
                    .ToList();
                conn.Close();

                return estacionamento;
            }

        }

        public List<Estacionamento> ObterTodosAbertos()
        {

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var estacionamento = conn
                    .Query<Estacionamento>("select * from Estacionamento where Fechado=0 or Fechado is null order by Entrada")
                    .ToList();
                conn.Close();

                return estacionamento;
            }

        }

        public List<Estacionamento> ObterPorId(int Nro)
        {

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var estacionamento = conn
                    .Query<Estacionamento>("select * from Estacionamento where Nro = @Nro", new { Nro })
                    .ToList();
                conn.Close();

                return estacionamento;
            }

        }

        public List<Estacionamento> ObterAbertoPorDia(DateTime dateR)
        {

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var estacionamento = conn
                    .Query<Estacionamento>("select * from Estacionamento where DataReg = @dateR and (Fechado=0 or Fechado is null) order by Entrada", new { dateR })
                    .ToList();
                conn.Close();

                return estacionamento;
            }

        }

        public void Insert(Estacionamento estacionamento)
        {
            var sql = "Insert Into Estacionamento(Empresa, Apto, Placa, Modelo, Entrada, Saida, ValorTotal, Metodo, DataReg, Fechado)" +
                      "Values (@Empresa, @Apto, @Placa, @Modelo, @Entrada, @Saida, @ValorTotal, @Metodo, @DataReg, @Fechado)";
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql,
                    new
                    {
                        
                        Empresa = estacionamento.Empresa,
                        Apto = estacionamento.Apto,
                        Placa = estacionamento.Placa,
                        Modelo = estacionamento.Modelo,
                        Entrada = estacionamento.Entrada,
                        Saida = estacionamento.Saida,
                        ValorTotal = estacionamento.ValorTotal,
                        Metodo = estacionamento.Metodo,
                        DataReg = estacionamento.DataReg,
                        Fechado = estacionamento.Fechado
                    }); ; ;
                conn.Close();
            }
        }

        public void Update(Estacionamento estacionamento)
        {
            var sql = "Update Estacionamento set Empresa=@Empresa, Apto =@Apto, Placa=@Placa, Modelo=@Modelo, Entrada=@Entrada, Saida=@Saida, ValorTotal=@ValorTotal, Metodo=@Metodo, DataReg=@DataReg, Fechado=@Fechado " +
                      " where Nro = @Nro";
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql,
                    new
                    {
                        Nro = estacionamento.Nro,
                        Empresa = estacionamento.Empresa,
                        Apto = estacionamento.Apto,
                        Placa = estacionamento.Placa,
                        Modelo = estacionamento.Modelo,
                        Entrada = estacionamento.Entrada,
                        Saida = estacionamento.Saida,
                        ValorTotal = estacionamento.ValorTotal,
                        Metodo = estacionamento.Metodo,
                        DataReg = estacionamento.DataReg,
                        Fechado = estacionamento.Fechado
                    });
                conn.Close();
            }
        }

        public void UpdateData(Estacionamento estacionamento)
        {
            var sql = "Update Estacionamento set Fechado=@Fechado " +
                      " where DataReg=@DataReg";
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql,
                    new
                    {
                        DataReg = estacionamento.DataReg,
                        Fechado = estacionamento.Fechado
                    });
                conn.Close();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query("Delete from Estacionamento where Nro = @Nro",
                    new
                    {
                        Nro = id
                    }) ;
                conn.Close();
            }
        }

        //cx1

        public List<AppCaixa1> ObterTodosCx1()
        {

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var appcaixa = conn
                    .Query<AppCaixa1>("select * from AppCaixa1")
                    .ToList();
                conn.Close();

                return appcaixa;
            }

        }

        public List<AppCaixa1> ObterPorIdCx1(int Nro)
        {

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var caixa = conn
                    .Query<AppCaixa1>("select * from AppCaixa1 where Nro = @Nro", new { Nro })
                    .ToList();
                conn.Close();

                return caixa;
            }

        }

        public void InsertCx1(AppCaixa1 caixa)
        {
            var sql = "Insert Into AppCaixa1(DataAb, DataFx, NroEs)" +
                      "Values (@DataAb, @DataFx, @NroEs)";
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql,
                    new
                    {

                        DataAb = caixa.DataAb,
                        DataFx = caixa.DataFx,
                        NroEs = caixa.NroEs
                    });
                conn.Close();
            }
        }

        public void UpdateCx1(AppCaixa1 caixa)
        {
            var sql = "Update AppCaixa1 set DataAb=@DataAb, DataFx=@DataFx, NroEs=@NroEs" +
                      " where Nro = @Nro";
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql,
                    new
                    {
                        Nro = caixa.Nro,
                        DataAb = caixa.DataAb,
                        DataFx = caixa.DataFx,
                        NroEs = caixa.NroEs
                    });
                conn.Close();
            }
        }

        public void UpdateCx1Es(AppCaixa1 caixa)
        {
            var sql = "Update AppCaixa1 set DataFx=@DataFx" +
                      " where NroEs = @NroEs";
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql,
                    new
                    {
                        DataFx = caixa.DataFx,
                        NroEs = caixa.NroEs
                    });
                conn.Close();
            }
        }

        public void DeleteCx1(int id)
        {
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query("Delete from AppCaixa1 where Nro = @Nro",
                    new
                    {
                        Nro = id
                    });
                conn.Close();
            }
        }

        //cx2

        public List<AppCaixa2> ObterTodosCx2()
        {

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var appcaixa = conn
                    .Query<AppCaixa2>("select * from AppCaixa2")
                    .ToList();
                conn.Close();

                return appcaixa;
            }

        }

        public List<AppCaixa2> ObterPorIdCx2(int Nro)
        {

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var caixa = conn
                    .Query<AppCaixa2>("select * from AppCaixa2 where Nro = @Nro", new { Nro })
                    .ToList();
                conn.Close();

                return caixa;
            }

        }

        public void InsertCx2(AppCaixa2 caixa)
        {
            var sql = "Insert Into AppCaixa2(NroCx1, TipoMov, Valor, Especie)" +
                      "Values (@NroCx1, @TipoMov, @Valor, @Especie)";
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql,
                    new
                    {

                        NroCx1 = caixa.NroCx1,
                        TipoMov = caixa.TipoMov,
                        Valor = caixa.Valor,
                        Especie = caixa.Especie
                    });
                conn.Close();
            }
        }

        public void UpdateCx2(AppCaixa2 caixa)
        {
            var sql = "Update AppCaixa2 set NroCx1=@NroCx1, TipoMov=@TipoMov, Valor=@Valor, Especie=@Especie" +
                      "where Nro = @Nro";
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql,
                    new
                    {
                        Nro = caixa.Nro,
                        NroCx1 = caixa.NroCx1,
                        TipoMov = caixa.TipoMov,
                        Valor = caixa.Valor,
                        Especie = caixa.Especie
                    });
                conn.Close();
            }
        }

        public void DeleteCx2(int id)
        {
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query("Delete from AppCaixa2 where Nro = @Nro",
                    new
                    {
                        Nro = id
                    });
                conn.Close();
            }
        }
    }
}
