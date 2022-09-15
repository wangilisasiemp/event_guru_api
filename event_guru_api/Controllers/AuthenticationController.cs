using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using event_guru_api.auth;
using event_guru_api.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using IronBarCode;
using System.Web;

namespace event_guru_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticationController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    var userRoles = await _userManager.GetRolesAsync(user);
                    var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,user.UserName),
                    new Claim(ClaimTypes.NameIdentifier,user.UserName),
                    new Claim(ClaimTypes.HomePhone,user.PhoneNumber),
                    new Claim(ClaimTypes.GivenName,user.FirstName),
                    new Claim(ClaimTypes.Surname,user.LastName),
                    new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                };

                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    var token = GetToken(authClaims);
                    return Ok(new
                    {
                        token = $"Bearer {new JwtSecurityTokenHandler().WriteToken(token)}",
                        expiration = token.ValidTo
                    });
                }
                return Unauthorized();
            }
            catch (Exception err)
            {
                return Problem(err.Message);
            }

        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            return token;
        }

        /***/
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            try
            {
                //check if the form is dully filled
                if (!ModelState.IsValid)
                {
                    return ValidationProblem();
                }

                //check if the passwords entered actually match
                if (!model.Password!.Equals(model.ConfirmPassword))
                {
                    ModelState.AddModelError("PasswordsDontMatch", "Your passwords do not match");
                    return ValidationProblem(ModelState);
                }

                //check to see if the user exists
                var userExists = await _userManager.FindByNameAsync(model.PhoneNumber);
                if (userExists is not null)
                {
                    ModelState.AddModelError("UserExists", "The user with those details already exists");
                    return ValidationProblem(ModelState);
                }

                var newUser = new ApplicationUser()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    Gender = model.Gender,
                    Address = model.Address,
                    UserName = model.PhoneNumber,
                    SecurityStamp = Guid.NewGuid().ToString(),
                };

                var result = await _userManager.CreateAsync(newUser, model.Password);
                if (!result.Succeeded)
                {

                    return Problem($" Something went wrong {result.Errors.ToString()}");
                }

                //Add roles to the user using role manager
                if (!await _roleManager.RoleExistsAsync(UserRoles.Attendee))
                {
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.Attendee));
                }

                if (await _roleManager.RoleExistsAsync(UserRoles.Attendee))
                {
                    await _userManager.AddToRoleAsync(newUser, UserRoles.Attendee);
                }
                return Ok(new Response { Status = "Success", Message = "User added successfully!" });
            }
            catch (Exception err)
            {
                return Problem(err.Message);
            }


        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            try
            {
                //check if the form is dully filled
                if (!ModelState.IsValid)
                {
                    return ValidationProblem();
                }

                //check if the passwords entered actually match
                if (!model.Password!.Equals(model.ConfirmPassword))
                {
                    ModelState.AddModelError("PasswordsDontMatch", "Your passwords do not match");
                    return ValidationProblem(ModelState);
                }

                //check to see if the user exists
                var userExists = await _userManager.FindByNameAsync(model.PhoneNumber);
                if (userExists is not null)
                {
                    ModelState.AddModelError("UserExists", "The user with those details already exists");
                    return ValidationProblem(ModelState);
                }

                var newUser = new ApplicationUser()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    Gender = model.Gender,
                    Address = model.Address,
                    UserName = model.PhoneNumber,
                    SecurityStamp = Guid.NewGuid().ToString(),
                };

                var result = await _userManager.CreateAsync(newUser, model.Password);
                if (!result.Succeeded)
                {

                    return Problem($" Something went wrong {result.Errors.ToString()}");
                }

                //Add roles to the user using role manager
                if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                {
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                }

                if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
                {
                    await _userManager.AddToRoleAsync(newUser, UserRoles.Admin);
                }

                return Ok(new Response { Status = "Success", Message = "User admin added successfully!" });
            }
            catch (Exception err)
            {
                return Problem(err.Message);
            }

        }

        [HttpPost]
        [Route("register-organizer")]
        public async Task<IActionResult> RegisterOrganizer([FromBody] RegisterModel model)
        {
            try
            {
                //check if the form is dully filled
                if (!ModelState.IsValid)
                {
                    return ValidationProblem();
                }

                //check if the passwords entered actually match
                if (!model.Password!.Equals(model.ConfirmPassword))
                {
                    ModelState.AddModelError("PasswordsDontMatch", "Your passwords do not match");
                    return ValidationProblem(ModelState);
                }

                //check to see if the user exists
                var userExists = await _userManager.FindByNameAsync(model.PhoneNumber);
                if (userExists is not null)
                {
                    ModelState.AddModelError("UserExists", "The user with those details already exists");
                    return ValidationProblem(ModelState);
                }

                var newUser = new ApplicationUser()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    Gender = model.Gender,
                    Address = model.Address,
                    UserName = model.PhoneNumber,
                    SecurityStamp = Guid.NewGuid().ToString(),
                };

                var result = await _userManager.CreateAsync(newUser, model.Password);
                if (!result.Succeeded)
                {

                    return Problem($" Something went wrong {result.Errors.ToString()}");
                }

                //Add roles to the user using role manager
                if (!await _roleManager.RoleExistsAsync(UserRoles.Organizer))
                {
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.Organizer));
                }

                if (await _roleManager.RoleExistsAsync(UserRoles.Organizer))
                {
                    await _userManager.AddToRoleAsync(newUser, UserRoles.Organizer);
                }

                return Ok(new Response { Status = "Success", Message = "User added successfully!" });
            }
            catch (Exception err)
            {
                return Problem(err.Message);
            }

        }

        [HttpGet]
        [Route("/qrcode")]
        public async Task<IActionResult> GenerateQrCode(string email)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(email);
                if (user is null)
                {
                    return Problem(title: "Something went wrong");
                }
                GeneratedBarcode barcode = QRCodeWriter.CreateQrCode(user.Id, 300);
                barcode.AddBarcodeValueTextBelowBarcode();
                barcode.SetMargins(10);
                barcode.ChangeBarCodeColor(Color.BlueViolet);
                string folderPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "Uploads"));
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                string filePath = Path.Combine(folderPath, "NewQrCode.png");
                barcode.SaveAsPng(filePath);
                return Ok();
            }
            catch (Exception err)
            {
                return Problem(err.Message);
            }

        }

    }
}
