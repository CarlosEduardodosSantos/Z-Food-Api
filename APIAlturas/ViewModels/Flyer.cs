using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIAlturas.ViewModels
{
    public class Flyer
    {
        public Flyer()
        {
            FlyerGuid = Guid.NewGuid();
        }
        public Guid FlyerGuid { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public string Picture { get; set; }
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }
        public int RestauranteId { get; set; }
    }
}
