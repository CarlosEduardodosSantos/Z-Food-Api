using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using APIAlturas.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;

namespace APIAlturas.Controllers
{
    [Route("api/[controller]")]
    public class ProdutoController : Controller
    {
        private readonly ProdutoDao _produtoDao;
        private readonly ComplementoDao _complementoDao;
        public ProdutoController([FromServices]ProdutoDao produtoDao, ComplementoDao complementoDao)
        {
            _produtoDao = produtoDao;
            _complementoDao = complementoDao;
        }
        [HttpGet("produtoscadastro/{token}/{limit}/{page}")]
        public RootResult GetProdutosCadastro(string token, int limit, int page)
        {
            var data = _produtoDao.GetProdutosCadastro(token).ToList();
            var totalPage = (int)Math.Ceiling((double)data.Count / limit);
            return new RootResult()
            {
                TotalPage = totalPage,
                Results = data.Skip((page - 1) * limit).Take(limit).ToList<object>()
            };
        }

        [HttpGet("produtos/{restauranteId}/{limit}/{page}")]
        public RootResult GetProdutos(string restauranteId, int limit, int page)
        {
            var data = _produtoDao.GetProdutos(restauranteId).ToList();
            var extras = _produtoDao.GetPromocoes(restauranteId).ToList();
            var totalPage = (int)Math.Ceiling((double)data.Count / limit);
            return new RootResult()
            {
                TotalPage = totalPage,
                Results = data.Skip((page - 1) * limit).Take(limit).ToList<object>(),
                Extras = extras
            };
        }


        [HttpGet("getByName/{name}/{limit}/{page}")]
        public RootResult GetByName(string name, int limit, int page)
        {
            var data = _produtoDao.GetByName(name).ToList();
            var totalPage = (int)Math.Ceiling((double)data.Count / limit);
            return new RootResult()
            {
                TotalPage = totalPage,
                Results = data.Skip((page - 1) * limit).Take(limit).ToList<object>(),
                Extras = null
            };
        }


        [HttpGet("getByCategoriaId/{restauranteId}/{categoriaId}/{limit}/{page}")]
        public RootResult getByCategoriaId(string restauranteId, string categoriaId, int limit, int page)
        {
            var produtoGrupos = new List<ProdutosGrupo>();
            var data = _produtoDao.GetByCategoriaId(restauranteId, categoriaId, limit, page);

            var totalPage = (int)Math.Ceiling((double)data.produtos.Count / limit);

            produtoGrupos.Add(data);
            return new RootResult()
            {
                TotalPage = totalPage,
                Results = produtoGrupos,
                Extras = null
            };
        }

        [HttpGet("getByCard/{restauranteId}/{limit}/{page}")]
        public RootResult getByCard(string restauranteId, int limit, int page)
        {
            var produtos = _produtoDao.GetProdutoByCard(restauranteId).ToList();

            var totalPage = (int)Math.Ceiling((double)produtos.Count / limit);

            
            return new RootResult()
            {
                TotalPage = totalPage,
                Results = produtos.Skip((page - 1) * limit).Take(limit).ToList<object>(),
                Extras = null
            };
        }


        [HttpGet("byCategorias/{grupoId}/{restauranteId}/{limit}/{page}")]
        public RootResult GetByCategorias([FromServices] GrupoProdutoDao grupoProdutoDao, string grupoId, string restauranteId, int limit, int page)
        {
            var grupo = grupoProdutoDao.ObterPorId(grupoId);

            var categoriaIds = grupo.Categorias.Select(t => t.CategoriaId.ToString()).ToArray();
            
            var data = _produtoDao.GetByCategorias(categoriaIds, restauranteId).ToList();
            var extras = _produtoDao.GetDestaqueByCategorias(categoriaIds, restauranteId).ToList();
            var totalPage = (int)Math.Ceiling((double)data.Count / limit);
            return new RootResult()
            {
                TotalPage = totalPage,
                Results = data.Skip((page - 1) * limit).Take(limit).ToList<object>(),
                Extras = extras
            };
        }


        [HttpGet("destaques/{restauranteId}/{limit}/{page}")]
        public RootResult GetDestaque(string restauranteId, int limit, int page)
        {
            var extras = _produtoDao.GetPromocoes(restauranteId).ToList();
            var totalPage = (int)Math.Ceiling((double)extras.Count / limit);
            return new RootResult()
            {
                TotalPage = totalPage,
                Results = extras.Skip((page - 1) * limit).Take(limit).ToList<object>(),
                Extras = extras
            };
        }

