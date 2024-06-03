using AutoMapper;
using Azure.Core;
using FinalHackBank.CORE;
using FinalHackBank.CORE.Dto;
using FinalHackBank.CORE.Interfaces;
using FinalHackBank.CORE.Models;
using FinalHackBank.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.Design;
using System.Reflection.Metadata;
using System.Reflection;
using System.Security.Cryptography;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FinalHackBank.API.Helpers;

namespace FinalHackBank.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;


        private readonly IMapper mapper;

        public AuthController(IUnitOfWork unitOfWork, IMapper mapper)
        {

            _unitOfWork = unitOfWork;

            this.mapper = mapper;

        }

        /*---------------------------------------------------------------------------------------------------------------------------------*/
        /*----------------------------------              Connecter à l'application                ----------------------------------------*/
        /*---------------------------------------------------------------------------------------------------------------------------------*/

        [HttpPost("Connect")]
        public async Task<IActionResult> Connect([FromBody] LoginDto user)
        {
            try
            {
                if (user != null)
                {

                    var res = await _unitOfWork.user.FindAsync(x => x.Email == user.Email);
                    if (res != null)
                    {
                        
                        bool passwordMatches = PasswordHasher.VerifyPassword(user.Password, res.Password);
                        if (!passwordMatches)

                        {

                            return NotFound(new { Message = "le mot de passe est incorrect" });
                        }

                        // il faut fixé id = 1 de la table StatusUser est 'désactivé' 

                        /*if (res.StatusUserId != 1)
                        {
                            return Ok(new { Message = "compte désactivé" });
                        }
                        */

                        IConfiguration configuration = new ConfigurationBuilder()
                                                .SetBasePath(Directory.GetCurrentDirectory())
                                                .AddJsonFile("appsettings.json")
                                                .Build();


                        string secretKey = configuration.GetSection("JWTSettings")["SecretKey"];
                        Console.WriteLine(secretKey);
                        int tokenExpirationMinutes = int.Parse(configuration.GetSection("JWTSettings")["TokenExpirationMinutes"]);

                        var resemp = await _unitOfWork.employee.FindAsync(x => x.UserId == res.UserId);
                        if(resemp != null)
                        {
                            var claims = new List<Claim>
                          {
                             new Claim(ClaimTypes.NameIdentifier, res.UserId.ToString()),
                             new Claim(ClaimTypes.Name, res.Namee),
                             new Claim(ClaimTypes.Role, resemp.RoleId.ToString()),
                             new Claim(ClaimTypes.UserData, res.StatusUserId.ToString())

                          };
                            var tokeOptions = new JwtSecurityToken(
                            issuer: "https://localhost:7015",
                            audience: "https://localhost:7015",
                            claims: claims,
                            expires: DateTime.Now.AddMinutes(tokenExpirationMinutes),
                            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)), SecurityAlgorithms.HmacSha256)
                        );
                            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                            return Ok(new AuthenticatedResponseDto { Token = tokenString });
                        }
                        else
                        {
                            var claims = new List<Claim>
                          {
                             new Claim(ClaimTypes.NameIdentifier, res.UserId.ToString()),
                             new Claim(ClaimTypes.Name, res.Namee),
                             new Claim(ClaimTypes.UserData, res.StatusUserId.ToString())

                          };
                            var tokeOptions = new JwtSecurityToken(
                            issuer: "https://localhost:7015",
                            audience: "https://localhost:7015",
                            claims: claims,
                            expires: DateTime.Now.AddMinutes(tokenExpirationMinutes),
                            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)), SecurityAlgorithms.HmacSha256)
                        );
                            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                            return Ok(new AuthenticatedResponseDto { Token = tokenString });
                        }
                        

                        



                        


                    }


                }

                return BadRequest(new { Message = "Le nom d'utilisateur ou le mot de passe est incorrect" });


            }

            

            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


    }
}
