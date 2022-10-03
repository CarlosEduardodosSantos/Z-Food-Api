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
    public class CartaoDao
    {
        private readonly IConfiguration _configuration;

        public CartaoDao(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<Cartao> ObterPorUserId(string userId)
        {
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                var cartoes = conn
                    .Query<Cartao>("Select * From UserCartoes Where UserId = @userId", new { userId })
                    .ToList();
                conn.Close();

                return cartoes;
            }
        }

        public void Insert(Cartao cartao)
        {
            var sql = "Insert Into UserCartoes(CartaoId, UserId, num, cvv, monthExp, yearExp, titular, cpf, image, brand) " +
                      "Values (@CartaoId, @UserId, @num, @cvv, @monthExp, @yearExp, @titular, @cpf, @image, @brand)";
            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("ViPFood")))
            {
                conn.Open();
                conn.Query(sql,
                    new
                    {
                        CartaoId = cartao.CartaoId,
                        UserId = cartao.UserId,
                        num = FuncaoIteis.SoNumeros(cartao.num),
                        cvv = cartao.cvv,
                        monthExp = cartao.monthExp,
                        yearExp = cartao.yearExp,
                        titular = cartao.titular,
                        cpf = FuncaoIteis.SoNumeros(cartao.cpf),
                        image = cartao.image,
                        brand = cartao.brand
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
                conn.Query("Delete From UserCartoes Where CartaoId = @cartaoId",
                    new
                    {
                        cartaoId = Guid.Parse(id)
                    });
                conn.Close();
            }
        }
    }
}