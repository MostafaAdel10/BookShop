using Microsoft.Extensions.Configuration;
using PayPalCheckoutSdk.Core;

namespace Infrastructure.PayPal
{
    public class PayPalClient
    {
        private readonly IConfiguration _configuration;
        public PayPalClient(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public PayPalEnvironment GetEnvironment()
        {
            var clientId = _configuration["PayPal:ClientId"];
            var clientSecret = _configuration["PayPal:ClientSecret"];
            var mode = _configuration["PayPal:Mode"];
            return mode.ToLower() == "live"
                ? new LiveEnvironment(clientId, clientSecret)
                : new SandboxEnvironment(clientId, clientSecret);
        }
        public PayPalHttp.HttpClient GetClient()
        {
            return new PayPalHttpClient(GetEnvironment());
        }
    }
}
