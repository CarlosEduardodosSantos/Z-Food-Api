using System;

namespace APIAlturas.ViewModels
{
    public class AuditoriaConsumo
    {
        public int Id { get; set; }
        public string Historico { get; set; }
        public string Login { get; set; }
        public DateTime Dia { get; set; }
        public DateTime Data { get; set; }
        public decimal ? Valor { get; set; }
        public int RestauranteId { get; set; }
    }
}
