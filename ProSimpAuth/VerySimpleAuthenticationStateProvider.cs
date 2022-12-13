using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace ProSimpAuth
{
    public class VerySimpleAuthenticationStateProvider : AuthenticationStateProvider
    {
        // default is an empty ClaimsPrincipal which won't authenticate
        private ClaimsPrincipal _user = new ClaimsPrincipal();

        // MySecrets injected by DI
        private IOptions<MySecrets> _mySecrets;

        public VerySimpleAuthenticationStateProvider(IOptions<MySecrets> secrets)
            => _mySecrets = secrets;

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
            => Task.FromResult(new AuthenticationState(_user));

        public Task<AuthenticationState> ChangeIdentityAsync(string? secret)
        {
            var role = GetRole(secret);

            // creates a claims array with the provided information
            Claim[] Claims = new[]{            
            new Claim(ClaimTypes.Role, role ?? string.Empty)
        };

            // If we don't have role then create an empty ClaimsPrincipal which won't authenticate
            // If we do create a populated ClaimsPrincipal
            // Note you only actually need to provide an AuthenticationType for the user to be authenticated
            if (role == null)
                _user = new ClaimsPrincipal();
            else
                _user = new ClaimsPrincipal(new ClaimsIdentity(Claims, "SecretAuthType"));

            // Get a new AuthenticationState and Notify any listeners that the state has changed
            var task = this.GetAuthenticationStateAsync();
            this.NotifyAuthenticationStateChanged(task);
            return task;
        }

        //public Task<AuthenticationState> ChangeIdentityAsync(string? username, string? secret)
        //{
        //    var role = GetRole(secret);

        //    // creates a claims array with the provided information
        //    Claim[] Claims = new[]{
        //    new Claim(ClaimTypes.Name, username ?? "Anonymous"),
        //    new Claim(ClaimTypes.Role, role ?? string.Empty)
        //};

        //    // If we don't have role then create an empty ClaimsPrincipal which won't authenticate
        //    // If we do create a populated ClaimsPrincipal
        //    // Note you only actually need to provide an AuthenticationType for the user to be authenticated
        //    if (role == null)
        //        _user = new ClaimsPrincipal();
        //    else
        //        _user = new ClaimsPrincipal(new ClaimsIdentity(Claims, "SecretAuthType"));

        //    // Get a new AuthenticationState and Notify any listeners that the state has changed
        //    var task = this.GetAuthenticationStateAsync();
        //    this.NotifyAuthenticationStateChanged(task);
        //    return task;
        //}

        private string? GetRole(string? secret)
        {
            secret = secret ?? string.Empty;

            if (secret.Equals(_mySecrets.Value.Admin))
                return "AdminRole";

            if (secret.Equals(_mySecrets.Value.User))
                return "UserRole";

            if (secret.Equals(_mySecrets.Value.Visitor))
                return "VisitorRole";

            return null;
        }
    }
}