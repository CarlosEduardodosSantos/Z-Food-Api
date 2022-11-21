using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIAlturas.ViewModels;

namespace APIAlturas.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UsuarioCartaoConsumoController : Controller
    {
        private readonly CartaoConsumoDAO _cartaoDao;
        private readonly UsuariosCartaoConsumoDao _usuariosDAL;

        public UsuarioCartaoConsumoController(UsuariosCartaoConsumoDao usuariosDAL, CartaoConsumoDAO cartaoDao)
        {
            _usuariosDAL = usuariosDAL;
            _cartaoDao = cartaoDao;
        }


        [HttpGet("obterLogin/{Login}/{Senha}/{resId}")]
        public UsuariosCartaoConsumoModel ObterLoginPorId(string Login, string Senha, int resId)
        {

                var data = _usuariosDAL.ObterUserPorSenha(Login, Senha, resId).FirstOrDefault();
                return data;

        }

        [HttpGet("obterPorCodigo/{usuarioid}/{ResId}")]
        public UsuariosCartaoConsumoModel ObterUsuarioPorCodigo(int usuarioid, int ResId)
        {
                var data = _usuariosDAL.ObterUserPorCodigo(usuarioid, ResId).FirstOrDefault();
                return data;
            
        }

        [HttpGet("obterTodos/{ResId}")]
        public RootResult ObterTodos(int ResId)
        {
                var data = _usuariosDAL.ObterTodos(ResId).ToList();
                var totalPage = 1;
                return new RootResult()
                {
                    TotalPage = totalPage,
                    Results = data
                };

        }

        [HttpPost("adicionar")]
        public object Adicionar([FromBody] UsuariosCartaoConsumoModel usuario)
        {

                try
                {
                    _usuariosDAL.Insert(usuario);
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
        public object Alterar([FromBody] UsuariosCartaoConsumoModel usuario)
        {
                try
                {
                    _usuariosDAL.Update(usuario);
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

        [HttpDelete("deletar/{codigo}")]
        public object Delete(int codigo)
        {


                try
                {
                    _usuariosDAL.Delete(codigo);
                    return new
                    {
                        errors = false,
                        message = "Exclusão efetuada com sucesso."
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
