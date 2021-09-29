using System;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Models.Request;
using Contracts.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence.Models;
using Persistence.Repositories;

namespace RestAPI.Controllers
{
    [ApiController]
    [Route("campgrounds")]
    public class CampgroundsController : ControllerBase
    {
        private readonly ICampgroundsRepository _campgroundsRepository;
        private readonly IUsersRepository _usersRepository;

        public CampgroundsController(ICampgroundsRepository campgroundsRepository, IUsersRepository usersRepository)
        {
            _campgroundsRepository = campgroundsRepository;
            _usersRepository = usersRepository;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CreateCampgroundResponse>> CreateCampground(CreateCampgroundRequest request)
        {
            var firebaseId = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "user_id").Value;

            var user = await _usersRepository.GetAsync(firebaseId);

            var campground = new CampgroundReadModel
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Name = request.Name,
                Price = request.Price,
                Description = request.Description,
                DateCreated = DateTime.Now
            };

            await _campgroundsRepository.SaveOrUpdateAsync(campground);

            return Ok(new CreateCampgroundResponse
            {
                Id = campground.Id,
                UserId = campground.UserId,
                Name = campground.Name,
                Price = campground.Price,
                Description = campground.Description,
                DateCreated = campground.DateCreated
            });
        }
    }
}