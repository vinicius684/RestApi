namespace DevIO.Api.Extensions
{
    public class AppSettings
    {
        public string Secret { get; set; } //chave
        public int ExpiracaoHoras { get; set; }
        public string Emissor { get; set; }
        public string ValidoEm { get; set; }
    }
}