


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
    public class DocumenttController : ControllerBase
    {


        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DocumenttController(IMapper mapper, IUnitOfWork unitOfWork)
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

                var doc = await _unitOfWork.documentt.GetAllAsync();
                var dtos = _mapper.Map<List<DocumenttDto>>(doc);
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

                var bidd = await _unitOfWork.bid.FirstOrDefaultAsync(b => b.BidId == id);
                var dtos = _mapper.Map<List<BidDto>>(bidd);
                return Ok(dtos);


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
                var doc = await _unitOfWork.documentt.GetByIdAsync(id);
                if (doc == null)
                {
                    return NotFound();
                }
                await _unitOfWork.documentt.DeleteAsync(doc);
                await _unitOfWork.CompleteAsync();
                return Ok(new { message = $"Le document a été supprimé avec succès" });
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


        [HttpPost("Add")]
        public async Task<IActionResult> AdddocumenttAsync([FromBody] DocumenttDto docDto)
        {
            try
            {
                if (docDto == null)
                {
                    return BadRequest(ModelState);
                }

                var doc = new Documentt
                {
                    Title = docDto.Title,
                    DocumentId = docDto.DocumentId,
                    Size = docDto.Size,
                    Content = docDto.Content,
                };

                // Ajouter la demande à la base de données
                await _unitOfWork.documentt.AddAsync (doc);
                await _unitOfWork.CompleteAsync();

                return Ok(new { message = $"l'offre a été ajouté avec succès" });

            }

            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdatedocumenttByIdAsync([FromBody] DocumenttDto docDto, int id)
        {
            try
            {
                if (docDto == null)

                {
                    return BadRequest(ModelState);
                }

                var existingdoc = await _unitOfWork.documentt.GetByIdAsync(id);
                if (existingdoc == null)
                {
                    return NotFound(new { message = $"Le document {id} n'existe pas." });
                }


                existingdoc.Title = docDto.Title;
                existingdoc.DocumentId = docDto.DocumentId;
                existingdoc.Size= docDto.Size;
                existingdoc.Content = docDto.Content;

                await _unitOfWork.documentt.UpdateAsync(existingdoc);
                await _unitOfWork.CompleteAsync();


                return Ok(new { message = $"le docupment est mis à jour avec succès" });
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}

