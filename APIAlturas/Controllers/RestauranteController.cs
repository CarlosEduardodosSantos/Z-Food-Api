using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using APIAlturas.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace APIAlturas.Controllers
{
    [Route("api/[controller]")]
    public class RestauranteController : Controller
    {
        private readonly RestauranteDAO _restauranteDAO;
        public RestauranteController([FromServices]RestauranteDAO restauranteDAO)
        {
            _restauranteDAO = restauranteDAO;
        }
        //[Authorize("Bearer")]
        [HttpGet("RestaurantesCep/{cep}/{setorId}/{limit}/{page}")]
        public RootResult Get(string cep, string setorId, int limit, int page)
        {
            var data = _restauranteDAO.Find(cep, setorId).ToList();
            var totalPage = (int)Math.Ceiling((double)data.Count / limit);
            return new RootResult()
            {
                TotalPage = totalPage,
                Results = data.Skip((page - 1) * limit).Take(limit).ToList<object>()
            };
        }
        [HttpGet("RestaurantByToken/{cep}/{token}")]
        public RootResult GetByToken(string cep, string token)
        {
            var data = _restauranteDAO.GetByToken(cep, token);

            return new RootResult()
            {
                TotalPage = 1,
                Results = data
            };
        }

        [HttpGet("restauranteByToken/{token}")]
        public Restaurante GetRestauranteByToken(string token)
        {
            return _restauranteDAO.FindByToken(token);
        }
        [HttpGet("getById/{restauranteId}")]
        public Restaurante GetById(int restauranteId)
        {
            return _restauranteDAO.FindById(restauranteId);
        }

        [HttpGet("restauranteByCnpj/{cnpj}")]
        public Restaurante GetRestauranteByCnpj(string cnpj)
        {
            var restaurante = _restauranteDAO.FindByCnpj(cnpj);
            return restaurante;
        }
        [HttpGet("RestaurantesWishlist/{usuarioId}/{cep}")]
        public List<Restaurante> GetWishlist(string usuarioId, string cep)
        {
            return _restauranteDAO.FindWishlist(usuarioId, cep).ToList();
        }
        [HttpPost("addWishlist")]
        public object AddWishlist([FromBody] Wishlist wishlist)
        {

            _restauranteDAO.AddWishlist(wishlist);

            return new
            {
                errors = false,
                message = "Cadastro efetuado com sucesso."
            };
        }
        [HttpPost("removeWishlist")]
        public object RemoveWishlist([FromBody] Wishlist wishlist)
        {

            _restauranteDAO.RemoveWishlist(wishlist);

            return new
            {
                errors = false,
                message = "Cadastro efetuado com sucesso."
            };
        }

        [HttpPost("player")]
        public object PlayerRestaurante([FromBody] PlayersClick playersClick)
        {
            _restauranteDAO.AddPlayerRestaurante(playersClick);
            return new
            {
                errors = false,
                message = "Cadastro efetuado com sucesso."
            };
        }
        [HttpPut("putstatuss/{token}/{status}")]
        public object putStatus(string token, string status)
        {
            var situacao = (RestauranteSituacaoEnum)Enum.Parse(typeof(RestauranteSituacaoEnum), status, true);
            _restauranteDAO.AlterarSituacao(token, situacao);
            return new
            {
                errors = false,
                message = "Status alterado com sucesso."
            };
        }

        [HttpGet("getstatuss")]
        public object GetStatus(string token)
        {
            return _restauranteDAO.ObterSituacao(token);
        }

        [HttpPut("alterarRestaurante")]
        public object AlterarRestaurante([FromBody] Restaurante restaurante,
            [FromServices]IHostingEnvironment hostingEnv)
        {
            try
            {

                _restauranteDAO.AlterarRestaurante(restaurante);


                return new
                {
                    errors = false,
                    message = "Cadastro efetuado com sucesso."
                };


            }
            catch
            {
                return new
                {
                    errors = true,
                    message = "Erro ao efetuar a alteração."
                };
            }

        }

        [HttpGet("locaisAtendimentos/{restauranteId}")]
        public List<LocalAtendimento> ObterLocaisAtendimento(int restauranteId)
        {
            return _restauranteDAO.ObterLocalAtendimentoPorToken(restauranteId).ToList();
        }
        [HttpPost("adicionarLocalAtendimento")]
        public object AdicionarLocalAtendimento([FromBody] LocalAtendimento localAtendimento)
        {

            try
            {
                _restauranteDAO.AdicionarLocalAtendimento(localAtendimento);

                return new
                {
                    errors = false,
                    message = "Cadastro efetuado com sucesso."
                };
            }
            catch
            {
                return new
                {
                    errors = true,
                    message = "Erro ao efetuar o cadastro."
                };
            }
        }
        [HttpPut("alterarLocalAtendimento")]
        public object AlterarLocalAtendimento([FromBody] LocalAtendimento localAtendimento)
        {
            try
            {
                _restauranteDAO.AlterarLocalAtendimento(localAtendimento);

                return new
                {
                    errors = false,
                    message = "Cadastro efetuado com sucesso."
                };
            }
            catch
            {
                return new
                {
                    errors = true,
                    message = "Erro ao efetuar o cadastro."
                };
            }

        }

        [HttpDelete("deleteLocalAtendimento/{atendimentoLocalId}")]
        public object DeleteLocalAtendimento(int atendimentoLocalId)
        {
            try
            {
                _restauranteDAO.DeleteLocalAtendimento(atendimentoLocalId);

                return new
                {
                    errors = false,
                    message = "Cadastro excluido com sucesso."
                };
            }
            catch
            {
                return new
                {
                    errors = true,
                    message = "Erro a exclusão do cadastro."
                };
            }

        }

        [HttpPut("alterarVlEst")]
        public object alterarVlEst([FromBody] Restaurante restaurante)
        {
            try
            {
                _restauranteDAO.AlterarRestauranteValorEst(restaurante);
                return new
                {
                    errors = false,
                    message = "Cadastro atualizado com sucesso."
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