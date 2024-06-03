using AutoMapper;
using Azure.Core;
using FinalHackBank.CORE;
using FinalHackBank.CORE.Dto;
using FinalHackBank.CORE.Interfaces;
using FinalHackBank.CORE.Models;
using FinalHackBank.EF;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.Design;
using System.Net;
using System.Security.Cryptography;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using FinalHackBank.API.Helpers;


namespace FinalHackBank.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {


        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly string _uploadFolder;

        public EmployeeController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedDocuments");

        }


        // chercher la liste de tous les demande
        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {

                var emps = (from a in await _unitOfWork.employee.GetAllAsync()
                           join b in await _unitOfWork.user.GetAllAsync() on a.UserId equals b.UserId
                           join c in await _unitOfWork.statususer.GetAllAsync() on b.StatusUserId equals c.Id
                           join d in await _unitOfWork.role.GetAllAsync() on a.RoleId equals d.Id
                           join e in await _unitOfWork.department.GetAllAsync() on a.DepartmentId equals e.Id



                           select new
                           {
                               Id = a.UserId,
                               Email = b.Email,
                               Status = c.Name,
                               Name = b.Namee,
                               role = d.Name,
                               department = e.Name,

                           });

                return Ok(emps); 

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

                var emp = (from a in await _unitOfWork.employee.GetAllAsync()
                            join b in await _unitOfWork.user.GetAllAsync() on a.UserId equals b.UserId
                            join c in await _unitOfWork.statususer.GetAllAsync() on b.StatusUserId equals c.Id
                            join d in await _unitOfWork.role.GetAllAsync() on a.RoleId equals d.Id
                            join e in await _unitOfWork.department.GetAllAsync() on a.DepartmentId equals e.Id
                            where a.UserId == id


                           select new
                            {
                                Id = a.UserId,
                                Email = b.Email,
                                Status = c.Name,
                                Name = b.Namee,
                                role = d.Name,
                                department = e.Name,

                            });

                return Ok(emp);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }


[HttpPut("UpdateUserDetails")]
public async Task<IActionResult> UpdateUserDetails([FromBody] UserUpdateDto userUpdateDto)
{
    try
    {
        var user = await _unitOfWork.user.GetByIdAsync(userUpdateDto.UserId);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        user.Namee = userUpdateDto.Name;
        user.Email = userUpdateDto.Email;

        await _unitOfWork.user.UpdateAsync(user);
        await _unitOfWork.CompleteAsync();

        return Ok(new { message = "User details updated successfully." });
    }
    catch (Exception ex)
    {
        return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
    }
}

// Endpoint to update user's password
[HttpPut("UpdatePassword")]
public async Task<IActionResult> UpdatePassword([FromBody] PasswordUpdateDto passwordUpdateDto)
{
    try
    {
        var user = await _unitOfWork.user.GetByIdAsync(passwordUpdateDto.UserId);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        user.Password = Helpers.PasswordHasher.HashPassword(passwordUpdateDto.NewPassword);

        await _unitOfWork.user.UpdateAsync(user);
        await _unitOfWork.CompleteAsync();

        return Ok(new { message = "Password updated successfully." });
    }
    catch (Exception ex)
    {
        return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
    }
}

        [HttpDelete("id")]
        public async Task<IActionResult> Delete(int id)

        {
            try
            {
                var emp = await _unitOfWork.employee.GetByIdAsync(id);
                if (emp == null)
                {
                    return NotFound();
                }
                await _unitOfWork.employee.DeleteAsync(emp);
                await _unitOfWork.CompleteAsync();
                var usr = await _unitOfWork.user.GetByIdAsync(id);
                if (usr == null)
                {
                    return NotFound();
                }
                await _unitOfWork.user.DeleteAsync(usr);
                await _unitOfWork.CompleteAsync();
                return Ok(new { message = $"L'employé a été supprimé avec succès" });
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


        [HttpPost("Add")]
        public async Task<IActionResult> AddrequestAsync([FromBody] EmployeeCreationDto employeeCreationDto)
        {
            try
            {
                if (employeeCreationDto == null)  
                {
                    return BadRequest(ModelState);
                }
                                
                var usr = new User
                {
                    Namee = employeeCreationDto.Namee,
                    Email = employeeCreationDto.Email,
                    Password = Helpers.PasswordHasher.HashPassword(employeeCreationDto.Password),
                    StatusUserId = employeeCreationDto.StatusUserId,
                };

                // Ajouter la demande à la base de données
                await _unitOfWork.user.AddAsync(usr);
                await _unitOfWork.CompleteAsync();

                var emp = new Employee
                {
                    UserId = usr.UserId,
                    RoleId = employeeCreationDto.RoleId,
                    DepartmentId = employeeCreationDto.DepartmentId,
                    
                };

                // Ajouter la demande à la base de données
                await _unitOfWork.employee.addAsync(emp);
                await _unitOfWork.CompleteAsync();





                return Ok(new { message = $"l'employé a été ajouté avec succès" });

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateRequestByIdAsync([FromBody] EmployeeCreationDto employeeCreationDto, int  id)
        {
            try
            {
                if (employeeCreationDto == null)
                {
                    return BadRequest(ModelState);
                }
                
                var existinguser = await _unitOfWork.user.GetByIdAsync(id);
                if (existinguser == null)
                {
                    return NotFound(new { message = $"L'utilisateur {employeeCreationDto.Namee} n'existe pas." });
                }


                existinguser.Namee = employeeCreationDto.Namee;
                existinguser.Email = employeeCreationDto.Email;
                existinguser.Password = employeeCreationDto.Password;
                existinguser.StatusUserId = employeeCreationDto.StatusUserId;


                await _unitOfWork.user.UpdateAsync(existinguser);
                await _unitOfWork.CompleteAsync();

                var existingemployee = await _unitOfWork.employee.GetByIdAsync(id);

                existingemployee.RoleId = employeeCreationDto.RoleId;
                existingemployee.DepartmentId = employeeCreationDto.DepartmentId;
                await _unitOfWork.employee.UpdateAsync(existingemployee);
                await _unitOfWork.CompleteAsync();

                return Ok(new { message = $"L'employé a été mise à jour avec succès" });
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
public class UserUpdateDto
{
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}

public class PasswordUpdateDto
{
    public int UserId { get; set; }
    public string NewPassword { get; set; }
}
