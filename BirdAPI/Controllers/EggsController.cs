using AutoMapper;
using BirdAPI_lab4.DTOs;
using BirdAPI_lab4.Models;
using BirdAPI_lab4.Repositories;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

namespace BirdAPI_lab4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EggsController : ControllerBase
    {
        private readonly IEggRepository _eggRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateEggDto> _createValidator;
        private readonly IValidator<UpdateEggDto> _updateValidator;

        public EggsController(
            IEggRepository eggRepository,
            IMapper mapper,
            IValidator<CreateEggDto> createValidator,
            IValidator<UpdateEggDto> updateValidator)
        {
            _eggRepository = eggRepository;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EggDto>>> GetAllEggs()
        {
            var eggs = await _eggRepository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<EggDto>>(eggs));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EggDto>> GetEggById(int id)
        {
            var egg = await _eggRepository.GetByIdAsync(id);
            if (egg == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<EggDto>(egg));
        }

        [HttpPost]
        public async Task<ActionResult<EggDto>> AddEgg(CreateEggDto createDto)
        {
            var validationResult = await _createValidator.ValidateAsync(createDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.ToDictionary(e => e.PropertyName, e => e.ErrorMessage));
            }

            var egg = _mapper.Map<Egg>(createDto);
            await _eggRepository.AddAsync(egg);

            var eggDto = _mapper.Map<EggDto>(egg);
            return CreatedAtAction(nameof(GetEggById), new { id = eggDto.Id }, eggDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEgg(int id, UpdateEggDto updateDto)
        {
            var validationResult = await _updateValidator.ValidateAsync(updateDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.ToDictionary(e => e.PropertyName, e => e.ErrorMessage));
            }

            var egg = await _eggRepository.GetByIdAsync(id);
            if (egg == null)
            {
                return NotFound();
            }

            _mapper.Map(updateDto, egg);
            await _eggRepository.UpdateAsync(egg);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEgg(int id)
        {
            var egg = await _eggRepository.GetByIdAsync(id);
            if (egg == null)
            {
                return NotFound();
            }

            await _eggRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}