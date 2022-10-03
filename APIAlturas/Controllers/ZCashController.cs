using System.Linq;
using APIAlturas.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace APIAlturas.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ZCashController : Controller
    {
        private readonly ZcashMovimentacaoDao _movimentacaoDao;

        public ZCashController(ZcashMovimentacaoDao movimentacaoDao)
        {
            _movimentacaoDao = movimentacaoDao;
        }

        [HttpGet("obterByUsuario/{usuarioId}")]
        public RootResult ObterByUsuarioId(string usuarioId)
        {
            var data = _movimentacaoDao.ObterPorUsuarioId(usuarioId).ToList();
            var totalPage = 1;
            return new RootResult()
            {
                TotalPage = totalPage,
                Results = data
            };
        }
    }
}