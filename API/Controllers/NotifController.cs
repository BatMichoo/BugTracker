using Core.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("notifications")]
    public class NotifController : BaseController
    {
        private readonly IBugNotificationRepository _repository;

        public NotifController(IBugNotificationRepository repository)
        {
            _repository = repository;
        }

        [HttpPut]
        public async Task<IActionResult> MarkRead(int id)
        {
            await _repository.MarkRead(id);

            return Ok();
        }
    }
}
