using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using APIAlturas.Helper;
using APIAlturas.ViewModels;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace APIAlturas.ViewModels
{
    public class CartaoConsumo
    {
        //campos do banco

        public Guid CartaoConsumoId { get; set; }

        public string Numero { get; set; }

        public string Descricao { get; set; }

        public decimal Valor { get; set; }

        public string Validade { get; set; }

        public string Cpf { get; set; }
        
        public decimal Desconto { get; set; }

        public string Nome { get; set; }
        public int RestauranteId { get; set; }

        public decimal SaldoAtual { get; set; }

        public string Grupo { get; set; }
        public string RegistradoPor { get; set; }
        public bool Frete { get; set; }

    }
}
