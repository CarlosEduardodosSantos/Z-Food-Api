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
    public class CartaoConsumoController : Controller
    {
        enum metodos
        {
            Dinheiro = 1,
            Débito = 2,
            Crédito = 3,
            CartãoConsumo = 4,
            Implantação = 5,
            Negativa = 0
        }

        private readonly CartaoConsumoDAO _cartaoDao;

        public CartaoConsumoController(CartaoConsumoDAO cartaoDao)
        {
            _cartaoDao = cartaoDao;
        }

        [HttpGet("obterByConsuId/{Id}")]
        public CartaoConsumo ObterConsulPorId(string Id)
        {
            var data = _cartaoDao.ObterPorId(Id).FirstOrDefault();
            return data;

        }

        [HttpGet("obterByConsuNr/{ResId}/{Id}")]
        public CartaoConsumo ObterPorNr(int ResId, string Id)
        {
            var data = _cartaoDao.ObterPorNr(ResId, Id).FirstOrDefault();
            return data;

        }

        [HttpGet("obterByConsuNrOrName/{ResId}/{Id}/{Nome}")]
        public RootResult ObterPorNomeOuNumero(int ResId, string Id, string Nome)
        {
            var data = _cartaoDao.ObterPorNumeroOrNome(ResId, Id, Nome).ToList();
            var totalPage = 1;
            return new RootResult()
            {
                TotalPage = totalPage,
                Results = data
            };

        }

        [HttpGet("obterByConsuRes/{RestauranteId}")]
        public RootResult ObterPorResId(int RestauranteId)
        {
            var data = _cartaoDao.ObterPorResId(RestauranteId).ToList();
            var totalPage = 1;
            return new RootResult()
            {
                TotalPage = totalPage,
                Results = data
            };

        }

        [HttpGet("obterTodosConsu")]
        public RootResult ObterTodosConsu()
        {
            var data = _cartaoDao.ObterTodosConsu().ToList();
            var totalPage = 1;
            return new RootResult()
            {
                TotalPage = totalPage,
                Results = data
            };

        }

        [HttpPost("adicionarConsul")]
        public object Adicionar([FromBody] CartaoConsumo cartao)
        {
            if (cartao.Valor >= 0)
            {
                try
                {
                    cartao.CartaoConsumoId = cartao.CartaoConsumoId == Guid.Empty ? Guid.NewGuid() : cartao.CartaoConsumoId;
                    _cartaoDao.Insert(cartao);

                    var auditoria = new AuditoriaConsumo()
                    {
                        Historico = "Novo Cartão Registrado " + cartao.Numero,
                        Login = cartao.RegistradoPor,
                        RestauranteId = cartao.RestauranteId
                    };

                    _cartaoDao.InsertAuditoria(auditoria);


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
            else
            {
                return new { errors = true, message = "O cartão está sem saldo!" };
            }

        }

        [HttpPut("zerarCartao/{id}/{login}")]
        public object Alterar(string id, string login)
        {
            try
            {
                var data = _cartaoDao.ObterPorId(id).FirstOrDefault();
                var auditoria = new AuditoriaConsumo()
                {
                    Historico = "Cartão Zerado " + data.Numero,
                    Login = login,
                    RestauranteId = data.RestauranteId,
                    Valor = data.SaldoAtual
                };

                _cartaoDao.ZerarCartao(id);
                _cartaoDao.InsertAuditoria(auditoria);
                return new
                {
                    errors = false,
                    message = "Cartão Zerado."
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

        [HttpDelete("deletarConsu/{id}/{login}")]
        public object Delete(string id, string login)
        {

            try
            {
                var data = _cartaoDao.ObterPorId(id).FirstOrDefault();
                var auditoria = new AuditoriaConsumo()
                {
                    Historico = "Cartão Deletado " + data.Numero,
                    Login = login,
                    RestauranteId = data.RestauranteId 
                };

                _cartaoDao.Delete(id);
                _cartaoDao.InsertAuditoria(auditoria);

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

        //mov


        [HttpGet("obterMovByMovId/{MovId}")]
        public CartaoConsumoMov ObterMovPorMovId(string movId)
        {
            var data = _cartaoDao.ObterPorMovId(movId).FirstOrDefault();

            return data;
        }

        [HttpGet("obterMovPos/{MovId}")]
        public RootResult ObterMovPos(string movId)
        {
            var data = _cartaoDao.ObterMovPositiva(movId).ToList();
            var totalPage = 1;
            return new RootResult()
            {
                TotalPage = totalPage,
                Results = data
            };
        }

        [HttpGet("obterMovNeg/{MovId}")]
        public RootResult ObterMovNeg(string movId)
        {
            var data = _cartaoDao.ObterMovNegativa(movId).ToList();
            var totalPage = 1;
            return new RootResult()
            {
                TotalPage = totalPage,
                Results = data
            };
        }

        [HttpGet("obterMovByConsuId/{ConsuId}")]
        public RootResult ObterMovPorConsuId(string consuId)
        {
            var data = _cartaoDao.ObterMovPorConsuId(consuId).ToList();
            var totalPage = 1;
            return new RootResult()
            {
                TotalPage = totalPage,
                Results = data
            };
        }

        [HttpPost("adicionarMov")]
        public object AdicionarMov([FromBody] CartaoConsumoMovModel cartaoModel)
        {
            try
            {
                var cartao = _cartaoDao.ObterPorNr(cartaoModel.RestauranteId, cartaoModel.NumeroCartao).FirstOrDefault();
                if (cartao == null) { return new { Aproved = false, Mensage = "Cartão Inexistente" }; }


                var valorDesc = cartaoModel.Valor - (cartaoModel.Valor / 100) * cartao.Desconto;
                if (cartaoModel.Metodo == 5)
                {
                    valorDesc = cartaoModel.Valor;
                }
                var soma = cartao.SaldoAtual - valorDesc;
                var valorRetorno = cartao.SaldoAtual;
                var saldo = cartao.SaldoAtual;
                var login = "Consumidor";
                if(cartaoModel.Login != null)
                {
                    login = cartaoModel.Login;
                }
                var imp = "";

                if (cartaoModel.TipoMov == 2)
                {
                    if (soma < 0) { return new { Aproved = false, Mensage = "Saldo indisponível." }; }
                    else
                    {
                        CartaoConsumo datau = new CartaoConsumo();
                        datau.CartaoConsumoId = cartao.CartaoConsumoId;
                        datau.Cpf = cartao.Cpf;
                        datau.Desconto = cartao.Desconto;
                        datau.Descricao = cartao.Descricao;
                        datau.Nome = cartao.Nome;
                        datau.Numero = cartao.Numero;
                        datau.Validade = cartao.Validade;
                        datau.Valor = cartao.Valor;
                        datau.RestauranteId = cartao.RestauranteId;
                        datau.Grupo = cartao.Grupo;
                        datau.RegistradoPor = cartao.RegistradoPor;
                        datau.SaldoAtual = soma;
                        datau.Frete = cartao.Frete;
                        _cartaoDao.Update(datau);
                        valorRetorno = valorDesc;
                        saldo = soma;

                    }

                }

                else if (cartaoModel.TipoMov == 1)
                {
                    var credito = cartao.SaldoAtual + cartaoModel.Valor;

                    CartaoConsumo datau = new CartaoConsumo();
                    datau.CartaoConsumoId = cartao.CartaoConsumoId;
                    datau.Cpf = cartao.Cpf;
                    datau.Desconto = cartao.Desconto;
                    datau.Descricao = cartao.Descricao;
                    datau.Nome = cartao.Nome;
                    datau.Numero = cartao.Numero;
                    datau.Validade = cartao.Validade;
                    datau.Valor = cartao.Valor;
                    datau.RestauranteId = cartao.RestauranteId;
                    datau.Grupo = cartao.Grupo;
                    datau.RegistradoPor = cartao.RegistradoPor;
                    datau.SaldoAtual = credito;
                    datau.Frete = cartao.Frete;
                    _cartaoDao.Update(datau);
                    valorRetorno = cartaoModel.Valor;
                    saldo = credito;
                    login = cartaoModel.Login;

                }


                var cartaoMov = new CartaoConsumoMov()
                {
                    CartaoConsumoMovId = Guid.NewGuid(),
                    CartaoConsumoId = cartao.CartaoConsumoId,
                    DataMov = cartaoModel.DataMov,
                    Saldo = valorRetorno,
                    TipoMov = cartaoModel.TipoMov,
                    Historico = cartaoModel.Historico,
                    UsuarioId = cartaoModel.UsuarioId,
                    Login = login,
                    Metodo = cartaoModel.Metodo
                };



                if(cartaoMov.TipoMov == 2 && cartaoMov.Metodo == 5)
                {
                    imp = " Negativa";
                }
                var auditoria = new AuditoriaConsumo()
                {
                    Historico = "Movimentação Cartão Consumo " + cartao.Numero + " " +(metodos)cartaoMov.Metodo + imp,
                    Login = cartaoMov.Login,
                    Valor = cartaoMov.Saldo,
                    RestauranteId = cartao.RestauranteId
                };

                
               _cartaoDao.InsertMov(cartaoMov);

                var caixa1 = new Caixa1()
                {
                    Nro = cartao.Numero,
                    Historico = "Movimentação Cartão Consumo",
                    Login = cartaoMov.Login,
                    Valor = cartaoMov.Saldo,
                    Metodo = cartaoMov.Metodo,
                    MovId = cartaoMov.CartaoConsumoMovId,
                    RestauranteId = cartao.RestauranteId
                };

                _cartaoDao.InsertCx1(caixa1);
                _cartaoDao.InsertAuditoria(auditoria);
                return new { Aproved = true, Mensage = "Operação realizada com sucesso.", Valor = valorRetorno, Saldo = saldo, Frete = cartao.Frete, Desconto = cartao.Desconto, Cliente = cartao.Nome, MovId = cartaoMov.CartaoConsumoMovId };

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

        [HttpPut("alterarMov")]
        public object AlterarMov([FromBody] CartaoConsumoMov cartao)
        {
            try
            {
                var mov = _cartaoDao.ObterPorMovId(cartao.CartaoConsumoMovId.ToString()).FirstOrDefault();
                if (mov.TipoMov == 2)
                {

                    var data = _cartaoDao.ObterPorId(cartao.CartaoConsumoId.ToString()).FirstOrDefault();
                    var soma = data.SaldoAtual + (mov.Saldo - (mov.Saldo * (data.Desconto / Convert.ToDecimal(100.00)))) - (cartao.Saldo + (cartao.Saldo * (data.Desconto / Convert.ToDecimal(100.00))));
                    if (soma < 0) { return new { errors = true, message = "O cartão está sem saldo!" }; }
                    else
                    {
                        CartaoConsumo datau = new CartaoConsumo();
                        datau.CartaoConsumoId = data.CartaoConsumoId;
                        datau.Cpf = data.Cpf;
                        datau.Desconto = data.Desconto;
                        datau.Descricao = data.Descricao;
                        datau.Nome = data.Nome;
                        datau.Numero = data.Numero;
                        datau.Validade = data.Validade;
                        datau.Valor = data.Valor;
                        datau.RestauranteId = data.RestauranteId;
                        datau.Grupo = data.Grupo;
                        datau.RegistradoPor = datau.RegistradoPor;
                        datau.Frete = datau.Frete;
                        datau.SaldoAtual = soma;
                        _cartaoDao.Update(datau);
                        cartao.TipoMov = 2;
                    }

                }

                else if (mov.TipoMov == 1)
                {
                    var data = _cartaoDao.ObterPorId(cartao.CartaoConsumoId.ToString()).FirstOrDefault();
                    var soma = (data.SaldoAtual - mov.Saldo) + cartao.Saldo;
                    if (soma < 0) { return new { errors = true, message = "O cartão está sem saldo!" }; }


                    else
                    {
                        CartaoConsumo datau = new CartaoConsumo();
                        datau.CartaoConsumoId = data.CartaoConsumoId;
                        datau.Cpf = data.Cpf;
                        datau.Desconto = data.Desconto;
                        datau.Descricao = data.Descricao;
                        datau.Nome = data.Nome;
                        datau.Numero = data.Numero;
                        datau.Validade = data.Validade;
                        datau.Valor = data.Valor;
                        datau.RestauranteId = data.RestauranteId;
                        datau.Grupo = data.Grupo;
                        datau.RegistradoPor = datau.RegistradoPor;
                        datau.Frete = datau.Frete;
                        datau.SaldoAtual = Convert.ToDecimal(soma);
                        _cartaoDao.Update(datau);
                        cartao.TipoMov = 1;
                    }

                }
                _cartaoDao.UpdateMov(cartao);
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


        [HttpDelete("deletarMov/{id}/{login}")]
        public object DeleteMov(string id, string login)
        {

            try
            {
                var mov = _cartaoDao.ObterPorMovId(id).FirstOrDefault();
                var data = _cartaoDao.ObterPorId(mov.CartaoConsumoId.ToString()).FirstOrDefault();
                if (mov.TipoMov == 1)
                {
                    var soma = data.SaldoAtual - mov.Saldo;
                    CartaoConsumo datau = new CartaoConsumo();
                    datau.CartaoConsumoId = data.CartaoConsumoId;
                    datau.Cpf = data.Cpf;
                    datau.Desconto = data.Desconto;
                    datau.Descricao = data.Descricao;
                    datau.Nome = data.Nome;
                    datau.Numero = data.Numero;
                    datau.Validade = data.Validade;
                    datau.Valor = data.Valor;
                    datau.RestauranteId = data.RestauranteId;
                    datau.Grupo = data.Grupo;
                    datau.RegistradoPor = datau.RegistradoPor;
                    datau.Frete = data.Frete;
                    datau.SaldoAtual = Convert.ToDecimal(soma);
                    _cartaoDao.Update(datau);
                }
                else if (mov.TipoMov == 2)
                {
                    var soma = data.SaldoAtual + (mov.Saldo - (mov.Saldo * (data.Desconto / 100)));
                    CartaoConsumo datau = new CartaoConsumo();
                    datau.CartaoConsumoId = data.CartaoConsumoId;
                    datau.Cpf = data.Cpf;
                    datau.Desconto = data.Desconto;
                    datau.Descricao = data.Descricao;
                    datau.Nome = data.Nome;
                    datau.Numero = data.Numero;
                    datau.Validade = data.Validade;
                    datau.Valor = data.Valor;
                    datau.RestauranteId = data.RestauranteId;
                    datau.Grupo = data.Grupo;
                    datau.RegistradoPor = datau.RegistradoPor;
                    datau.Frete = data.Frete;
                    datau.SaldoAtual = Convert.ToDecimal(soma);
                    _cartaoDao.Update(datau);
                }

                _cartaoDao.DeleteMov(id);
                _cartaoDao.DeleteCaix(id);

                var auditoria = new AuditoriaConsumo()
                {
                    Historico = "Estorno Cartão Consumo " + data.Numero,
                    Login = login,
                    Valor = mov.Saldo,
                    RestauranteId = data.RestauranteId
                };
                _cartaoDao.InsertAuditoria(auditoria);

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

        [HttpDelete("deletarConsuCart/{id}")]
        public object DeleteAllMovByCartId(string id)
        {

            try
            {
                _cartaoDao.DeleteAllMovByCartId(id);
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

        [HttpGet("obterCaixaAberto/{data}/{login}/{resId}")]
        public RootResult ObterTodosCx1(DateTime data, string login, int resId)
        {
            var res = _cartaoDao.ObterTodosCx1(data, login, resId).ToList();
            var totalPage = 1;
            return new RootResult()
            {
                TotalPage = totalPage,
                Results = res
            };

        }


        [HttpPut("fecharCaixa/{data}/{login}/{total}/{resId}")]
        public object fecharCaixa(DateTime data, string login, decimal total, int resId)
        {
            try
            {
                var auditoria = new AuditoriaConsumo()
                {
                    Historico = "Fechamento de Caixa",
                    Login = login,  
                    Valor = total,
                    RestauranteId = resId
                };

                _cartaoDao.FecharCx1(data, login, resId);
                _cartaoDao.InsertAuditoria(auditoria);
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

        [HttpGet("obterAuditoria/{resId}")]
        public RootResult ObterAuditoria(int resId)
        {
            var res = _cartaoDao.ObterAuditoriaTodos(resId).ToList();
            var totalPage = 1;
            return new RootResult()
            {
                TotalPage = totalPage,
                Results = res
            };

        }

        [HttpGet("obterAuditoriaData/{data}/{resId}")]
        public RootResult ObterAuditoriaDia(DateTime data, int resId)
        {
            var res = _cartaoDao.ObterAuditoriaDia(data, resId).ToList();
            var totalPage = 1;
            return new RootResult()
            {
                TotalPage = totalPage,
                Results = res
            };

        }
    }
}
