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
    public class CallController : ControllerBase
    {


        private readonly IUnitOfWork _unitOfWork;

        public CallController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        // chercher la liste de tous les appels
        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {

                var calls = (from a in await _unitOfWork.call.GetAllAsync()
                            join b in await _unitOfWork.status.GetAllAsync() on a.StatusId equals b.Id
                            


                            select new
                            {
                                Id = a.CallId,
                                Description = a.Descriptions,
                                Status = b.Name,
                                Budget = a.Budget,
                                Remarque = a.Remark,
                            });


                return Ok(calls); ;

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }


        // chercher un appel selon son id

        [HttpPost("GetById")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {

                var call = (from a in await _unitOfWork.call.GetAllAsync()
                           join b in await _unitOfWork.status.GetAllAsync() on a.StatusId equals b.Id
                           where a.CallId == id


                           select new
                           {
                               Id = a.CallId,
                               Description = a.Descriptions,
                               Status = b.Name,
                               Budget = a.Budget,
                               Remarque = a.Remark,
                           });


                return Ok(call); ;

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
                var call = await _unitOfWork.call.GetByIdAsync(id);
                if (call == null)
                {
                    return NotFound();
                }
                await _unitOfWork.call.DeleteAsync(call);
                await _unitOfWork.CompleteAsync();
                return Ok(new { message = $"L'appel a été supprimée avec succès" });
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


        [HttpPost("Add")]
        public async Task<IActionResult> AddEquipementAsync([FromBody] CallDto callDto)
        {
            try
            {
                if (callDto == null)
                {
                    return BadRequest(ModelState);
                }

                var apl = new Call
                {
                    CallId = callDto.CallId,
                    Descriptions = callDto.Descriptions,
                    StatusId = callDto.StatusId,
                    Budget = callDto.Budget,
                    Remark = callDto.Remark,
                };

                // Ajouter l'équipement à la base de données
                await _unitOfWork.call.addAsync(apl);
                await _unitOfWork.CompleteAsync();
                
                return Ok(new { message = $"la appel a été ajouté avec succès" });

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdatecallByIdAsync([FromBody] CallDto callDto, int  id)
        {
            try
            {
                if (callDto == null)
                {
                    return BadRequest(ModelState);
                }

                var existingcall = await _unitOfWork.call.GetByIdAsync(id);
                if (existingcall == null)
                {
                    return NotFound(new { message = $"La demande {id} n'existe pas." });
                }

                existingcall.Descriptions = callDto.Descriptions;
                existingcall.StatusId = callDto.StatusId;
                existingcall.Budget = callDto.Budget;
                existingcall.Remark = callDto.Remark;



                await _unitOfWork.call.UpdateAsync(existingcall);
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

