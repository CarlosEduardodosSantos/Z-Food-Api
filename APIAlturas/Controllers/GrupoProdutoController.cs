using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using APIAlturas.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace APIAlturas.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class GrupoProdutoController : Controller
    {
        private readonly GrupoProdutoDao _grupoProdutoDao;

        public GrupoProdutoController(GrupoProdutoDao grupoProdutoDao)
        {
            _grupoProdutoDao = grupoProdutoDao;
        }

        [HttpGet("obterByRestauranteId/{restauranteId}")]
        public RootResult ObterByToken(string restauranteId)
        {
            var data = _grupoProdutoDao.ObterPorRestauranteId(restauranteId).ToList();
            var totalPage = 1;
            return new RootResult()
            {
                TotalPage = totalPage,
                Results = data
            };
        }
        [HttpPost("adicionar")]
        public object Adicionar([FromServices]IHostingEnvironment hostingEnv, [FromBody] GrupoProduto grupo)
        {
            string imgZimmer = new String(grupo.ImagemZimmer);
            grupo.ImagemZimmer = $"{Request.Scheme}://{Request.Host}/api/Galeria/Uteis/{grupo.GupoId}-zimmer.jpg";
            _grupoProdutoDao.Adicionar(grupo);

            //Grava Imagem
            var imagePath = $"{hostingEnv.WebRootPath}/Galeria/Uteis/";
            var imageName = $"{grupo.GupoId}.jpg";
            // Imagem do Zimmer
            string imageNameZimmer = $"{grupo.GupoId}-zimmer.jpg";

            if (!Directory.Exists(imagePath))
            {
                Directory.CreateDirectory(imagePath);
            }


            //set the image path
            string imgPath = Path.Combine(imagePath, imageName);
            // Monta o caminho da imagem do zimmer
            string imgPathZimmer = Path.Combine(imagePath, imageNameZimmer);

            if (System.IO.File.Exists(imgPath))
                System.IO.File.Delete(imgPath);
            if (System.IO.File.Exists(imgPathZimmer))
                System.IO.File.Delete(imgPathZimmer);

            byte[] imageBytes = Convert.FromBase64String(grupo.ImagemBase64);
            byte[] imageZimmerBytes = Convert.FromBase64String(imgZimmer);

            try
            {
                System.IO.File.WriteAllBytes(imgPath, imageBytes);
                System.IO.File.WriteAllBytes(imgPathZimmer, imageZimmerBytes);


                return new
                {
                    errors = false,
                    message = "Cadastro efetuado com sucesso."
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


            return new
            {
                errors = false,
                message = "Grupo adicionada com sucesso."
            };
        }
        [HttpPut("alterar")]
        public object Alterar([FromServices]IHostingEnvironment hostingEnv, [FromBody] GrupoProduto grupo)
        {
            var url = this.Request.Host;
            var shema = this.Request.Scheme;

            string ImagemBase64 = new String(grupo.ImagemZimmer);

            grupo.Imagem = $"{shema}://{url}/api/Galeria/Uteis/{grupo.GupoId}.jpg";
            grupo.ImagemZimmer = $"{shema}://{url}/api/Galeria/Uteis/{grupo.GupoId}-zimmer.jpg";

            _grupoProdutoDao.Alterar(grupo);

            //Grava Imagem
            var imagePath = $"{hostingEnv.WebRootPath}/Galeria/Uteis/";
            var imageName = $"{grupo.GupoId}.jpg";
            // Imagem do Zimmer
            string imageNameZimmer = $"{grupo.GupoId}-zimmer.jpg";

            //set the image path
            string imgPath = Path.Combine(imagePath, imageName);
            string imgPathZimmer = Path.Combine(imagePath, imageNameZimmer);

            if (!Directory.Exists(imagePath))
            {
                Directory.CreateDirectory(imagePath);
            }

            if (System.IO.File.Exists(imgPath))
                System.IO.File.Delete(imgPath);
            if (System.IO.File.Exists(imgPathZimmer))
                System.IO.File.Delete(imgPathZimmer);

            byte[] imageBytes = Convert.FromBase64String(grupo.ImagemBase64);
            byte[] imageZimmerBytes = Convert.FromBase64String(ImagemBase64);

            try
            {
                System.IO.File.WriteAllBytes(imgPath, imageBytes);
                System.IO.File.WriteAllBytes(imgPathZimmer, imageZimmerBytes);


                return new
                {
                    errors = false,
                    message = "Cadastro efetuado com sucesso."
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

        [HttpDelete("deletar/{id}")]
        public object Deletar(string id)
        {
            try
            {
                _grupoProdutoDao.Excluir(id);
                return new
                {
                    errors = false,
                    message = "Cadastro excluido com sucesso."
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


        [HttpPost("relacionar")]
        public object Relacionar([FromBody] GrupoCategoriaRelacao relacao)
        {
            _grupoProdutoDao.RelacaoGrupoCategoria(relacao);
            return new
            {
                errors = false,
                message = "Categoria relacionada com sucesso."
            };
        }

        [HttpDelete("deletarRelacao/{id}")]
        public object DeletarRelacao(string id)
        {
            try
            {
                _grupoProdutoDao.ExcluirRelacao(id);
                return new
                {
                    errors = false,
                    message = "Cadastro excluido com sucesso."
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

        [HttpGet("byRestId/{restId}")]
        public IActionResult getByRestId(int restId)
        {
            try
            {
                if (Request.Headers.TryGetValue("deepLevel", out var deepLevel)) return Ok(this._grupoProdutoDao.getGroupsByRestId(restId, int.Parse(deepLevel)));
                else return Ok(this._grupoProdutoDao.getGroupsByRestId(restId));
            } catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}