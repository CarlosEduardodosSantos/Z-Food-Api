using APIAlturas.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;

namespace APIAlturas.Controllers
{
    [Route("api/[controller]")]
    public class ProdutoShiftsController : Controller
    {
        ProdutoShiftsDAO _produtoShiftsDAO;
        public ProdutoShiftsController(
            [FromServices] ProdutoShiftsDAO produtoShiftsDAO
        )
        {
            _produtoShiftsDAO = produtoShiftsDAO;
        }

        [HttpGet("{prodShiftsGuid}")]
        public IActionResult ByGuid(Guid prodShiftsGuid)
        {
            try
            {
                return Ok(_produtoShiftsDAO.GetByGuid(prodShiftsGuid));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("ByProdId/{prodId}")]
        public IActionResult ByProdId(int prodId)
        {
            try
            {
                return Ok(_produtoShiftsDAO.GetByProdId(prodId));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("")]
        public IActionResult Create(
            [FromBody] ProdutoShifts prodShifts
        ) {
            try
            {
                return Ok(_produtoShiftsDAO.Create(prodShifts));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("{prodShiftsGuid}")]
        public IActionResult Delete(Guid prodShiftsGuid)
        {
            try
            {
                return Ok(_produtoShiftsDAO.Delete(prodShiftsGuid));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
