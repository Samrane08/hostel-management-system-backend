namespace DDO_Service.API.Features.Token
{
    public class JwtIssuerOptions
    {
        public string IssuerSigningKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string ValidIssuer { get; set; } = string.Empty;
        public string ValidAudience { get; set; } = string.Empty;
        public string ValidFor { get; set; } = "0";
    }
}
