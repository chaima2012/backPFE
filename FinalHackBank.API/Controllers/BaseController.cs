using AutoMapper;
using FinalHackBank.CORE.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinalHackBank.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController<TEntity, TDto> : ControllerBase where TEntity : class, IEntityWithName where TDto : class, IEntityWithName
    {


        private readonly IMapper _mapper;
        private readonly IBaseRepository<TEntity> _repository;
        string entityName = typeof(TEntity).Name;

        public BaseController(IMapper mapper, IBaseRepository<TEntity> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        /* GetById*/

        [HttpPost("id")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null) { return NotFound(); };
                var dto = _mapper.Map<TDto>(entity);
                return Ok(dto);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        // GetAll
        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {

                var entities = await _repository.GetAllAsync();
                var dtos = _mapper.Map<List<TDto>>(entities);
                return Ok(dtos);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        /* GetByName*/

        [HttpPost("GetByName")]
        public async Task<IActionResult> GetByNameAsync(string nom)
        {
            try
            {
                var entities = await _repository.FindAsync(b => b.Name == nom);
                if (entities == null) { return NotFound(); };
                var dtos = _mapper.Map<TDto>(entities);
                return Ok(dtos);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        /* supprimer une unite fonctionnelle selon id technicien et id diplome*/

        [HttpPost("returnid")]
        public async Task<IActionResult> GetIdapartirLibelle(string nom)
        {
            try
            {
                var entities = await _repository.FindAsync(b => b.Name == nom);
                if (entities == null) { return NotFound(); };


                return Ok(new { Response = entities.Id });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }






        // Add
        [HttpPost("Add")]
        public async Task<IActionResult> AddAsync([FromBody] TDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest(ModelState);
                }

                var existingentity = await _repository.FindAsync(s => s.Name.Trim().ToUpper() == dto.Name.Trim().ToUpper());
                if (existingentity != null)
                {
                    ModelState.AddModelError("", $"{entityName} existe déja dans la base de donnée!");
                    return StatusCode(422, ModelState);
                }

                var entity = _mapper.Map<TEntity>(dto);
                await _repository.addAsync(entity);


                return Ok(new { message = $"{entityName} ajouté avec sucess" });
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        // Delete
        [HttpDelete("id")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null)
                {
                    return NotFound();
                }

                await _repository.DeleteAsync(entity);
                return Ok(new { message = $"{entityName} supprimé avec sucess" });
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        // Update
        [HttpPut("id")]
        public async Task<IActionResult> UpdateAsync([FromBody] TDto dto, int id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null) { return NotFound(); }

                entity.Name = dto.Name ;
                _mapper.Map(dto, entity);
                await _repository.UpdateAsync(entity);

                // Return a JSON object with a success message
                return Ok(new { message = $"{entityName} modifié avec succès" });
            }
            catch (Exception ex)
            {
                // Return a JSON object with an error message
                return BadRequest(new { error = ex.Message });
            }
        }


        /* supprimer selon son nom*/

        [HttpDelete("Nom")]
        public async Task<IActionResult> DeleteByName(string nom)
        {
            try
            {
                var entity = await _repository.FindAsync(b => b.Name == nom);
                if (entity == null)
                {
                    return NotFound();
                }
                await _repository.DeleteAsync(entity);
                return Ok(new { message = $"{entityName} supprimé avec sucess" });
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        /* modifier Etat MPC selon son nom*/

        [HttpPut("Nom")]
        public async Task<IActionResult> UpdateEtatMPCByName([FromBody] TDto dto, string nom)
        {
            try
            {
                var entity = await _repository.FindAsync(b => b.Name == nom);
                if (entity == null) { return NotFound(); }
                entity.Name = dto.Name;
                await _repository.UpdateAsync(entity);
                return Ok(new { message = $"{entityName} modifié avec sucess" });
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

    }
}


