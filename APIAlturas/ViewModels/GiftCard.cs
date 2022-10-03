using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIAlturas.ViewModels
{
    public class GiftCard
    {
        public GiftCard()
        {
            GiftCardGuid = Guid.NewGuid();
            DataHora = DateTime.Now;
        }
        public Guid GiftCardGuid { get; set; }
        public float Value { get; set; }
        public DateTime DataHora { get; set; }
        public Guid UserId { get; set; }
        public int RestauranteId { get; set; }
    }
}
