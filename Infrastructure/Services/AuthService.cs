namespace cybersoft_final_project.Infrastructure.Services;

using cybersoft_final_project.Context;
using cybersoft_final_project.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using web_api_base.Helper; // Đảm bảo namespace chứa PasswordHelper đúng với dự án bạn

public class AuthService
{
    private readonly MyDbContext _context;
    private readonly IConfiguration _config;

    public AuthService(MyDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async Task<(bool Success, string Message, string? Token)> LoginAsync(string username, string password)
    {
        // Tìm người dùng theo username
        var user = _context.users.FirstOrDefault(u => u.username == username || u.email == username);
        if (user == null || !PasswordHelper.VerifyPassword(password, user.password))
        {
            return (false, "Sai tên đăng nhập hoặc mật khẩu", null);
        }

        // Tạo JWT
        var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("UserId", user.userid.ToString()),
                new Claim("Full name", user.fullname),
                new Claim("Role", user.role)
            }),
            Expires = DateTime.UtcNow.AddMonths(5),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _config["Jwt:Issuer"],
            Audience = _config["Jwt:Audience"]
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);

        return (true, "Đăng nhập thành công", jwt);
    }
    
    
    public async Task<(bool Success, string Message)> RegisterAsync(string username, string password, string email)
    {
        if (_context.users.Any(u => u.username == username))
            return (false, "Tên đăng nhập đã tồn tại");

        if (_context.users.Any(u => u.email == email))
            return (false, "Email đã được sử dụng");

        var hashedPassword = PasswordHelper.HashPassword(password);

        var newUser = new user
        {
            username = username,
            password = hashedPassword,
            email = email,
            status = true,
            role = "customer", // mặc định role người dùng thường
            fullname = "",
            address = "",
            phone = "",
            birthday = null
        };

        _context.users.Add(newUser);
        await _context.SaveChangesAsync();

        return (true, "Đăng ký thành công");
    }

}