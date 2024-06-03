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
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FinalHackBank.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequesttController : ControllerBase
    {


        private readonly IUnitOfWork _unitOfWork;

        public RequesttController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        // chercher la liste de tous les demande
        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {

                var req = (from a in await _unitOfWork.requestt.GetAllAsync()
                          join b in await _unitOfWork.statusdemand.GetAllAsync() on a.StatusId equals b.Id
                          join c in await _unitOfWork.decision.GetAllAsync() on a.DecisionId equals c.Id
                           


                           select new
                           {
                               Id = a.RequestId,
                               Description = a.Description,
                               Daterequest = a.Daterequest,
                               Status = b.Name,
                               Decision = c.Name,
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

                var req = (from a in await _unitOfWork.requestt.GetAllAsync()
                           join b in await _unitOfWork.statusdemand.GetAllAsync() on a.StatusId equals b.Id
                           join c in await _unitOfWork.decision.GetAllAsync() on a.DecisionId equals c.Id
                           where a.RequestId == id


                           select new
                           {
                               Id = a.RequestId,
                               Description = a.Description,
                               Daterequest = a.Daterequest,
                               Status = b.Name,
                               Decision = c.Name,
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
        public async Task<IActionResult> AddrequestAsync([FromBody] RequesttDto requesttDto)
        {
            try
            {
                if (requesttDto == null)
                {
                    return BadRequest(ModelState);
                }

                var demande = new Requestt
                {
                    RequestId = requesttDto.RequestId,
                    Description = requesttDto.Description,
                    EmployeeId = requesttDto.EmployeeId,
                    Daterequest = requesttDto.Daterequest,
                    StatusId = requesttDto.StatusId,
                    DecisionId = requesttDto.DecisionId,
                };

                // Ajouter la demande à la base de données
                await _unitOfWork.requestt.addAsync(demande);
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
                
                existingRequest.Description = requesttDto.Description;
                existingRequest.EmployeeId = requesttDto.EmployeeId;
                existingRequest.Daterequest = requesttDto.Daterequest;
                existingRequest.StatusId = requesttDto.StatusId;
                existingRequest.DecisionId = requesttDto.DecisionId;

                
                await _unitOfWork.requestt.UpdateAsync(existingRequest);
                await _unitOfWork.CompleteAsync();


                return Ok(new { message = $"La demande mise à jour avec succès" });
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
