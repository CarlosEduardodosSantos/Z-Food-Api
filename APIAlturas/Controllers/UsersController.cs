using System;
using System.Linq;
using System.Threading.Tasks;
using APIAlturas.ExtensionLogger;
using APIAlturas.Service;
using APIAlturas.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace APIAlturas.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly UsersDAO _usersDao;

        public UsersController(UsersDAO usersDao)
        {
            _usersDao = usersDao;
        }

        [HttpGet("obterPorTelefone/{numero}/{restauranteId}")]
        public Users ObterPorTelefone(string numero, int restauranteId)
        {
            try
            {
                var data = _usersDao.ObterPorTelefone(numero, restauranteId).FirstOrDefault();

                return data;
            }

            catch (Exception e)
            {
                var erro = e;
                return null;
            }
        }

        [HttpPost("cadastrarPorTelefone")]
        public object CadastrarPorTelefone([FromBody] Users dadosUsuario)
        {
            try
            {
                var usuario = _usersDao.ObterPorTelefone(dadosUsuario.fone, dadosUsuario.restauranteId).FirstOrDefault();

                if (usuario == null)
                {
                    _usersDao.CadastrarPorTelefone(dadosUsuario);
                } else
                {
                    _usersDao.AtualizarPorTelefone(dadosUsuario);
                }

                return new
                {
                    errors = false,
                    message = "Cadastro/atualização realizada com sucesso."
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
