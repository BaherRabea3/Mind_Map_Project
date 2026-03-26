using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using MindMapManager.Core.DTOs;
using MindMapManager.Core.Entities;
using MindMapManager.Core.ServiceContracts;
using System.Security.Claims;

namespace MindMapManager.WebAPI.Controllers
{
    [AllowAnonymous]
    //[ApiVersion("1.0")]
    public class AccountController : CustomControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtService _jwtService;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IJwtService jwtService,IConfiguration configuration,IEmailService emailService,RoleManager<ApplicationRole> roleManager)
        {
           _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _config = configuration;
            _emailService = emailService;
            _roleManager = roleManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                string ErrorMassege = 
                    string.Join(" | ",ModelState.Values
                    .SelectMany(v=> v.Errors)
                    .Select(error => error.ErrorMessage));
            }
            ApplicationUser appuser = new ApplicationUser();
            appuser.Email = registerDTO.Email;
            appuser.UserName = registerDTO.Name;

            IdentityResult result = await _userManager.CreateAsync(appuser,registerDTO.Password);

            if (!result.Succeeded)
            {
                string errorMessage = string.Join(" | ", result.Errors.Select(error => error.Description));
                return Problem(errorMessage);
            }
             
            await _signInManager.SignInAsync(appuser, isPersistent: false);

            // create token
            var authRespnse = await _jwtService.CreateJwtTokenAsync(appuser);

            appuser.RefreshToken = authRespnse.RefreshToken;
            appuser.RefreshTokenExpiration = authRespnse.RefreshTokenExpiration;

            await _userManager.UpdateAsync(appuser);
            
            return Ok(authRespnse);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                string errorMassege = 
                    string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
            }

            ApplicationUser? appUser = await _userManager.FindByEmailAsync(loginDTO.Email);

            if (User != null)
            {
                bool IsValidPassword = await _userManager.CheckPasswordAsync(appUser, loginDTO.Password);
                if (IsValidPassword)
                {
                    await _signInManager.SignInAsync(appUser, isPersistent: loginDTO.RememberMe);

                    var authResponse = await _jwtService.CreateJwtTokenAsync(appUser);

                    appUser.RefreshToken = authResponse.RefreshToken;
                    appUser.RefreshTokenExpiration = authResponse.RefreshTokenExpiration;

                    await _userManager.UpdateAsync(appUser);

                    return Ok(authResponse);
                }
            }
            return Problem("Invalid Email or Password");
           
        }

        [HttpGet("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return NoContent();
        }

        [HttpPost("generate-new-jwt-token")]
        public async Task<IActionResult> GenerateNewJwtToken(TokenDTO tokenDTO)
        {
            if (tokenDTO == null)
                return BadRequest("invalid client token");

            var principal = _jwtService.GetPrincipaleFromJwtToken(tokenDTO.Token);

            if (principal == null)
            {
                return BadRequest("Invalid jwt access token");
            }

            string? email = principal.FindFirstValue(ClaimTypes.Email);

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null || 
                tokenDTO.RefreshToken != user.RefreshToken || 
                user.RefreshTokenExpiration < DateTime.UtcNow)
            {
                return BadRequest("Invalid refresh token");
            }

            var authResponse = await _jwtService.CreateJwtTokenAsync(user);

            user.RefreshToken = authResponse.RefreshToken;
            user.RefreshTokenExpiration = authResponse.RefreshTokenExpiration;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                string errorMsg = string.Join(" | ", result.Errors.Select(error => error.Description));
                return Problem(errorMsg);
            }

            return Ok(authResponse);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO forgotPasswordDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userManager.FindByEmailAsync(forgotPasswordDTO.Email);

            if (user == null)
            {
                return BadRequest("Invalid payload");
            }
            string ResetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            
            if (string.IsNullOrEmpty(ResetPasswordToken))
            {
                return BadRequest("Something went wrong");
            }
            var param = new Dictionary<string, string?>()
            {
                {"Token",ResetPasswordToken },
                {"Email", user.Email }
            }; 
            var callbackURL = QueryHelpers.AddQueryString(forgotPasswordDTO.ClientUri,param);

            // send email
            List<EmailAddress> emailAddresses = new List<EmailAddress>();
            emailAddresses.Add(new EmailAddress { DisplayName = user.UserName,Email = user.Email });
            string subject = "Reset your account password";
            string body = $"click here to reset password\n{callbackURL}";
            Message message = new Message(emailAddresses, subject, body);

            await _emailService.SendPaswordResetEmailAsync(message);

            return Ok(new
            {
                token = ResetPasswordToken,
                email = user.Email
            });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequestDto request)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                return BadRequest("Invalid payload");

            var result = await _userManager.ResetPasswordAsync(user, request.ResetToken, request.Password);

            if (!result.Succeeded)
            {
                var msg = string.Join(" | ", result.Errors.Select(error => error.Description));
                return BadRequest(msg);
            }

            return Ok();

        }

        [HttpGet("is-email-already-registered")]
        public async Task<IActionResult> IsEmailIsEmailAlreadyRegistered(string email)
        {
            var RegisterEmail = await _userManager.FindByEmailAsync(email);

            if (RegisterEmail == null)
                return Ok(true);

            return Ok(false);
        }
    }
}
