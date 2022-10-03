using APIAlturas.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace APIAlturas.Controllers
{
    [Route("api/[controller]")]
    public class FlyerController : Controller
    {
        private readonly FlyerDAO _flyerDao;
        public FlyerController(
            [FromServices]FlyerDAO flyerDao
        )
        {
            _flyerDao = flyerDao;
        }

        [HttpGet("{guid}")]
        public IActionResult GetById(Guid guid)
        {
            try
            {
                return Ok(_flyerDao.GetById(guid));
            } catch(Exception e){
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("ByRestId/{restId}")]
        public IActionResult GetByRestId(int restId)
        {
            try
            {
                return Ok(_flyerDao.getByRestId(restId));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("")]
        public IActionResult Update(
            [FromBody]Flyer flyer,
            [FromServices]RestauranteDAO _restDAO,
            [FromServices]IHostingEnvironment hostingEnv
        ) {
            try
            {
                if (string.IsNullOrEmpty(flyer.Picture)) return StatusCode(400, "Foto promocional não pode ser vazia");

                string image64 = flyer.Picture;
                Restaurante rest = _restDAO.FindById(flyer.RestauranteId);

                string folderPath = $"{hostingEnv.WebRootPath}/galeria/Flyers/{rest.Token}";
                string flyerImageName = $"{flyer.FlyerGuid}.jpg";

                // A partir daqui picture se torna uma url e não um 64
                flyer.Picture = $"{Request.Scheme}://{Request.Host}/api/galeria/flyers/{rest.Token}/{flyer.FlyerGuid}.jpg";
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string imagePath = Path.Combine(folderPath, flyerImageName);

                RemovePicture(imagePath);
                byte[] imageBytes = Convert.FromBase64String(image64);

                System.IO.File.WriteAllBytes(imagePath, imageBytes);

                return Ok(_flyerDao.Update(flyer));
            } catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("")]
        public IActionResult Create(
            [FromBody]Flyer flyer,
            [FromServices] RestauranteDAO _restDAO,
            [FromServices] IHostingEnvironment hostingEnv
        ) {
            try
            {
                string image64 = flyer.Picture;
                Restaurante rest = _restDAO.FindById(flyer.RestauranteId);

                // A partir daqui picture se torna uma url e não um 64
                flyer.Picture = $"{Request.Scheme}://{Request.Host}/api/galeria/flyers/{rest.Token}/{flyer.FlyerGuid}.jpg";

                object createResult = _flyerDao.Create(flyer); // Criando no banco
                if (string.IsNullOrEmpty(flyer.Picture)) return Ok(new {
                    errors = false,
                    message = "Registro efetuado"
                });

                string folderPath = $"{hostingEnv.WebRootPath}/galeria/Flyers/{rest.Token}";
                string flyerImageName = $"{flyer.FlyerGuid}.jpg";

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string imagePath = Path.Combine(folderPath, flyerImageName);

                if (System.IO.File.Exists(imagePath)) System.IO.File.Delete(imagePath);

                byte[] imageBytes = Convert.FromBase64String(image64);

                System.IO.File.WriteAllBytes(imagePath, imageBytes);
                return Ok(new {
                    errors = false,
                    message = "Registro efetuado"
                });
            } catch(Exception e)
            {
                return StatusCode(500, new {
                    errors = true,
                    messsage = e.Message
                });
            }
        }

        [HttpDelete("{guid}")]
        public IActionResult Delete(
            Guid guid,
            [FromServices] RestauranteDAO _restDAO,
            [FromServices] IHostingEnvironment hostingEnvironment
        ) {
            try
            {
                Flyer flyer = _flyerDao.GetById(guid);
                Restaurante rest = _restDAO.FindById(flyer.RestauranteId);
                if (!string.IsNullOrEmpty(flyer.Picture)) RemovePicture($"{hostingEnvironment.WebRootPath}/galeria/flyers/{rest.Token}/{guid}.jpg");

                return Ok(_flyerDao.Delete(guid));
            } catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        private void RemovePicture(string picturePath)
        {
            try
            {
                System.IO.File.Delete(picturePath);
            } catch(Exception e)
            {
                throw e;
            }
        }
    }
}
