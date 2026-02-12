using API.Dtos;
using API.Entities;

namespace API.Services.Interfaces;

public interface ITokenService
{
    string CreateToken(UserDto user);
}
