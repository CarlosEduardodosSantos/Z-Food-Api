using System;

namespace APIAlturas.ViewModels
{
    public class PollingModel
    {
        public PollingModel()
        {
            id = Guid.NewGuid();
        }
        public Guid id { get; set; }
        public Guid token { get; set; }
        public string code => statuss.ToString();
        public PedidoStatusEnumView statuss { get; set; }
        public string correlationId { get; set; }
        public DateTime createdAt { get; set; }
        public string historico { get; set; }
        public string orderId => correlationId;
    }
}