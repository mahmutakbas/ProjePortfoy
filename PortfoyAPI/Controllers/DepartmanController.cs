using Business.Concrete;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PortfoyProje.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class DepartmanController : ControllerBase
    {
        private readonly IDepartmanService _departmanService;

        public DepartmanController(IDepartmanService departmanService)
        {
            _departmanService = departmanService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _departmanService.GetAll();

            return Ok(result);
        }

        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id < 1)
                return BadRequest("Item Not Found");

            var result = await _departmanService.Get(id);

            if (result.Data == null)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Departman departman)
        {
            var result = await _departmanService.AddAsync(departman);

            if (!result.Success)
                return BadRequest(new { isSuccess = false, Message = result.Message });
            return Ok(new { isSuccess = true, Message = "Kayıt Başarılı" });
        }

        [HttpPut]
        public async Task<IActionResult> Update(Departman departman)
        {
            var result = await _departmanService.Update(departman);

            if (!result.Success)
                return BadRequest(new { isSuccess = false, Message = result.Message });
            return Ok(new { isSuccess = true, Message = "Kayıt Başarıyla Güncellendi" });
        }

        [HttpDelete]
        public async Task<IActionResult> Delte(int id)
        {
            var result = await _departmanService.Delete(id);

            if (!result.Success)
                return BadRequest(new { isSuccess = false, Message = "Kayıt Silme Başarısız" });
            return Ok(new { isSuccess = true, Message = "Kayıt Başarıyla Silindi" });
        }
    }
}
