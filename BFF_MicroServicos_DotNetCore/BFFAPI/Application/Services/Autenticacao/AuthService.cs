using BFFAPI.Application.Services.Autenticacao;
using System.Security.Cryptography;

namespace BFFAPI.Services.Autenticacao
{
    public class AuthService : IAuthService
    {
        private readonly string _jwtSecretKey;

        public AuthService(string jwtSecretKey)
        {
            _jwtSecretKey = jwtSecretKey;
        }

        public AuthResult Authenticate(string username, string password)
        {           
                        
            // Autenticação Temporária(Criar microserviço de cadastro de usuário)
            if (username == "0" && password == "123456")
            {
                var userId = "0";
                var userName = "will";
                var authService = new JwtAuthService(_jwtSecretKey);
                var token = authService.GenerateToken(userId, userName);
                
                return new AuthResult { Success = true, Token = token };
            }
                        
            return new AuthResult { Success = false, ErrorMessage = "Credenciais inválidas." };
        }       
    }
}
