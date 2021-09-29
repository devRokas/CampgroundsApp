using System;
using System.Threading.Tasks;
using Persistence.Models;

namespace Persistence.Repositories
{
    public interface IImagesRepository
    {
        Task<int> SaveAsync(ImageReadModel model);
        
        Task<CampgroundReadModel> GetAsync(Guid campgroundId);
        
        Task<int> DeleteByIdAsync(Guid id);
        
        Task<int> DeleteByCampgroundIdAsync(Guid campgroundId);
    }
}