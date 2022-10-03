using System;

namespace APIAlturas.ViewModels
{
    public class Restaurante
    {
        public int RestauranteId { get; set; }
        public Guid Token { get; set; }
        public RestauranteSituacaoEnum Situacao { get; set; }
        public string Nome { get; set; }
        public string Cnpj { get; set; }
        public string Ie { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }
        public string Fone { get; set; }
        public string Imagem { get; set; }
        public string ImagemBase64 { get; set; }
        public DateTime AbreAs { get; set; }
        public DateTime FechaAs { get; set; }
        public double AvaliacaoRating { get; set; }
        public string Grupo { get; set; }
        public string Status => StatusAbertura();
        public decimal ValorEntrega { get; set; }
        public bool AtendeLocal { get; set; }
        public bool EstaAtendendo => StatusEstaAtendendo();
        public bool IsOperation { get; set; }
        public bool Disponivel => StatusDisponivel();
        public string TempoEntrega { get; set; }
        public Guid SetorId { get; set; }
        public string Descricao { get; set; }
        public DateTime DataHoraPooling { get; set; }
        public string OneSignalAppId { get; set; }
        public string OnSignalAppKey { get; set; }
        public string OneSignalGoogleId { get; set; }
        public string Zimmer { get; set; }
        public bool AceitaRetira { get; set; }
        public int PgtoOnlineZiP { get; set; }
        public decimal PedidoMinimo { get; set; }
        public string Versao { get; set; }
        public string LinkGoogle { get; set; }
        public string LinkApple { get; set; }
        public string SenhaMaster { get; set; }
        public string Site { get; set; }
        public string FoneCelular { get; set; }
        public string Email { get; set; }
        public bool IsGrupoPage { get; set; }
        public int CardStyle { get; set; }
        public decimal ValorEst { get; set; }

        private string StatusAbertura()
        {
            if (!AtendeLocal)
                return "AREA SEM COBERTURA";
            if (Situacao != RestauranteSituacaoEnum.Aberto)
                return "FECHADO";
                if (DateTime.Now.Hour >= AbreAs.Hour && DateTime.Now.Hour <= FechaAs.Hour)
                return "SEJA BEM VINDO.";
            else
                return $"FECHADO (abre as  {AbreAs.Hour.ToString()} Hs)";
        }
        private bool StatusEstaAtendendo()
        {
            return DateTime.Now.Hour >= AbreAs.Hour && DateTime.Now.Hour <= FechaAs.Hour;
        }
        private bool StatusDisponivel()
        {
            if (!EstaAtendendo)
                return false;
            if (!AtendeLocal)
                return false;
            if (Situacao != RestauranteSituacaoEnum.Aberto)
                return false;
            else if (!(DateTime.Now.Hour >= AbreAs.Hour && DateTime.Now.Hour <= FechaAs.Hour))
                return false;

            return true;
        }
    }
}