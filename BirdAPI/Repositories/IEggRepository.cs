using BirdAPI_lab4.Models;

namespace BirdAPI_lab4.Repositories
{
    public interface IEggRepository
    {
        Task<IEnumerable<Egg>> GetAllAsync();
        Task<Egg> GetByIdAsync(int id);
        Task<Egg> AddAsync(Egg egg);
        Task UpdateAsync(Egg egg);
        Task DeleteAsync(int id);
    }
}