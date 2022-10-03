using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIAlturas.Service;
using APIAlturas.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace APIAlturas.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class FarmaciasController: Controller
    {
        private readonly FarmaciasDao _farmaDao;

        public FarmaciasController(FarmaciasDao farmaDao)
        {
            _farmaDao = farmaDao;
        }

        [HttpGet("obterByFarmaId/{Cnpj}")]
        public async Task<Farmacias> ObterConsulPorId(string Cnpj)
        {
          var data = _farmaDao.ObterPorId(Cnpj).FirstOrDefault();
          Farmacias.Conexao = data.ConString;
          return data;
        }


        [HttpPost("adicionar")]
        public object Adicionar([FromBody] ReceitasPharma receita)
        {

            try
            {
                _farmaDao.Insert(receita);
                return new
                {
                    errors = false,
                    message = "Cadastro efetuado com sucesso."
                };
            }
            catch (Exception e)
            {
                return new
                {
                    errors = true,
                    message = e.Message
                };
            }

        }
    }
}
