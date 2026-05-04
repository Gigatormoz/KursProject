using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.Security;
using ProdjectApi.Data;
using ProdjectApi.Domain.Models;
using ProdjectApi.Domain.Requests.Users;
using ProdjectApi.Service;
using ProdjectApi.Service.Contracts;

namespace ProdjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersControllers : ControllerBase
    {
        private readonly ProjectDbContext _context;
        private readonly IPasswordService _passwordService;
        private readonly SmtpEmailService _emailService;
        public UsersControllers(ProjectDbContext context, IPasswordService passwordService, SmtpEmailService emailService)
        {
            _context = context;
            _passwordService = passwordService;
            _emailService = emailService;
        }
        //поиск всех пользователей
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.Users.ToListAsync());
        }

        //создание пользователя
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserRequest request)
        {
            var existingUser = await _context.Users
                .Where(u => u.Email == request.Email).SingleOrDefaultAsync();

            if (existingUser != null)
                return Conflict(new { Message = "Пользователь с таким адресом электронной почты уже существует" });

            var passwordHash = _passwordService.GeneratePasswordHash(request.Password);

            var user = new Users
            {
                Name = request.Name,
                Surname = request.Surname,
                Patronymic = request.Patronymic,
                Email = request.Email,
                IsEmailVerified = false,
                Nickname = request.Nickname,
                PasswordHash = passwordHash,
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = Guid.NewGuid().ToString();
            var code = Random.Shared.Next(100000,999999).ToString();
            var verification = new EmailVerification
            {
                UserId = user.Id,
                Code = code,
                ExpiresAt = DateTime.UtcNow.AddMinutes(3),
                IsUsed = false
            };

            _context.EmailVerifications.Add(verification);
            await _context.SaveChangesAsync();

            Console.WriteLine("Письмо отпровляется");
            await _emailService.SendVerificationEmail(
                email: request.Email,
                code: verification.Code
            );

            return Ok(new { Message = "Подтвердите почту" });

        }

        //Подтверждение почты
        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmail(string code)
        {
            if (string.IsNullOrEmpty(code))
                return BadRequest("Неверный код: код не указан");

            var verification = await _context.EmailVerifications
                .Where(v => v.Code == code
                            && v.ExpiresAt > DateTime.UtcNow
                            && !v.IsUsed)
                .SingleOrDefaultAsync();

            if (verification == null)
                return BadRequest("Неверный или просроченный код");

            var user = await _context.Users
                .Where(u => u.Id == verification.UserId)
                .SingleOrDefaultAsync();

            if (user == null)
                return NotFound("Пользователь не найден");

            verification.IsUsed = true;
            user.IsEmailVerified = true;
            await _context.SaveChangesAsync();

            return Ok("Электронная почта успешно подтверждена");
        }

        //Поиск пользователя по id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = _context.Users.FindAsync(id);

            if(user == null)
                return NotFound();

            return Ok(user);
        }

        //Удаление пользователя по id
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if(user == null)
                return NotFound();

            _context.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //Смена пароля
        [HttpPut]
        public async Task<IActionResult> ChangePassword(int id, ChangePasswordRequest request)
        {
            var user = await _context.Users
                .Where(u => u.Id == id).SingleOrDefaultAsync();

            if (user == null)
                return NotFound();

            if (!_passwordService.VerifyPassword(request.OldPassword, user.PasswordHash   ))
                return BadRequest(new { Message = "Неправильный пароль" });

            user.PasswordHash = _passwordService.GeneratePasswordHash(request.NewPassword);

            await _context.SaveChangesAsync();
            return Ok(new { Message = "Пароль успешно изменён" });
        }
    }
}
