using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIAlturas.ViewModels
{
    public class RestauranteRating
    {
        public RestauranteRating()
        {
            RestauranteRatingGuid = Guid.NewGuid();
            DataHora = DateTime.Now;
        }
        public Guid RestauranteRatingGuid { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int Value { get; set; }
        public string Suggestion { get; set; }
        public DateTime? DataHora { get; set; }
        public int RestauranteId { get; set; }
    }
}
