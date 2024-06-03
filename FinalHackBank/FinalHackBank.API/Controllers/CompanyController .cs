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

                var req  = await _unitOfWork.company.GetAllAsync();
                var dtos = _mapper.Map<List<BidDto>>(req);
                return Ok(dtos);

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

                var req = (from a in await _unitOfWork.bid.GetAllAsync()
                           join b in await _unitOfWork.documentt.GetAllAsync() on a.DocumentId equals b.DocumentId
                           join c in await _unitOfWork.company.GetAllAsync() on a.CompanyId equals c.UserId
                           
                           
                           where a.BidId == id


                           select new
                           {
                               Id = a.BidId,

                              DocumentId =  a.DocumentId ,
                               CompanyId = a.CompanyId ,
                              MontantTTC=a.AmountTtc

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
                var off = await _unitOfWork.bid.GetByIdAsync(id);
                if (off == null)
                {
                    return NotFound();
                }
                await _unitOfWork.bid.DeleteAsync(off);
                await _unitOfWork.CompleteAsync();
                return Ok(new { message = $"L'offre a été supprimé avec succès" });
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


        [HttpPost("Add")]
        public async Task<IActionResult> AddrequestAsync([FromBody] BidDto bidDto)
        {
            try
            {
                if (bidDto == null)
                {
                    return BadRequest(ModelState);
                }
                                
                var bidd = new Bid
                {
                  CompanyId = bidDto.CompanyId,
                  DocumentId = bidDto.DocumentId,
                  AmountTtc = bidDto.AmountTtc,
                };

                // Ajouter la demande à la base de données
                await _unitOfWork.bid.AddAsync(bidd);
                await _unitOfWork.CompleteAsync();
                
                return Ok(new { message = $"l'offre a été ajouté avec succès" });

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateRequestByIdAsync([FromBody] BidDto bidDto, int  id)
        {
            try
            {
                if (bidDto == null)
                {
                    return BadRequest(ModelState);
                }
                
                var existingBid = await _unitOfWork.bid.GetByIdAsync(id);
                if (existingBid == null)
                {
                    return NotFound(new { message = $"L'offre {id} n'existe pas." });
                }
                           

                existingBid.CompanyId= bidDto.CompanyId;
                existingBid.DocumentId = bidDto.DocumentId;
                existingBid.AmountTtc = bidDto.AmountTtc;

                
                await _unitOfWork.bid.UpdateAsync(existingBid);
                await _unitOfWork.CompleteAsync();


                return Ok(new { message = $"L'offre mise à jour avec succès" });
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
