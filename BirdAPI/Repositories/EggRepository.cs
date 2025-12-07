using BirdAPI_lab4.Data;
using BirdAPI_lab4.Models;
using Microsoft.EntityFrameworkCore;

namespace BirdAPI_lab4.Repositories
{
    public class EggRepository : IEggRepository
    {
        private readonly BirdFarmDbContext _context;

        public EggRepository(BirdFarmDbContext context)
        {
            _context = context;
        }

        public async Task<Egg> AddAsync(Egg egg)
        {
            _context.Eggs.Add(egg);
            await _context.SaveChangesAsync();
            return egg;
        }

        public async Task DeleteAsync(int id)
        {
            var egg = await _context.Eggs.FindAsync(id);
            if (egg != null)
            {
                _context.Eggs.Remove(egg);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Egg>> GetAllAsync()
        {
            return await _context.Eggs.ToListAsync();
        }

        public async Task<Egg> GetByIdAsync(int id)
        {
            return await _context.Eggs.FindAsync(id);
        }

        public async Task UpdateAsync(Egg egg)
        {
            _context.Entry(egg).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}