using System.Collections.Generic;
using System.Linq;
using APIAlturas.Service;
using APIAlturas.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace APIAlturas.Controllers
{
    [Route("api/[controller]")]
    public class LocalizacaoController : Controller
    {
        private readonly LocalizacaoDao _localizacaoDao;

        public LocalizacaoController(LocalizacaoDao localizacaoDao)
        {
            _localizacaoDao = localizacaoDao;
        }

        [HttpGet("estados")]
        public List<Estado> GetEstados()
        {
            return _localizacaoDao.ObterEstados().OrderBy(t=> t.estado).ToList();
        }
        [HttpGet("cidades/{estado}")]
        public List<Cidade> GetCidades(string estado)
        {
            return _localizacaoDao.ObterCidades(estado).OrderBy(t => t.cidade).ToList();
        }
        [HttpGet("bairros/{estado}/{cidade}")]
        public List<Bairro> GetBairros(string estado, string cidade)
        {
            return _localizacaoDao.ObterBairros(estado, cidade).OrderBy(t => t.bairro).ToList();
        }
        [HttpGet("viaCep/{cep}")]
        public CepViewModel GetviaCep(string cep)
        {
            var service = new CepBrasilService();
            return service.ObterPorCep(cep);
        }
        [HttpGet("viaCepByEndereco/{restauranteId}/{endereco}")]
        public List<CepViewModel> GetByEndereco([FromServices] RestauranteDAO restauranteDAO, int restauranteId, string endereco)
        {
            var enderecoNew = endereco.ToUpper().Replace("RUA", "").Replace("AV", "").Replace("AVENIDA", "").Trim();
            if (enderecoNew.Length < 3) return new List<CepViewModel>();


            var service = new CepBrasilService();

            var restaurante = restauranteDAO.FindById(restauranteId);
            var estado = restaurante.Uf;
            var cidade = restaurante.Cidade;

            return service.ObterPorEndereco(estado, cidade, endereco).Take(5).ToList();
        }
    }
}