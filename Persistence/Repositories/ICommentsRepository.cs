using System;
using System.Threading.Tasks;
using Persistence.Models;

namespace Persistence.Repositories
{
    public interface ICommentsRepository
    {
        Task<int> SaveOrUpdateAsync(CommentReadModel model);
        
        Task<CampgroundReadModel> GetAsync(Guid campgroundId);

        Task<CampgroundReadModel> GetAsync(Guid id, Guid userId);
        
        Task<int> DeleteAsync(Guid id, Guid userId);
    }
}