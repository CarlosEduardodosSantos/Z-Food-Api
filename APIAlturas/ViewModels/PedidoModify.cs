using System;

namespace APIAlturas.ViewModels
{
    public class PedidoModify
    {
        public Guid PedidoGuid { get; set; }
        public int Situacao { get; set; }
        public string PlayersId { get; set; }
        public string Mensagem { get; set; }
        public bool EnviaNotification { get; set; }
    }
}