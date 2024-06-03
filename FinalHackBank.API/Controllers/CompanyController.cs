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
using System.Net;
using System.Security.Cryptography;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FinalHackBank.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {


        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CompanyController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        // chercher la liste de tous les demande
        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {

                var companies = (from a in await _unitOfWork.company.GetAllAsync()
                           join b in await _unitOfWork.user.GetAllAsync() on a.UserId equals b.UserId
                           join c in await _unitOfWork.statususer.GetAllAsync() on b.StatusUserId equals c.Id



                           select new
                           {
                               Id = a.UserId,
                               Email = b.Email,
                               Status = c.Name,
                               Name = b.Namee,
                               Adresse = a.Address,
                               PhoneNumber = a.PhoneNumber

                           });

                return Ok(companies); 

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }


        // chercher un demande selon son id

        [HttpPost("GetById")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {

                var companies = (from a in await _unitOfWork.company.GetAllAsync()
                                 join b in await _unitOfWork.user.GetAllAsync() on a.UserId equals b.UserId
                                 join c in await _unitOfWork.statususer.GetAllAsync() on b.StatusUserId equals c.Id
                                 where a.UserId == id


                                 select new
                                 {
                                     Id = a.UserId,
                                     Email = b.Email,
                                     Status = c.Name,
                                     Name = b.Namee,
                                     Adresse = a.Address,
                                     PhoneNumber = a.PhoneNumber

                                 });

                return Ok(companies);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }


        [HttpDelete("id")]
        public async Task<IActionResult> Delete(int id)

        {
            try
            {
                var comp = await _unitOfWork.company.GetByIdAsync(id);
                if (comp == null)
                {
                    return NotFound();
                }
                await _unitOfWork.company.DeleteAsync(comp);
                await _unitOfWork.CompleteAsync();
                var usr = await _unitOfWork.user.GetByIdAsync(id);
                if (usr == null)
                {
                    return NotFound();
                }
                await _unitOfWork.user.DeleteAsync(usr);
                await _unitOfWork.CompleteAsync();
                return Ok(new { message = $"La société a été supprimé avec succès" });
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


        [HttpPost("Add")]
        public async Task<IActionResult> AddrequestAsync([FromBody] CompanyCreationDto companyCreationDto)
        {
            try
            {
                if (companyCreationDto == null)  
                {
                    return BadRequest(ModelState);
                }
                                
                var usr = new User
                {
                    Namee = companyCreationDto.Namee,
                    Email = companyCreationDto.Email,
                    Password = Helpers.PasswordHasher.HashPassword(companyCreationDto.Password),
                    
                    StatusUserId = companyCreationDto.StatusUserId,
                };

                // Ajouter la demande à la base de données
                await _unitOfWork.user.AddAsync(usr);
                await _unitOfWork.CompleteAsync();

                var comp = new Company
                {
                    UserId = usr.UserId,
                    PhoneNumber = companyCreationDto.PhoneNumber,   
                    Address = companyCreationDto.Address,
                    
                };

                // Ajouter la demande à la base de données
                await _unitOfWork.company.addAsync(comp);
                await _unitOfWork.CompleteAsync();





                return Ok(new { message = $"la societé a été ajouté avec succès" });

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateRequestByIdAsync([FromBody] CompanyCreationDto companyCreationDto, int  id)
        {
            try
            {
                if (companyCreationDto == null)
                {
                    return BadRequest(ModelState);
                }
                
                var existinguser = await _unitOfWork.user.GetByIdAsync(id);
                if (existinguser == null)
                {
                    return NotFound(new { message = $"L'utilisateur {companyCreationDto.Namee} n'existe pas." });
                }


                existinguser.Namee = companyCreationDto.Namee;
                existinguser.Email = companyCreationDto.Email;
                existinguser.Password = companyCreationDto.Password;
                existinguser.StatusUserId = companyCreationDto.StatusUserId;


                await _unitOfWork.user.UpdateAsync(existinguser);
                await _unitOfWork.CompleteAsync();

                var existingcompany = await _unitOfWork.company.GetByIdAsync(id);

                existingcompany.PhoneNumber = companyCreationDto.PhoneNumber; 
                existingcompany.Address = companyCreationDto.Address;
                await _unitOfWork.company.UpdateAsync(existingcompany);
                await _unitOfWork.CompleteAsync();

                return Ok(new { message = $"La societé a été mise à jour avec succès" });
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
