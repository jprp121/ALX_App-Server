using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.Dtos;
using API.Entities;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;
public class AccountController(DataContext context, ITokenService tokenService) : BaseController
{
    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(UserRegisterInfo userInfo)
    {
        if(await UserExists(userInfo))
            return BadRequest("Username is taken");    

        using var hmac = new HMACSHA512();

        var user = new User(){
            UserName = userInfo.Username,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userInfo.Password)),
            PasswordSalt = hmac.Key
        };

        context.Add(user);

        await context.SaveChangesAsync();

        return user;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(UserLoginInfo user)
    {
        var loginuser = await context.Users.FirstOrDefaultAsync(x => x.UserName.ToLower() == user.Username.ToLower());

        if(loginuser == null) return Unauthorized("invalid username");

        using var hmac = new HMACSHA512(loginuser.PasswordSalt);

        var ComputeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(user.Password));

        for (int i = 0; i < ComputeHash.Length; i++)
        {
            if(ComputeHash[i] != loginuser.PasswordHash[i]) return Unauthorized("wrong password");
        }

        var dto = new UserDto(){
            UserName = loginuser.UserName,
            Name = loginuser.Name
        };

        dto.Token = tokenService.CreateToken(dto);
        return dto;
    }

    private async Task<bool> UserExists(UserRegisterInfo userInfo){
        return await context.Users.AnyAsync(x => x.UserName.ToLower() == userInfo.Username.ToLower());
    }
}
