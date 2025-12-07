using BirdAPI_lab4.Models;

namespace BirdAPI_lab4.Repositories
{
    public interface IBirdRepository
    {
        Task<IEnumerable<Bird>> GetAllAsync();
        Task<Bird> GetByIdAsync(int id);
        Task<Bird> AddAsync(Bird bird);
        Task UpdateAsync(Bird bird);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}