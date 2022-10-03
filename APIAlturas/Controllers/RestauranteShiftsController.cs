using APIAlturas.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace APIAlturas.Controllers
{
    [Route("api/[controller]")]
    public class RestauranteShiftsController : Controller
    {
        private readonly RestauranteShiftsDao _restauranteShiftsDao;

        public RestauranteShiftsController(RestauranteShiftsDao restauranteShiftsDao)
        {
            _restauranteShiftsDao = restauranteShiftsDao;
        }

        [HttpGet("ByRestId/{restId}")]
        public IActionResult GetByRestId(int restId)
        {
            try
            {
                return Ok(_restauranteShiftsDao.GetByRestauranteId(restId));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("")]
        public IActionResult Create([FromBody]RestauranteShifts restauranteShif)
        {
            try
            {
                _restauranteShiftsDao.Adicionar(restauranteShif);
                return Ok(new
                {
                    errors = false,
                    message = "Registro incluido com sucesso"
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, new
                {
                    errors = true,
                    messsage = e.Message
                });
            }
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                _restauranteShiftsDao.Remover(id);
                return Ok(new
                {
                    errors = false,
                    message = "Excluido com sucesso"
                });

            }

            catch (Exception e)
            {
                return StatusCode(500, new
                {
                    errors = true,
                    messsage = e.Message
                });
            }
        }
    }
}
