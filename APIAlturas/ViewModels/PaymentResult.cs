using System;

namespace APIAlturas.ViewModels
{
    public class PaymentResult
    {
        public Guid PaymentResultId { get; set; }
        public Guid UserId { get; set; }
        public string Status { get; set; }
        public string CodigoAutorizacao { get; set; }
        public DateTime DataHora { get; set; }
        public string ReferenciaId { get; set; }
        public string Menssage { get; set; }
        public string Nsu { get; set; }
        public bool Autorizado { get; set; }
        public bool Pendente { get; set; }
    }
}