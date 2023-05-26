using Business.Concrete;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PortfoyProje.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ProjeDetayController : ControllerBase
    {
        private readonly IProjeDetayService _projeDetayService;

        public ProjeDetayController(IProjeDetayService projeDetayService)
        {
            _projeDetayService = projeDetayService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _projeDetayService.GetAll();

            return Ok(result);
        }

        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id < 1)
                return BadRequest("Item Not Found");

            var result = await _projeDetayService.Get(id);

            if (result.Data == null)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProjeDetay projeDetay)
        {
            var result = await _projeDetayService.AddAsync(projeDetay);

            if (!result.Success)
                return BadRequest(new { isSuccess = false, Message = "Kayıt Başarısız" });
            return Ok(new { isSuccess = true, Message = "Kayıt Başarılı" });
        }

        [HttpPut]
        public async Task<IActionResult> Update(ProjeDetay projeDetay)
        {
            var result = await _projeDetayService.Update(projeDetay);

            if (!result.Success)
                return BadRequest(new { isSuccess = false, Message = result.Message });
            return Ok(new { isSuccess = true, Message = "Kayıt Başarıyla Güncellendi" });
        }

        [HttpDelete]
        public async Task<IActionResult> Delte(int id)
        {
            var result = await _projeDetayService.Delete(id);

            if (!result.Success)
                return BadRequest(new { isSuccess = false, Message = "Kayıt Silme Başarısız" });
            return Ok(new { isSuccess = true, Message = "Kayıt Başarıyla Silindi" });
        }
    }
}
