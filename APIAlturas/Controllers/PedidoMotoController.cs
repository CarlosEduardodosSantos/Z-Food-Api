using APIAlturas.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace APIAlturas.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class PedidoMotoController : Controller
    {
        [HttpPost("pedir/id/token")]
        public object Pedir(string id, string token)
        {

            return new
            {
                errors = false,
                message = "Zip Moto solicitada com sucesso."
            };
        }

        [HttpPut("cancelar/id/token")]
        public object Cancelar(string id, string token)
        {

            return new
            {
                errors = false,
                message = "Cancelamento efetuado com sucesso."
            };
        }

        [HttpGet("consultar/{pedidoId}")]
        public object ConsultarPedido(string pedidoId)
        {

            return new
            {
                errors = false,
                message = "Cadastro efetuado com sucesso."
            };
        }
        [HttpGet("minhas/{pedidoId}")]
        public RootResult Minhas(string restauranteId, string setorId, int limit, int page)
        {

            return new RootResult();
        }

    }
}