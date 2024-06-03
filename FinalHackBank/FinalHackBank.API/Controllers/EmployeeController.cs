
/*using AutoMapper;
using Azure.Core;
using FinalHackBank.CORE;
using FinalHackBank.CORE.Dto;
using FinalHackBank.CORE.Interfaces;
using FinalHackBank.CORE.Models;
using FinalHackBank.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FinalHackBank.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {


        private readonly IUnitOfWork _unitOfWork;

        public EmployeeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        // chercher la liste de tous les demande
        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {

                var req = (from a in await _unitOfWork.employee.GetAllAsync()
                           join b in await _unitOfWork.requestt.GetAllAsync() on a.RequestId equals b.Id
                          join c in await _unitOfWork.statusdemand.GetAllAsync() on a.StatusId equals c.Id
                          join d in await _unitOfWork.decision.GetAllAsync() on a.DecisionId equals d.Id
                           //join e in await _unitOfWork.employee.GetAllAsync() on a.EmployeeId equals e.Id
                           //join e in await _unitOfWork.user.GetAllAsync() on a.EmployeeId equals e.Id


                           select new
                           {
                               Id = a.RequestId,
                               Description = a.Description,
                               Date_ = a.Date_,
                               //Employee = a.Name,
                               Status = c.Name,
                               Decision = d.Name,
                               Department = b.Name,
                           });


                return Ok(req); ;

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

                var req = (from a in await _unitOfWork.employee.GetAllAsync()
                           join b in await _unitOfWork.department.GetAllAsync() on a.DepartmentId equals b.Id
                           join c in await _unitOfWork.statusdemand.GetAllAsync() on a.StatusId equals c.Id
                           join d in await _unitOfWork.decision.GetAllAsync() on a.DecisionId equals d.Id
                           //join e in await _unitOfWork.employee.GetAllAsync() on a.EmployeeId equals e.Id
                           //join e in await _unitOfWork.user.GetAllAsync() on a.EmployeeId equals e.Id
                           where a.RequestId == id


                           select new
                           {
                               Id = a.RequestId,
                               Description = a.Description,
                               Date_ = a.Date_,
                               //Employee = a.Name,
                               Status = c.Name,
                               Decision = d.Name,
                               Department = b.Name,
                           });


                return Ok(req); ;

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
                var req = await _unitOfWork.requestt.GetByIdAsync(id);
                if (req == null)
                {
                    return NotFound();
                }
                await _unitOfWork.requestt.DeleteAsync(req);
                await _unitOfWork.CompleteAsync();
                return Ok(new { message = $"La demande a été supprimée avec succès" });
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


        [HttpPost("Add")]
        public async Task<IActionResult> AddEquipementAsync([FromBody] RequesttDto requesttDto)
        {
            try
            {
                if (requesttDto == null)
                {
                    return BadRequest(ModelState);
                }
                var employee = await _unitOfWork.employee.FindAsync(b => b.UserId == requesttDto.EmployeeId);
                var department = await _unitOfWork.department.FindAsync(b => b.Id == requesttDto.DepartmentId);
                var decision = await _unitOfWork.decision.FindAsync(b => b.Id == requesttDto.DecisionId);
                var statusdemand = await _unitOfWork.statusdemand.FindAsync(b => b.Id == requesttDto.StatusId);


                var equipement = new Requestt
                {
                    RequestId = requesttDto.RequestId,
                    Description = requesttDto.Description,
                    EmployeeId = requesttDto.EmployeeId,
                    Date_ = requesttDto.Date_,
                    StatusId = requesttDto.StatusId,
                    DecisionId = requesttDto.DecisionId,
                    DepartmentId = requesttDto.DepartmentId,
                };

                // Ajouter l'équipement à la base de données
                await _unitOfWork.requestt.addAsync(equipement);
                await _unitOfWork.CompleteAsync();
                
                return Ok(new { message = $"la demande a été ajouté avec succès" });

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateRequestByIdAsync([FromBody] RequesttDto requesttDto, int  id)
        {
            try
            {
                if (requesttDto == null)
                {
                    return BadRequest(ModelState);
                }
                
                var existingRequest = await _unitOfWork.requestt.GetByIdAsync(id);
                if (existingRequest == null)
                {
                    return NotFound(new { message = $"La demande {id} n'existe pas." });
                }
                var codedepartment = await _unitOfWork.department.GetByIdAsync(requesttDto.DepartmentId);
                var Codestatusdemand = await _unitOfWork.statusdemand.GetByIdAsync(requesttDto.StatusId);
                var Codedecision = await _unitOfWork.decision.GetByIdAsync(requesttDto.DecisionId);
                var Codeemployee = await _unitOfWork.employee.GetByIdAsync(requesttDto.EmployeeId);


                existingRequest.Description = requesttDto.Description;
                existingRequest.EmployeeId = requesttDto.EmployeeId;
                existingRequest.Date_ = requesttDto.Date_;
                existingRequest.StatusId = requesttDto.StatusId;
                existingRequest.DecisionId = requesttDto.DecisionId;
                existingRequest.DepartmentId = requesttDto.DepartmentId;

                
                await _unitOfWork.requestt.UpdateAsync(existingRequest);
                await _unitOfWork.CompleteAsync();


                return Ok(new { message = $"Equipement mise à jour avec succès" });
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
*/