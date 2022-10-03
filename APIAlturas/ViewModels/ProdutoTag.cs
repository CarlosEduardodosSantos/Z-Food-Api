using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIAlturas.ViewModels
{
    public class ProdutoTag
    {
        public Guid TagsId { get; set; }
        public Guid ProdutoGuid { get; set; }
        public string Tag { get; set; }
    }
}
