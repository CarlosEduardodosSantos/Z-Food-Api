using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing.Imaging;
using System.Net.Mime;
using System.Runtime.InteropServices;
using APIAlturas.ViewModels;
using FreeImageAPI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;

namespace APIAlturas.Controllers
{
    [Route("api/[controller]")]
    public class GaleriaController : Controller
    {
        [HttpGet("uteis/{fileName}")]
        public IActionResult GetImage([FromServices]IHostingEnvironment hostingEnv, string fileName)
        {
            var exactFileName = fileName;
            var imageUrl = $"{hostingEnv.WebRootPath}/Galeria/Uteis/{exactFileName}";
            using (var image =Image.Load(imageUrl, out IImageFormat format))
            {
                image.Mutate(x => x.Resize(256, 256));

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    image.Save(memoryStream, format);
                    return File(memoryStream.ToArray(), "image/jpeg");
                }  
            }
            /*

            if (System.IO.File.Exists(imageUrl))
            {
                Byte[] b = System.IO.File.ReadAllBytes(imageUrl);   // You can use your own method over here.         
                return File(b, "image/jpeg");
            }
            return null;

            */
        }

        [HttpGet("produtos/{fileName}")]
        public IActionResult GetImageProduto([FromServices]IHostingEnvironment hostingEnv, string fileName)
        {
            /*
            const int size = 150;
            var exactFileName = fileName;
            var imageUrl = $"{hostingEnv.WebRootPath}/Galeria/Produtos/{exactFileName}";
            using (var original = FreeImageBitmap.FromFile(imageUrl))
            {
                int width, height;
                if (original.Width > original.Height)
                {
                    width = size;
                    height = original.Height * size / original.Width;
                }
                else
                {
                    width = original.Width * size / original.Height;
                    height = size;
                }
                var resized = new FreeImageBitmap(original, width, height);
                // JPEG_QUALITYGOOD is 75 JPEG.
                // JPEG_BASELINE strips metadata (EXIF, etc.)
                resized.Save(OutputPath(path, outputDirectory, FreeImage), FREE_IMAGE_FORMAT.FIF_JPEG,
                    FREE_IMAGE_SAVE_FLAGS.JPEG_QUALITYGOOD |
                    FREE_IMAGE_SAVE_FLAGS.JPEG_BASELINE);
            }*/


            //var imageUrl = $"{hostingEnv.WebRootPath}/Galeria/Produtos/{exactFileName}";
            var exactFileName = fileName;
            var imageUrl = $"{hostingEnv.WebRootPath}/Galeria/Produtos/{exactFileName}";
            if (System.IO.File.Exists(imageUrl))
            {
                Byte[] b = System.IO.File.ReadAllBytes(imageUrl);   // You can use your own method over here.         
                return File(b, "image/jpeg");
            }
            return null;

            var pathToFolder = $"{hostingEnv.WebRootPath}/Galeria/Produtos/";
            foreach (string file in Directory.EnumerateFiles(
                pathToFolder,
                "*",
                SearchOption.AllDirectories)
            )
            {
                // do something

            }
        }

        [HttpGet("flyers/{restToken}/{fileName}")]
        public IActionResult GetFlyers(
            string restToken, string fileName,
            [FromServices] RestauranteDAO _restDAO,
            [FromServices] IHostingEnvironment hostingEnvironment
        ) {
            try
            {
                string imgUrl = $"{hostingEnvironment.WebRootPath}/Galeria/Flyers/{restToken}/{fileName}";
                System.Diagnostics.Debug.WriteLine(imgUrl);
                if (System.IO.File.Exists(imgUrl))
                {
                    byte[] imageBytes = System.IO.File.ReadAllBytes(imgUrl);
                    return File(imageBytes, "image/jpeg");
                }
                return Ok(null);
            } catch(Exception e)
            {
                return Ok(e.Message);
                /*return StatusCode(500, new { 
                    errors = true,
                    message = e.Message
                });*/
            }
        }

        [HttpGet("listaprodutos/{fileName}")]
        public RootResult GetListaProduto([FromServices]IHostingEnvironment hostingEnv)
        {
            var result = new List<string>();
            var pathToFolder = $"{hostingEnv.WebRootPath}/Galeria/Produtos/";
            foreach (string file in Directory.EnumerateFiles(pathToFolder, "*", SearchOption.AllDirectories))
            {
                result.Add(file);

            }

            return new RootResult()
            {
                Results = result
            };
        }


    }
}