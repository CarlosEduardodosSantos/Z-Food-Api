using System;
using System.Collections.Generic;
using System.Linq;
using APIAlturas.ViewModels;
using CreditCardValidator;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace APIAlturas.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class FormaPagamentoController : Controller
    {
        private readonly FormaPagamentoDao _formaPagamentoDao;
        
        public FormaPagamentoController(FormaPagamentoDao formaPagamentoDao)
        {
            _formaPagamentoDao = formaPagamentoDao;
        }

        [HttpGet("getCardBrand/{cardNumber}")]
        public CardBrandViewModel GetCardBrand([FromServices]IHostingEnvironment hostingEnv, string cardNumber)
        {

            var cardBrand = new CardBrandViewModel();
            var url = $"{Request.Scheme}://{Request.Host.Value}";

            var brand = cardNumber.Replace(" ", "").Trim().CreditCardBrandIgnoreLength();

            cardBrand.Brand = brand;
            cardBrand.BrandName = brand.ToString();
            cardBrand.CardNumber = cardNumber;
            cardBrand.IsValid = new CreditCardDetector(cardNumber).IsValid();
            cardBrand.Image = $"{url}/api/galeria/uteis/{brand.ToString().ToLower()}.png";
            
            return cardBrand;
        }


        [HttpGet("obterByToken/{token}/{limit}/{page}")]
        public RootResult ObterByToken(string token, int limit, int page)
        {
            var data = _formaPagamentoDao.ObterPorToken(token).ToList();
            
            var totalPage = (int)Math.Ceiling((double)data.Count / limit);
            return new RootResult()
            {
                TotalPage = totalPage,
                Results = data.Skip((page - 1) * limit).Take(limit).ToList<object>()
            };
        }

        [HttpGet("obterEspecies")]
        public IEnumerable<EspeciePagamento> ObterEspecies()
        {
            return _formaPagamentoDao.ObterEspecies();
        }

        [HttpPost("adicionar")]
        public object Adicionar([FromBody] FormaPagamento formaPagamento,
            [FromServices]RestauranteDAO restauranteDao)
        {
            var token = formaPagamento.RestauranteToken.ToString();
            var restaurante = restauranteDao.FindByToken(token);
            if (restaurante?.RestauranteId == null)
            {
                return new
                {
                    errors = true,
                    message = "Restaurante não encontrado."
                };
            }

            try
            {
                formaPagamento.FormaPagamentoId = formaPagamento?.FormaPagamentoId ?? Guid.NewGuid();
                _formaPagamentoDao.Insert(formaPagamento);
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

        [HttpPut("alterar")]
        public object Alterar([FromBody] FormaPagamento formaPagamento,
            [FromServices]RestauranteDAO restauranteDao)
        {
            var token = formaPagamento.RestauranteToken.ToString();
            var restaurante = restauranteDao.FindByToken(token);
            if (restaurante?.RestauranteId == null)
            {
                return new
                {
                    errors = true,
                    message = "Restaurante não encontrado."
                };
            }

            try
            {
                _formaPagamentoDao.Alterar(formaPagamento);
                return new
                {
                    errors = false,
                    message = "Cadastro alterado com sucesso."
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

        [HttpDelete("excluir/{id}")]
        public object Excluir(string id)
        {
            
            try
            {
                _formaPagamentoDao.Excluir(id);
                return new
                {
                    errors = false,
                    message = "Cadastro excluido com sucesso."
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