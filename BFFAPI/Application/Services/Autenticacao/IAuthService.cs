using BFFAPI.Application.Services.Autenticacao;

public interface IAuthService
{
    AuthResult Authenticate(string username, string password);
}