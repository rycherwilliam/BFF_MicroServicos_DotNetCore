namespace ClientesAPI.Application.Services.Autenticacao
{
    public interface IJwtAuthService
    {
        bool ValidateToken(string authToken);
    }
}
