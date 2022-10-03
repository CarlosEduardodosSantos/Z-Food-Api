using System;
using System.Linq;
using APIAlturas.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace APIAlturas.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class CartaoController : Controller
    {
        private readonly CartaoDao _cartaoDao;

        public CartaoController(CartaoDao cartaoDao)
        {
            _cartaoDao = cartaoDao;
        }


        [HttpGet("obterByUsuario/{usuarioId}")]
        public RootResult ObterByToken(string usuarioId)
        {
            var data = _cartaoDao.ObterPorUserId(usuarioId).ToList();
            var totalPage = 1;
            return new RootResult()
            {
                TotalPage = totalPage,
                Results = data
            };
        }

        [HttpPost("adicionar")]
        public object Adicionar([FromBody] Cartao cartao)
        {
            
            try
            {
                cartao.CartaoId = cartao.CartaoId == Guid.Empty ? Guid.NewGuid() : cartao.CartaoId;
                _cartaoDao.Insert(cartao);
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

        [HttpDelete("deletar/{id}")]
        public object Delete(string id)
        {

            try
            {
                _cartaoDao.Delete(id);
                return new
                {
                    errors = false,
                    message = "Exclusão efetuada com sucesso."
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