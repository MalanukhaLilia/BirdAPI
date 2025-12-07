using BirdAPI_lab4.Data;
using BirdAPI_lab4.Models;
using Microsoft.EntityFrameworkCore;

namespace BirdAPI_lab4.Repositories
{
    public class BirdRepository : IBirdRepository
    {
        private readonly BirdFarmDbContext _context;
        private readonly ILogger<BirdRepository> _logger;

        public BirdRepository(BirdFarmDbContext context, ILogger<BirdRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Bird> AddAsync(Bird bird)
        {
            _logger.LogInformation("Attempting to add a new bird: {Name}", bird.Name);
            _context.Birds.Add(bird);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Bird successfully added with ID: {Id}", bird.Id);
            return bird;
        }

        public async Task DeleteAsync(int id)
        {
            _logger.LogInformation("Attempting to delete bird with ID: {Id}", id);
            var bird = await _context.Birds.FindAsync(id);
            if (bird != null)
            {
                _context.Birds.Remove(bird);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Bird with ID: {Id} deleted successfully", id);
            }
            else
            {
                _logger.LogWarning("Bird with ID: {Id} not found for deletion", id);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Birds.AnyAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Bird>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all birds from database");
            var birds = await _context.Birds.ToListAsync();
            _logger.LogInformation("Retrieved {Count} birds", birds.Count);
            return birds;
        }

        public async Task<Bird> GetByIdAsync(int id)
        {
            _logger.LogInformation("Fetching bird with ID: {Id}", id);
            return await _context.Birds.FindAsync(id);
        }

        public async Task UpdateAsync(Bird bird)
        {
            _logger.LogInformation("Updating bird with ID: {Id}", bird.Id);
            _context.Entry(bird).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            _logger.LogInformation("Bird with ID: {Id} updated successfully", bird.Id);
        }
    }
}