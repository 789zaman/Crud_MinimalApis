using Microsoft.AspNetCore.Authentication;

namespace library.api.Properties.Auth
{
    public class apiKeyAuthSchemeOptions : AuthenticationSchemeOptions
    {
        public string ApiKey { get; set; } = "123456";
    }
}
