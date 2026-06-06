using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MarketPlaceCore.Core.Entities;
using MarketPlaceCore.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace MarketPlaceCore.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    private readonly IPasswordHasher _passwordHasher;

    public AccountController(IUnitOfWork unitOfWork, IConfiguration configuration, IPasswordHasher passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
        _passwordHasher = passwordHasher;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = (await _unitOfWork.Users.Find(u => u.Username == model.Username)).FirstOrDefault();

        if (user == null || !_passwordHasher.Verify(model.Password, user.PasswordHash))
        {
            return Unauthorized("نام کاربری یا رمز عبور اشتباه است");
        }

        return GenerateJwtToken(user);
    }

    [HttpPost("otp-request")]
    public async Task<IActionResult> RequestOtp([FromBody] string phoneNumber)
    {
        var user = (await _unitOfWork.Users.Find(u => u.PhoneNumber == phoneNumber)).FirstOrDefault();
        if (user == null) return NotFound("کاربری با این شماره یافت نشد");

        user.OtpCode = "12345"; // Simulated OTP
        user.OtpExpiry = DateTime.UtcNow.AddMinutes(5);
        _unitOfWork.Users.Update(user);
        await _unitOfWork.CompleteAsync();

        return Ok("کد تایید ارسال شد (شبیه‌سازی: 12345)");
    }

    [HttpPost("otp-login")]
    public async Task<IActionResult> OtpLogin([FromBody] OtpLoginModel model)
    {
        var user = (await _unitOfWork.Users.Find(u => u.PhoneNumber == model.PhoneNumber)).FirstOrDefault();
        if (user == null || user.OtpCode != model.Code || user.OtpExpiry < DateTime.UtcNow)
        {
            return Unauthorized("کد تایید نامعتبر یا منقضی شده است");
        }

        user.OtpCode = null;
        _unitOfWork.Users.Update(user);
        await _unitOfWork.CompleteAsync();

        return GenerateJwtToken(user);
    }

    private IActionResult GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var secretKey = _configuration["Jwt:Key"];
        if (string.IsNullOrEmpty(secretKey))
        {
            throw new Exception("تنظیمات کلید JWT یافت نشد");
        }
        var key = Encoding.ASCII.GetBytes(secretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("UserId", user.Id.ToString())
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return Ok(new { Token = tokenHandler.WriteToken(token), Role = user.Role });
    }
}

public class LoginModel
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class OtpLoginModel
{
    public string PhoneNumber { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}
