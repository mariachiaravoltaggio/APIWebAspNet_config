
using APIWebAspNet_config.Models.DTOs;

namespace APIWebAspNet_config.Repositories
{
    public interface IAuthRepository
    {
        bool IsUniqueUser(string username);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<UserDTO> Register(RegisterationRequestDTO requestDTO);
    }
}
