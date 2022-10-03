using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIAlturas.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIAlturas.Controllers
{
    [Produces("application/json")]
    [Route("api/Setores")]
    public class SetoresController : Controller
    {

        [HttpGet("todos/{limit}/{page}")]
        public RootResult ObterSetores(int limit, int page, 
            [FromServices]SetorDao setorDao)
        {
            var data = setorDao.ObterTodos().ToList();
            var totalPage = (int)Math.Ceiling((double)data.Count / limit);
            return new RootResult()
            {
                TotalPage = totalPage,
                Results = data.Skip((page - 1) * limit).Take(limit).ToList<object>()
            };
        }
    }
}