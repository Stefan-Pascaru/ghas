using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace ProductsApi.Auth;

public class BasicAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IConfiguration _configuration;

    public BasicAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IConfiguration configuration)
        : base(options, logger, encoder)
    {
        _configuration = configuration;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("Authorization", out var authHeaderValue))
            return Task.FromResult(AuthenticateResult.Fail("Missing Authorization header"));

        if (!AuthenticationHeaderValue.TryParse(authHeaderValue, out var header) ||
            !string.Equals(header.Scheme, "Basic", StringComparison.OrdinalIgnoreCase) ||
            header.Parameter is null)
            return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization header"));

        string credentials;
        try
        {
            credentials = Encoding.UTF8.GetString(Convert.FromBase64String(header.Parameter));
        }
        catch
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid Base64 encoding"));
        }

        var separatorIndex = credentials.IndexOf(':');
        if (separatorIndex < 0)
            return Task.FromResult(AuthenticateResult.Fail("Invalid credentials format"));

        var username = credentials[..separatorIndex];
        var password = credentials[(separatorIndex + 1)..];

        var expectedUsername = _configuration["BasicAuth:Username"];
        var expectedPassword = _configuration["BasicAuth:Password"];

        if (username != expectedUsername || password != expectedPassword)
            return Task.FromResult(AuthenticateResult.Fail("Invalid username or password"));

        var claims = new[] { new Claim(ClaimTypes.Name, username) };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }

    protected override Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Response.Headers["WWW-Authenticate"] = "Basic realm=\"ProductsApi\"";
        return base.HandleChallengeAsync(properties);
    }
}
