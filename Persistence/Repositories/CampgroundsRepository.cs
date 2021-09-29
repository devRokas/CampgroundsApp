using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Persistence.Models;

namespace Persistence.Repositories
{
    public class CampgroundsRepository : ICampgroundsRepository
    {
        private const string TableName = "Campgrounds";
        private readonly ISqlClient _sqlClient;

        public CampgroundsRepository(ISqlClient sqlClient)
        {
            _sqlClient = sqlClient;
        }

        public Task<int> SaveOrUpdateAsync(CampgroundReadModel model)
        {
            var sql = @$"INSERT INTO {TableName} (Id, UserId, Name, Price, Description, DateCreated) 
                        VALUES (@Id, @UserId, @Name, @Price, @Description, @DateCreated)
                        ON DUPLICATE KEY UPDATE Name = @Name, Price = @Price, Description = @Description";

            return _sqlClient.ExecuteAsync(sql, model);
        }

        public Task<IEnumerable<CampgroundReadModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<CampgroundReadModel> GetAsync(Guid id, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}