using System;
using System.Linq;
using APIAlturas.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace APIAlturas.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class EstacionamentoController : Controller
    {
        private readonly EstacionamentoDAO _estacionamentoDao;

        public EstacionamentoController(EstacionamentoDAO estacionamentoDao)
        {
            _estacionamentoDao = estacionamentoDao;
        }


        [HttpGet("obterTodosEst")]
        public RootResult ObterTodosEst()
        {
            var data = _estacionamentoDao.ObterTodos().ToList();
            var totalPage = 1;
            return new RootResult()
            {
                TotalPage = totalPage,
                Results = data
            };

        }

        [HttpGet("obterTodosEstAbertos")]
        public RootResult ObterTodosEstAbertos()
        {
            var data = _estacionamentoDao.ObterTodosAbertos().ToList();
            var totalPage = 1;
            return new RootResult()
            {
                TotalPage = totalPage,
                Results = data
            };

        }


        [HttpGet("obterByEstId/{Id}")]
        public Estacionamento ObterEstById(int Id)
        {
            var data = _estacionamentoDao.ObterPorId(Id).FirstOrDefault();
            return data;

        }


        [HttpGet("obterByDate/{dateR}")]
        public RootResult ObterAbertoPorDia(DateTime dateR)
        {
            var data = _estacionamentoDao.ObterAbertoPorDia(dateR).ToList();
            var totalPage = 1;
            return new RootResult()
            {
                TotalPage = totalPage,
                Results = data
            };

        }

        [HttpPost("adicionar")]
        public object Adicionar([FromBody] Estacionamento est)
        {
                try
                {
                    
                    _estacionamentoDao.Insert(est);
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
        public object Alterar([FromBody] Estacionamento est)
        {
            try
            {
                _estacionamentoDao.Update(est);
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

        [HttpPut("alterarByDate")]
        public object AlterarByDate([FromBody] Estacionamento est)
        {
            try
            {
                _estacionamentoDao.UpdateData(est);
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
                _estacionamentoDao.Delete(id);
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

        //caixa1

        [HttpGet("obterTodosCx1")]
        public RootResult ObterTodosCx1()
        {
            var data = _estacionamentoDao.ObterTodosCx1().ToList();
            var totalPage = 1;
            return new RootResult()
            {
                TotalPage = totalPage,
                Results = data
            };

        }

        [HttpGet("obterByCx1Id/{Id}")]
        public AppCaixa1 ObterCx1ById(int Id)
        {
            var data = _estacionamentoDao.ObterPorIdCx1(Id).FirstOrDefault();
            return data;

        }

        [HttpPost("adicionarCx1")]
        public object AdicionarCx1([FromBody] AppCaixa1 caixa)
        {
            try
            {

                _estacionamentoDao.InsertCx1(caixa);
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

        [HttpPut("alterarCx1")]
        public object AlterarCx1([FromBody] AppCaixa1 caixa)
        {
            try
            {
                _estacionamentoDao.UpdateCx1(caixa);
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

        [HttpPut("alterarCx1Es")]
        public object AlterarCx1Es([FromBody] AppCaixa1 caixa)
        {
            try
            {
                _estacionamentoDao.UpdateCx1Es(caixa);
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

        [HttpDelete("deletarCx1/{id}")]
        public object DeleteCx1(int id)
        {

            try
            {
                _estacionamentoDao.DeleteCx1(id);
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

        //caixa2

        [HttpGet("obterTodosCx2")]
        public RootResult ObterTodosCx2()
        {
            var data = _estacionamentoDao.ObterTodosCx2().ToList();
            var totalPage = 1;
            return new RootResult()
            {
                TotalPage = totalPage,
                Results = data
            };

        }

        [HttpGet("obterByCx2Id/{Id}")]
        public AppCaixa2 ObterCx2ById(int Id)
        {
            var data = _estacionamentoDao.ObterPorIdCx2(Id).FirstOrDefault();
            return data;

        }

        [HttpPost("adicionarCx2")]
        public object AdicionarCx2([FromBody] AppCaixa2 caixa)
        {
            try
            {

                _estacionamentoDao.InsertCx2(caixa);
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

        [HttpPut("alterarCx2")]
        public object AlterarCx2([FromBody] AppCaixa2 caixa)
        {
            try
            {
                _estacionamentoDao.UpdateCx2(caixa);
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

        [HttpDelete("deletarCx2/{id}")]
        public object DeleteCx2(int id)
        {

            try
            {
                _estacionamentoDao.DeleteCx2(id);
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
