using APIAlturas.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace APIAlturas.Controllers
{
    [Route("api/[controller]")]
    public class PlayerController : Controller
    {
        private readonly PlayersClickDAO _playersClickDao;

        public PlayerController(PlayersClickDAO playersClickDao)
        {
            _playersClickDao = playersClickDao;
        }

        [HttpPost("restaurante")]
        public object PlayerRestaurante([FromBody] PlayersClick playersClick)
        {
            _playersClickDao.AddPlayerRestaurante(playersClick);
            return new
            {
                errors = false,
                message = "Cadastro efetuado com sucesso."
            };
        }

        [HttpPost("produto")]
        public object PlayerProduto([FromBody] PlayersClick playersClick)
        {
            _playersClickDao.AddPlayerProduto(playersClick);
            return new
            {
                errors = false,
                message = "Cadastro efetuado com sucesso."
            };
        }
    }

}