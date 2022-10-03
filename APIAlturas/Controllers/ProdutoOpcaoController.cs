using System;
using System.Linq;
using APIAlturas.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace APIAlturas.Controllers
{
    [Produces("application/json")]
    [Route("api/produtoopcao")]
    public class ProdutoOpcaoController : Controller
    {
        private readonly ProdutoOpcaoDao _produtoOpcaoDao;

        public ProdutoOpcaoController(ProdutoOpcaoDao produtoOpcaoDao)
        {
            _produtoOpcaoDao = produtoOpcaoDao;
        }

        [HttpGet("obterByProdutoId/{restauranteId}/{produtoId}")]
        public RootResult ObterByProdutoId(string restauranteId, int produtoId)
        {
            var data = _produtoOpcaoDao.GetByProdutoId(restauranteId, produtoId).Where(t => t.Situacao == 1).ToList();
            return new RootResult()
            {
                TotalPage = 1,
                Results = data
            };
        }

        [HttpGet("obterByGestor/{restauranteId}/{produtoId}")]
        public RootResult ObterByGestor(string restauranteId, int produtoId)
        {
            var data = _produtoOpcaoDao.GetByGestor(restauranteId, produtoId).ToList();
            return new RootResult()
            {
                TotalPage = 1,
                Results = data
            };
        }

        [HttpGet("obteTiposByGestor/{restauranteId}")]
        public RootResult ObterTiposByGestor(string restauranteId)
        {
            var data = _produtoOpcaoDao.ObterTipoByRestaurante(restauranteId).ToList();
            return new RootResult()
            {
                TotalPage = 1,
                Results = data
            };
        }


        [HttpGet("obterByTipos/{restauranteId}")]
        public RootResult ObterByTipos(string restauranteId)
        {
            var data = _produtoOpcaoDao.ObterTipoByRestaurante(restauranteId).Where(t => t.Situacao == 1).ToList();
            return new RootResult()
            {
                TotalPage = 1,
                Results = data
            };
        }

        [HttpPost("adicionar")]
        public object Adicionar([FromBody] ProdutoOpcao produtoOpcao)
        {
            try
            {
                _produtoOpcaoDao.Insert(produtoOpcao);

                var relacao = new ProdutosOpcaoTipoRelacao()
                {
                    Id = Guid.NewGuid(),
                    ProdutosOpcaoId = produtoOpcao.ProdutosOpcaoId,
                    ProdutoId = produtoOpcao.ProdutoId
                };

                _produtoOpcaoDao.Relacionar(relacao);

                if (produtoOpcao.Replicar)
                {
                    var relacoes = _produtoOpcaoDao.ObterProdutoOpcaoRelacao(produtoOpcao.ProdutosOpcaoTipoId);
                    foreach (var item in relacoes)
                    {
                        if (produtoOpcao.ProdutoId == item) continue;

                        relacao = new ProdutosOpcaoTipoRelacao()
                        {
                            Id = Guid.NewGuid(),
                            ProdutosOpcaoId = produtoOpcao.ProdutosOpcaoId,
                            ProdutoId = item
                        };

                        _produtoOpcaoDao.Relacionar(relacao);

                    }
                }

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

        [HttpPost("relacionar")]
        public object Relacionar([FromBody] ProdutosOpcaoTipoRelacao produtosOpcaoTipoRelacao)
        {
            try
            {
                _produtoOpcaoDao.Relacionar(produtosOpcaoTipoRelacao);
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
        public object Alterar([FromBody] ProdutoOpcao produtoOpcao)
        {
            try
            {
                _produtoOpcaoDao.Alterar(produtoOpcao);
                return new
                {
                    errors = false,
                    message = "Cadastro alterado com sucesso."
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
                _produtoOpcaoDao.Deletar(id);
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

        [HttpDelete("deletarRelacao/{produtosOpcaoId}/{produtoId}")]
        public object DeletarRelacao(string produtosOpcaoId, int produtoId)
        {
            try
            {
                _produtoOpcaoDao.DeletarRelacao(produtosOpcaoId, produtoId);
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

        [HttpPost("adicionarTipo")]
        public object AdicionarTipo([FromBody] ProdutoOpcaoTipo produtoOpcaoTipo)
        {
            try
            {
                _produtoOpcaoDao.InsertTipo(produtoOpcaoTipo);
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
        [HttpPut("alterarTipo")]
        public object AlterarTipo([FromBody] ProdutoOpcaoTipo produtoOpcaoTipo)
        {
            try
            {
                _produtoOpcaoDao.AlterarTipo(produtoOpcaoTipo);
                return new
                {
                    errors = false,
                    message = "Cadastro alterado com sucesso."
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