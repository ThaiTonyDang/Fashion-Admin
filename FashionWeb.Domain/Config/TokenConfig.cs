namespace FashionWeb.Domain.Config
{
    public class TokenConfig
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Secret { get; set; }
        public int Expired { get; set; }
    }
}
