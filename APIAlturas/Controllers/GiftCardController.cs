using APIAlturas.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIAlturas.Controllers
{
    [Route("api/[controller]")]
    public class GiftCardController : Controller
    {
        GiftCardDAO _giftCardDAO;
        public GiftCardController (
            GiftCardDAO giftCardDAO
        ) {
            _giftCardDAO = giftCardDAO;
        }

        [HttpGet("{guid}")]
        public IActionResult ByGuid(Guid guid)
        {
            try
            {
                return Ok(_giftCardDAO.GetByGuid(guid));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("byUserId/{userId}")]
        public IActionResult ByUserId(Guid userId)
        {
            try
            {
                return Ok(_giftCardDAO.GetByUserId(userId));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("")]
        public IActionResult Create(
            [FromBody]GiftCard giftCard
        ) {
            try
            {
                return Ok(_giftCardDAO.Create(giftCard));
            } catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("")]
        public IActionResult Update(
            [FromBody]GiftCard giftCard
        ) {
            try
            {
                return Ok(_giftCardDAO.Update(giftCard));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("{guid}")]
        public IActionResult Delete(Guid guid)
        {
            try
            {
                return Ok(_giftCardDAO.Delete(guid));
            } catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
