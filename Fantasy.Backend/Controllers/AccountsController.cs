using Fantasy.Backend.Data;
using Fantasy.Backend.Helpers;
using Fantasy.Backend.UnitsOfWork.Interfaces;
using Fantasy.Shared.DTOs;
using Fantasy.Shared.Entities;
using Fantasy.Shared.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Fantasy.Backend.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IUserUnitOfWork _userUnitOfWork;
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;
        private readonly IFileStorage _fileStorage;
        private readonly IMailHelper _mailHelper;

        public AccountsController(IUserUnitOfWork userUnitOfWork, IConfiguration configuration,
            DataContext context, IFileStorage fileStorage, IMailHelper mailHelper)
        {
            _userUnitOfWork = userUnitOfWork;
            _configuration = configuration;
            _context = context;
            _fileStorage = fileStorage;
            _mailHelper = mailHelper;
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserDTO model)
        {
            var country = await _context.Countries.FindAsync(model.CountryId);
            if (country == null)
            {
                return BadRequest("ERR004");
            }

            User user = model;

            if (!string.IsNullOrEmpty(model.Photo))
            {
                var photoUser = Convert.FromBase64String(model.Photo);
                user.Photo = await _fileStorage.SaveFileAsync(photoUser, ".jpg", "users");
            }

            user.Country = country;
            var result = await _userUnitOfWork.AddUserAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userUnitOfWork.AddUserToRoleAsync(user, user.UserType.ToString());
                var response = await SendConfirmationEmailAsync(user, model.Language);
                if (response.WasSuccess)
                {
                    return NoContent();
                }

                return BadRequest(response.Message);
            }

            return BadRequest(result.Errors.FirstOrDefault());
        }

        [HttpPost("RecoverPassword")]
        public async Task<IActionResult> RecoverPasswordAsync([FromBody] EmailDTO model)
        {
            var user = await _userUnitOfWork.GetUserAsync(model.Email);
            if (user == null)
            {
                return NotFound();
            }

            var response = await SendRecoverEmailAsync(user, model.Language);
            if (response.WasSuccess)
            {
                return NoContent();
            }

            return BadRequest(response.Message);
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordDTO model)
        {
            var user = await _userUnitOfWork.GetUserAsync(model.Email);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userUnitOfWork.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (result.Succeeded)
            {
                return NoContent();
            }

            return BadRequest(result.Errors.FirstOrDefault()!.Description);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDTO model)
        {
            var result = await _userUnitOfWork.LoginAsync(model);
            if (result.Succeeded)
            {
                var user = await _userUnitOfWork.GetUserAsync(model.Email);
                return Ok(BuildToken(user));
            }

            if (result.IsLockedOut)
            {
                return BadRequest("ERR007");
            }

            if (result.IsNotAllowed)
            {
                return BadRequest("ERR008");
            }

            return BadRequest("ERR006");
        }

        private async Task<ActionResponse<string>> SendRecoverEmailAsync(User user, string language)
        {
            var myToken = await _userUnitOfWork.GeneratePasswordResetTokenAsync(user);
            var tokenLink = Url.Action("ResetPassword", "accounts", new
            {
                userid = user.Id,
                token = myToken
            }, HttpContext.Request.Scheme, _configuration["Url Frontend"]);

            if (language == "es")
            {
                return _mailHelper.SendMail(user.FullName, user.Email!, _configuration["Mail:SubjectRecoveryEs"]!, string.Format(_configuration["Mail:BodyRecoveryEs"]!, tokenLink), language);
            }
            return _mailHelper.SendMail(user.FullName, user.Email!, _configuration["Mail:SubjectRecoveryEn"]!, string.Format(_configuration["Mail:BodyRecoveryEn"]!, tokenLink), language);
        }

        private async Task<ActionResponse<string>> SendConfirmationEmailAsync(User user, string language)
        {
            var myToken = await _userUnitOfWork.GenerateEmailConfirmationTokenAsync(user);
            var tokenLink = Url.Action("ConfirmEmail", "accounts", new
            {
                userid = user.Id,
                token = myToken
            }, HttpContext.Request.Scheme, _configuration["Url Frontend"]);

            if (language == "es")
            {
                return _mailHelper.SendMail(user.FullName, user.Email!, _configuration["Mail:SubjectConfirmationEs"]!, string.Format(_configuration["Mail:BodyConfirmationEs"]!, tokenLink), language);
            }

            return _mailHelper.SendMail(user.FullName, user.Email!, _configuration["Mail:SubjectConfirmationEn"]!, string.Format(_configuration["Mail:BodyConfirmationEn"]!, tokenLink), language);
        }

        private TokenDTO BuildToken(User user)
        {
            var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Email!),
            new(ClaimTypes.Role, user.UserType.ToString()),
            new("FirstName", user.FirstName),
            new("LastName", user.LastName),
            new("Photo", user.Photo ?? string.Empty),
            new("CountryId", user.Country.CountryId.ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwtKey"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddDays(30);
            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: credentials);

            return new TokenDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
    }
}