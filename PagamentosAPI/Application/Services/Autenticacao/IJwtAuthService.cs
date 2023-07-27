namespace PagamentosAPI.Application.Services.Autenticacao
{
    public interface IJwtAuthService
    {
        bool ValidateToken(string authToken);
    }
}
