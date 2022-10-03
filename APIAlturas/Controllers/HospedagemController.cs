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
    public class HospedagemController : Controller
    {
        private readonly HospedagemDAO _HospedagemDAO;

        public HospedagemController(HospedagemDAO HospedagemDAO) 
        {
            _HospedagemDAO = HospedagemDAO;
        }

        [HttpGet("obterTodosHosp")]
        public RootResult ObterTodosHosp()
        {
            var data = _HospedagemDAO.ObterTodos().ToList();
            var totalPage = 1;
            return new RootResult()
            {
                TotalPage = totalPage,
                Results = data
            };

        }

        [HttpGet("obterByHospId/{Id}")]
        public Hospedagem ObterHospPorId(string Id)
        {
            var data = _HospedagemDAO.ObterPorId(Id).FirstOrDefault();
            return data;

        }

        [HttpPost("adicionar")]
        public object Adicionar([FromBody] Hospedagem hospedagem)
        {

            try
            {
                hospedagem.Nro = hospedagem.Nro == Guid.Empty ? Guid.NewGuid() : hospedagem.Nro;
                _HospedagemDAO.Insert(hospedagem);
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
        public object Alterar([FromBody] Hospedagem hospedagem)
        {
            try
            {
                _HospedagemDAO.Update(hospedagem);
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
        public object Delete(string id)
        {

            try
            {
                _HospedagemDAO.Delete(id);
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

        //Alimentação

        [HttpGet("obterTodosHospAli")]
        public RootResult ObterTodosAli()
        {
            var data = _HospedagemDAO.ObterTodosAlimentacao().ToList();
            var totalPage = 1;
            return new RootResult()
            {
                TotalPage = totalPage,
                Results = data
            };

        }


        [HttpGet("obterByHospAliById/{Id}")]
        public HospedagemAlimentacao ObterAliPorId(string Id)
        {
            var data = _HospedagemDAO.ObterAlimentacaoPorId(Id).FirstOrDefault();
            return data;

        }

        [HttpGet("obterAliByHosp/{nro}")]
        public RootResult ObterAliByHosp(string nro)
        {
            var data = _HospedagemDAO.ObterAlimentacaoPorNro(nro).ToList();
            var totalPage = 1;
            return new RootResult()
            {
                TotalPage = totalPage,
                Results = data
            };

        }

        [HttpGet("obterAliByCafe/{dataAtual}/{cafe}")]
        public RootResult ObterAliByCafe(bool cafe ,DateTime dataAtual)
        {
            var data = _HospedagemDAO.ObterAlimentacaoPorCafe(cafe, dataAtual).ToList();
            var totalPage = 1;
            return new RootResult()
            {
                TotalPage = totalPage,
                Results = data
            };

        }

        [HttpGet("obterAliByAlmoco/{dataAtual}/{almoco}")]
        public RootResult ObterAliByAlmoco(bool almoco, DateTime dataAtual)
        {
            var data = _HospedagemDAO.ObterAlimentacaoPorAlmoco(almoco, dataAtual).ToList();
            var totalPage = 1;
            return new RootResult()
            {
                TotalPage = totalPage,
                Results = data
            };

        }

        [HttpGet("obterAliByJanta/{dataAtual}/{janta}")]
        public RootResult ObterAliByJanta(bool janta, DateTime dataAtual)
        {
            var data = _HospedagemDAO.ObterAlimentacaoPorJanta(janta, dataAtual).ToList();
            var totalPage = 1;
            return new RootResult()
            {
                TotalPage = totalPage,
                Results = data
            };

        }

        [HttpGet("obterAliByData/{dataAtual}")]
        public RootResult ObterAliByDate(DateTime dataAtual)
        {
            var data = _HospedagemDAO.ObterAlimentacaoPorData(dataAtual).ToList();
            var totalPage = 1;
            return new RootResult()
            {
                TotalPage = totalPage,
                Results = data
            };

        }

        [HttpGet("obterAliByNome/{dataAtual}/{nome}")]
        public RootResult ObterAliByNome(DateTime dataAtual, string nome)
        {
            var data = _HospedagemDAO.ObterAlimentacaoPorNome(dataAtual, nome).ToList();
            var totalPage = 1;
            return new RootResult()
            {
                TotalPage = totalPage,
                Results = data
            };

        }

        [HttpPost("adicionarAli")]
        public object AdicionarAli([FromBody] HospedagemAlimentacao hospedagem)
        {

            try
            {
                hospedagem.AlimentacaoId = hospedagem.AlimentacaoId == Guid.Empty ? Guid.NewGuid() : hospedagem.AlimentacaoId;
                _HospedagemDAO.InsertAlimentacao(hospedagem);
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

        [HttpPut("alterarAli")]
        public object AlterarAli([FromBody] HospedagemAlimentacao hospedagem)
        {
            try
            {
                _HospedagemDAO.UpdateAlimentacao(hospedagem);
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

        [HttpDelete("deletarAli/{id}")]
        public object DeleteAli(string id)
        {

            try
            {
                _HospedagemDAO.DeleteAlimentacao(id);
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

        [HttpGet("obterRelatorio/{dtIni}/{dtFim}")]
        public RootResult ObterRelatorio(DateTime dtIni, DateTime dtFim)
        {
            var data = _HospedagemDAO.ObterPorRelatorio(dtIni, dtFim).ToList();
            var totalPage = 1;
            return new RootResult()
            {
                TotalPage = totalPage,
                Results = data
            };

        }

    }
}
