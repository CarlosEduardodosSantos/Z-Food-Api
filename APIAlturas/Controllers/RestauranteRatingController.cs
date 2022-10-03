using APIAlturas.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace APIAlturas.Controllers
{
    [Route("api/[controller]")]
    public class RestauranteRatingController : Controller
    {
        RestauranteRatingDAO _restauranteRatingDAO;
        public RestauranteRatingController(
            [FromServices]RestauranteRatingDAO restauranteRatingDAO
        ) {
            _restauranteRatingDAO = restauranteRatingDAO;
        }

        [HttpGet("{restRatingGuid}")]
        public IActionResult ById(Guid restRatingGuid)
        {
            try
            {
                return Ok(_restauranteRatingDAO.GetByGuid(restRatingGuid));
            } catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("ByRestId/{restId}")]
        public IActionResult ByRestId(int restId)
        {
            try
            {
                return Ok(_restauranteRatingDAO.GetByRestId(restId));
            } catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("")]
        public IActionResult Create(
            [FromBody]RestauranteRating restRating
        ) {
            try
            {
                return Ok(_restauranteRatingDAO.Create(restRating));
            } catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("")]
        public IActionResult Update(
            [FromBody]RestauranteRating restRating
        ) {
            try
            {
                return Ok(_restauranteRatingDAO.Update(restRating));
            } catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("{restRatingGuid}")]
        public IActionResult Delete(Guid restRatingGuid)
        {
            try
            {
                return Ok(_restauranteRatingDAO.Delete(restRatingGuid));
            } catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
