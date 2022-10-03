using System;
using System.Linq;
using APIAlturas.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace APIAlturas.Controllers
{
    [Produces("application/json")]
    [Route("api/Categoria")]
    public class CategoriaController : Controller
    {
        private readonly CategoriaDAO _categoriaDao;

        public CategoriaController(CategoriaDAO categoriaDao)
        {
            _categoriaDao = categoriaDao;
        }

        [HttpGet("todas/{token}/{limit}/{page}")]
        public RootResult ObterCategoriasByRestauranteId(string token, int limit, int page)
        {
            var data = _categoriaDao.GetByRestauranteToken(token).ToList();
            var totalPage = (int)Math.Ceiling((double)data.Count / limit);
            return new RootResult()
            {
                TotalPage = totalPage,
                Results = data.Skip((page - 1) * limit).Take(limit).ToList<object>()
            };
        }

        [HttpGet("getByGrupoId/{restauranteId}/{grupoId}/{limit}/{page}")]
        public RootResult ObterByGrupoId([FromServices] GrupoProdutoDao grupoProdutoDao,  string restauranteId, string grupoId, int limit, int page)
        {
            var grupo = grupoProdutoDao.ObterPorId(grupoId);

            var categoriaIds = grupo.Categorias.Select(t => t.CategoriaId.ToString()).ToArray();

            var data = _categoriaDao.GetByGrupoId(restauranteId, categoriaIds).ToList();
            var totalPage = (int)Math.Ceiling((double)data.Count / limit);
            return new RootResult()
            {
                TotalPage = totalPage,
                Results = data.Skip((page - 1) * limit).Take(limit).ToList<object>()
            };
        }
        [HttpPost("adiciona")]
        public object Adicionar([FromBody] Categoria categoria,
            [FromServices]RestauranteDAO restauranteDao)
        {
            var restaurante = restauranteDao.FindByToken(categoria.RestauranteToken.ToString());
            if (restaurante?.RestauranteId == null)
            {
                return new
                {
                    errors = true,
                    message = "Restaurante não encontrado."
                };
            }
            categoria.RestauranteId = restaurante.RestauranteId;
            
            var jaExite = _categoriaDao.GetByDescricao(categoria.Descricao.Trim(), categoria.RestauranteId).Any();
            if (jaExite)
            {
                return new
                {
                    errors = true,
                    message = "Categoria já existente."
                };
            }


            _categoriaDao.Insert(categoria);
            return new
            {
                errors = false,
                message = "Categoria adicionada com sucesso."
            };
        }

        [HttpPut("alterar")]
        public object Alterar([FromBody] Categoria categoria,
            [FromServices]RestauranteDAO restauranteDao)
        {
            var restaurante = restauranteDao.FindByToken(categoria.RestauranteToken.ToString());
            if (restaurante?.RestauranteId == null)
            {
                return new
                {
                    errors = true,
                    message = "Restaurante não encontrado."
                };
            }

            categoria.RestauranteId = restaurante.RestauranteId;

            var jaExite = _categoriaDao.GetById(categoria.CategoriaId).Any();
            if (!jaExite)
            {
                return new
                {
                    errors = true,
                    message = "Categoria não encontrada."
                };
            }


            _categoriaDao.Alterar(categoria);
            return new
            {
                errors = false,
                message = "Categoria adicionada com sucesso."
            };
        }
    }
}