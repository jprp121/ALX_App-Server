namespace API.Dtos;

public class UserDto
{
    public int Id { get; set; }
    public required string UserName {get; set;}
    public string? Name { get; set; }
    public string? Token {get;set;}
}

public class UserRegisterInfo
{
    public required string Username { get; set; }
    public required string Password {get;set;}
}   

public class UserLoginInfo
{
    public required string Username { get; set; }
    public required string Password {get;set;}
}