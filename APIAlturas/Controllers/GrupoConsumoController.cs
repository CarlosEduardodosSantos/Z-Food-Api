using System;
using System.Collections.Generic;
using System.Linq;
using APIAlturas.Service;
using APIAlturas.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace APIAlturas.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class GrupoConsumoController : Controller
    {
        private readonly GrupoConsumoDao _grupoDao;

        public GrupoConsumoController(GrupoConsumoDao grupoDao)
        {
            _grupoDao = grupoDao;
        }

        [HttpGet("obterByGrupoId/{id}")]
        public GrupoConsumo ObterConsulPorId(int id)
        {
            var data = _grupoDao.ObterPorId(id).FirstOrDefault();
            return data;

        }

        [HttpGet("obterTodosGrupo/{resId}")]
        public RootResult ObterTodosConsu(int resId)
        {
            var data = _grupoDao.ObterTodos(resId).ToList();
            var totalPage = 1;
            return new RootResult()
            {
                TotalPage = totalPage,
                Results = data
            };

        }

        [HttpPost("adicionar")]
        public object Adicionar([FromBody] GrupoConsumo grupo)
        {
            try
            {

                _grupoDao.Insert(grupo);
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

        [HttpPut("alterar")]
        public object Alterar([FromBody] GrupoConsumo grupo)
        {
            try
            {
                _grupoDao.Update(grupo);
                return new
                {
                    errors = false,
                    message = "Cadastro atualizado com sucesso."
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
        public object Delete(int id)
        {

            try
            {
                _grupoDao.Delete(id);
                return new
                {
                    errors = false,
                    message = "Cadastro deletado com sucesso."
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
