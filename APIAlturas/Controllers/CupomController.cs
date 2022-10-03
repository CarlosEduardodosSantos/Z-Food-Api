using System;
using System.Collections.Generic;
using APIAlturas.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace APIAlturas.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class CupomController : Controller
    {
        private readonly CupomDao _cupomDao;

        public CupomController(CupomDao cupomDao)
        {
            _cupomDao = cupomDao;
        }

        [HttpGet("obterByUsuario/{usuarioId}/{restauranteId}")]
        public RootResult ObterByUsuario(string usuarioId, int restauranteId)
        {
            var cupons = _cupomDao.ObterDisponiveis(usuarioId, restauranteId);
            var totalPage = 1;
            return new RootResult()
            {
                TotalPage = totalPage,
                Results = cupons
            };
        }


        [HttpGet("obterByCodigo/{usuarioId}/{restauranteId}/{codigoCupom}")]
        public RootResult ObterByCodigo(string usuarioId, int restauranteId, string codigoCupom)
        {
            var cupons = new List<Cupom>(); 
            var cupom = _cupomDao.ObterCodigo(usuarioId, restauranteId, codigoCupom);
            if(cupom != null)
                cupons.Add(cupom);

            var totalPage = 1;
            return new RootResult()
            {
                TotalPage = totalPage,
                Results = cupons
            };
        }


        [HttpGet("obterByRestauranteId/{restauranteId}")]
        public RootResult ObterByRestaurante(int restauranteId)
        {
            var cupons = _cupomDao.ObterPorRestaurante(restauranteId);
            var totalPage = 1;
            return new RootResult()
            {
                TotalPage = totalPage,
                Results = cupons
            };
        }

        [HttpGet("obterCupomMovimentacao/{cupomId}")]
        public RootResult ObterByRestaurante(string cupomId)
        {
            var cupons = _cupomDao.ObterCupomMovimentacaos(cupomId);
            var totalPage = 1;
            return new RootResult()
            {
                TotalPage = totalPage,
                Results = cupons
            };
        }

        [HttpPost("adicionar")]
        public object Adicionar([FromBody] Cupom cupom)
        {
            try
            {
                _cupomDao.Adicionar(cupom);
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
        [HttpDelete("alterarSituacao/{cupomId}/{situacao}")]
        public object AlterarSituacao(string cupomId, int situacao)
        {
            try
            {
                _cupomDao.AlteraSituacao(cupomId, situacao);
                return new
                {
                    errors = false,
                    message = "Situação alterada com sucesso."
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