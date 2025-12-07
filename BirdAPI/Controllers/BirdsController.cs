using AutoMapper;
using BirdAPI_lab4.Configuration;
using BirdAPI_lab4.DTOs;
using BirdAPI_lab4.Models;
using BirdAPI_lab4.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace BirdAPI_lab4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BirdsController : ControllerBase
    {
        private readonly IBirdRepository _birdRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateBirdDto> _createValidator;
        private readonly IValidator<UpdateBirdDto> _updateValidator;
        private readonly IMemoryCache _cache;
        private readonly FarmSettings _settings;

        public BirdsController(
            IBirdRepository birdRepository,
            IMapper mapper,
            IValidator<CreateBirdDto> createValidator,
            IValidator<UpdateBirdDto> updateValidator,
            IMemoryCache cache,
            IOptions<FarmSettings> settings)
        {
            _birdRepository = birdRepository;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _cache = cache;
            _settings = settings.Value;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BirdDto>>> GetAllBirds()
        {
            string cacheKey = "all_birds";

            if (!_cache.TryGetValue(cacheKey, out IEnumerable<BirdDto> birdsDto))
            {
                var birds = await _birdRepository.GetAllAsync();
                birdsDto = _mapper.Map<IEnumerable<BirdDto>>(birds);

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(_settings.CacheDurationSeconds));

                _cache.Set(cacheKey, birdsDto, cacheEntryOptions);
            }

            return Ok(birdsDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BirdDto>> GetBirdById(int id)
        {
            var bird = await _birdRepository.GetByIdAsync(id);
            if (bird == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<BirdDto>(bird));
        }

        [HttpPost]
        public async Task<ActionResult<BirdDto>> AddBird(CreateBirdDto createDto)
        {
            if (_settings.IsMaintenanceMode)
            {
                return StatusCode(503, "Farm is in maintenance mode. Adding new birds is currently disabled.");
            }

            var validationResult = await _createValidator.ValidateAsync(createDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.ToDictionary(e => e.PropertyName, e => e.ErrorMessage));
            }

            var bird = _mapper.Map<Bird>(createDto);
            await _birdRepository.AddAsync(bird);

            _cache.Remove("all_birds");

            var birdDto = _mapper.Map<BirdDto>(bird);
            return CreatedAtAction(nameof(GetBirdById), new { id = birdDto.Id }, birdDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBird(int id, UpdateBirdDto updateDto)
        {
            if (_settings.IsMaintenanceMode)
            {
                return StatusCode(503, "Farm is in maintenance mode. Updating birds is currently disabled.");
            }

            var validationResult = await _updateValidator.ValidateAsync(updateDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.ToDictionary(e => e.PropertyName, e => e.ErrorMessage));
            }

            var bird = await _birdRepository.GetByIdAsync(id);
            if (bird == null)
            {
                return NotFound();
            }

            _mapper.Map(updateDto, bird);
            await _birdRepository.UpdateAsync(bird);

            _cache.Remove("all_birds");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBird(int id)
        {
            if (_settings.IsMaintenanceMode)
            {
                return StatusCode(503, "Farm is in maintenance mode. Deleting birds is currently disabled.");
            }

            var bird = await _birdRepository.GetByIdAsync(id);
            if (bird == null)
            {
                return NotFound();
            }

            await _birdRepository.DeleteAsync(id);

            _cache.Remove("all_birds");

            return NoContent();
        }
    }
}