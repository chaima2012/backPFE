using AutoMapper;
using Azure.Core;
using FinalHackBank.API.Helpers;
using FinalHackBank.CORE;
using FinalHackBank.CORE.Dto;
using FinalHackBank.CORE.Interfaces;
using FinalHackBank.CORE.Models;
using FinalHackBank.EF;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.Design;
using System.Net;
using System.Security.Cryptography;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FinalHackBank.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {


        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LoginController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        // se connecter

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {
            try
            {



                var res = await _unitOfWork.user.FindAsync(x => x.Email == loginDto.Email);
                if (res != null)
                {
                    bool passwordMatches = PasswordHasher.VerifyPassword(loginDto.Password, res.Password);
                    if (!passwordMatches)

                    {
                        return Ok(new { Message = "Le nom d'utilisateur ou le mot de passe est incorrect" });
                    }

                    // il faut fixé id = 1 de la table StatusUser est 'désactivé' 

                    /*if (res.StatusUserId != 1)
                    {
                        return Ok(new { Message = "compte désactivé" });
                    }
                    */

                    return Ok(new { Message = "logged in!" });
                }

                return Ok(new { Message = "Le nom d'utilisateur ou le mot de passe est incorrect" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}