        [HttpGet("complementos/{categoriaId}")]
        public List<Complemento> GetComplementos(string categoriaId)
        {
            return _produtoDao.GetComplementos(categoriaId).ToList();
        }
        [HttpGet("meiomeios/{restauranteId}/{produtoId}/{limit}/{page}")]
        public RootResult GetMeioMeios(string restauranteId, int produtoId, int limit, int page)
        {
            var data = _produtoDao.GetMeioMeios(restauranteId, produtoId).ToList();
            var totalPage = (int)Math.Ceiling((double)data.Count);
            return new RootResult()
            {
                TotalPage = totalPage,
                Results = data.Skip((page - 1) * limit).Take(limit).ToList<object>()
            };
        }

        [HttpPost("player")]
        public object PlayerProduto([FromBody] PlayersClick playersClick)
        {
            _produtoDao.AddPlayerProduto(playersClick);
            return new
            {
                errors = false,
                message = "Cadastro efetuado com sucesso."
            };
        }

        [HttpGet("image/{token}/{fileName}")]
        public IActionResult GetImage([FromServices]IHostingEnvironment hostingEnv, string token, string fileName)
        {
            var exactFileName = fileName;
            var imageUrl = $"{hostingEnv.WebRootPath}/Produtos/{token}/{exactFileName}";

            using (var image = Image.Load(imageUrl, out IImageFormat format))
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

        [HttpPost("copiar")]
        public Produto Copiar([FromBody] ProdutoAdicionarViewModel produto,
            [FromServices] ProdutoOpcaoDao produtoOpcaoDao,
            [FromServices] IHostingEnvironment hostingEnv)
        {
            var produtoId = produto.ProdutoId;

            produto.Nome += " (Cópia)";
            produto.ProdutoGuid = Guid.NewGuid();
            produto.ReferenciaId = 0;
            produto.Imagem = String.Empty;

            _produtoDao.Incluir(produto);

            var produtoCopia = _produtoDao.GetProdutoPorGuid(produto.ProdutoGuid.ToString());
            //produtoOpcaoDao.Insert();
            var complementares = produtoOpcaoDao.GetByProdutoId(produtoCopia.RestauranteId.ToString(), produtoId);
            foreach (var produtoOpcaoTipo in complementares)
            {
                foreach (var produtoOpcao in produtoOpcaoTipo.ProdutoOpcaos)
                {
                    produtoOpcao.ProdutoId = produtoCopia.ProdutoId;
                    produtoOpcao.ProdutosOpcaoId = Guid.NewGuid();
                    produtoOpcaoDao.Insert(produtoOpcao);
                }
            }
            return produtoCopia;
        }

        [HttpPost("adicionar")]
        public object Adicionar([FromBody] ProdutoAdicionarViewModel produto, 
            [FromServices]RestauranteDAO restauranteDao,
            [FromServices]IHostingEnvironment hostingEnv)
        {
            var token = produto.TokenRestaurante;
            var restaurante = restauranteDao.FindByToken(token);
            if (restaurante?.RestauranteId == null)
            {
                return new
                {
                    errors = true,
                    message = "Restaurante não encontrado."
                };
            }

            produto.RestauranteId = restaurante.RestauranteId;
            var isExists = _produtoDao.GetProdutoPorGuid(produto.ProdutoGuid.ToString()) != null;

            if(!isExists)
            {
                produto.ProdutoGuid  = produto.ProdutoGuid == Guid.Empty ? Guid.NewGuid() : produto.ProdutoGuid;
                produto.Imagem = $"produto/image/{token}/{produto.ProdutoGuid}.jpg";
                _produtoDao.Incluir(produto);

            }
            else
            {
                if(produto.ProdutoGuid == Guid.Empty)
                    produto.ProdutoGuid = Guid.NewGuid();

                produto.Imagem = $"produto/image/{token}/{produto.ProdutoGuid}.jpg";
                _produtoDao.Alterar(produto);
            }

            //Grava Imagem

            if(string.IsNullOrEmpty(produto.ImagemBase64))
                return new
                {
                    errors = false,
                    message = "Cadastro efetuado com sucesso."
                };


            var imagePath = $"{hostingEnv.WebRootPath}/Produtos/{token}";
            var imageName = $"{produto.ProdutoGuid}.jpg";

            if (!Directory.Exists(imagePath))
            {
                Directory.CreateDirectory(imagePath);
            }

            //set the image path
            string imgPath = Path.Combine(imagePath, imageName);

            if(System.IO.File.Exists(imgPath))
                System.IO.File.Delete(imgPath);

            byte[] imageBytes = Convert.FromBase64String(produto.ImagemBase64);

            try
            {
                System.IO.File.WriteAllBytes(imgPath, imageBytes);

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

        [HttpPost("excluir")]
        public object Excluir([FromBody] ProdutoAdicionarViewModel produtoView,
            [FromServices] RestauranteDAO restauranteDao,
            [FromServices] IHostingEnvironment hostingEnv)
        {
            var token = produtoView.TokenRestaurante;
            var restaurante = restauranteDao.FindByToken(token);
            if (restaurante?.RestauranteId == null)
            {
                return new
                {
                    errors = true,
                    message = "Restaurante não encontrado."
                };
            }

            var produto = _produtoDao.GetProdutoPorId(produtoView.ProdutoId);
            if (produto == null)
            {
                return new
                {
                    errors = true,
                    message = "Produto não encontrado."
                };

            }
            
            _produtoDao.Excluir(produto.ProdutoId);

            return new
            {
                errors = false,
                message = "Produto excluido com sucesso."
            };

        }

        [HttpPost("addComplemento")]
        public object AddComplemento([FromBody] Complemento complemento,
            [FromServices] RestauranteDAO restauranteDao,
            [FromServices] IHostingEnvironment hostingEnv)
        {
            var restaurante = restauranteDao.FindByToken(complemento.TokenRestaurante.ToString());
            if (restaurante?.RestauranteId == null)
            {
                return new
                {
                    errors = true,
                    message = "Restaurante não encontrado."
                };
            }
            complemento.RestauranteId = restaurante.RestauranteId;

            var jaExite = _complementoDao.GetById(complemento.ComplementoId) != null;
            if (jaExite)
            {
                return new
                {
                    errors = true,
                    message = "Complemetno já existente."
                };
            }


            _complementoDao.Insert(complemento);
            
            return new
            {
                errors = false,
                message = "Complemento adicionada com sucesso."
            };
        }

        [HttpPost("removerComplemento")]
        public object RemoverComplemento([FromBody] Complemento complemento,
            [FromServices] RestauranteDAO restauranteDao,
            [FromServices] IHostingEnvironment hostingEnv)
        {
            var restaurante = restauranteDao.FindByToken(complemento.TokenRestaurante.ToString());
            if (restaurante?.RestauranteId == null)
            {
                return new
                {
                    errors = true,
                    message = "Restaurante não encontrado."
                };
            }
            complemento.RestauranteId = restaurante.RestauranteId;

            var jaExite = _complementoDao.GetById(complemento.ComplementoId) != null;
            if (!jaExite)
            {
                return new
                {
                    errors = true,
                    message = "Complemento não cadastrado para esse grupo."
                };
            }


            _complementoDao.Delete(complemento);

            return new
            {
                errors = false,
                message = "Complemento excluido com sucesso."
            };
        }

        /// <summary>
        /// Rota para pesquisar produtos através do Id do restaurante e de uma string
        /// </summary>
        [HttpGet("filter/{restId}/{filter}")]
        public IActionResult FilterProds(string restId, string filter)
        {
            try
            {
                IEnumerable<GrupoProduto> categorias = this._produtoDao.FilterProds(restId, filter);
                return Ok(categorias);
            } catch(Exception e)
            {
                if (e.Message == "400") return BadRequest("Campos inválidos");
                else return StatusCode(500);
            }
        }

        /// <summary>
        /// Devolve os produtos com ID de um categoria
        /// </summary>
        [HttpGet("byCategoryId/{catId}")]
        public IActionResult getByCategories(int catId)
        {
            try
            {
                return Ok(this._produtoDao.getByCategoryId(catId));
            } catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("lazy/{restId}/{quantity}/{catId}/{lastProdName}")]
        public IActionResult getLazy(int restId, int quantity, int catId, string lastProdName)
        {
            try
            {
                return Ok(this._produtoDao.getLazy(restId, quantity, catId, lastProdName));
            } catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}