using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIAlturas.ViewModels
{
    public class Users
    {
        public Guid userID { get; set; }
        public string nome { get; set; }
        public string fone { get; set; }
        public int restauranteId { get; set; }
        public string logradouro { get; set; }
        public string numero { get; set; }
        public string bairro { get; set; }
        public string cep { get; set; }
        public string localidade { get; set; }
        public string complemento { get; set; }
        public string uf { get; set; }
    }
}
