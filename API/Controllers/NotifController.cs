using Core.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("notifications")]
    public class NotifController : BaseController
    {
        private readonly INotifRepository _repository;

        public NotifController(INotifRepository repository)
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
