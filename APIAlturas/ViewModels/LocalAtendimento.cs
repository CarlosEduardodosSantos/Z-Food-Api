namespace APIAlturas.ViewModels
{
    public class LocalAtendimento
    {
        public int atendimentoLocalId { get; set; }
        public string descricao { get; set; }
        public int restauranteId { get; set; }
        public string faixaInicial { get; set; }
        public string faixaFinal { get; set; }
        public decimal valorEntrega { get; set; }
    }
}