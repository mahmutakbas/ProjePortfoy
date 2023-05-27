using Business.Concrete;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PortfoyProje.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class KPIController : ControllerBase
    {
        private readonly IKPIService _kPIService;

        public KPIController(IKPIService kPIService)
        {
            _kPIService = kPIService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _kPIService.GetAll();

            return Ok(result);
        }

        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id < 1)
                return BadRequest("Item Not Found");

            var result = await _kPIService.Get(id);

            if (result.Data == null)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(KPI kPI)
        {
            var result = await _kPIService.AddAsync(kPI);

            if (!result.Success)
                return BadRequest(new { isSuccess = false, Message = "Kayıt Başarısız" });
            return Ok(new { isSuccess = true, Message = "Kayıt Başarılı" });
        }

        [HttpPut]
        public async Task<IActionResult> Update(KPI kPI)
        {
            var result = await _kPIService.Update(kPI);

            if (!result.Success)
                return BadRequest(new { isSuccess = false, Message = result.Message });
            return Ok(new { isSuccess = true, Message = "Kayıt Başarıyla Güncellendi" });
        }

        [HttpDelete]
        public async Task<IActionResult> Delte(int id)
        {
            var result = await _kPIService.Delete(id);

            if (!result.Success)
                return BadRequest(new { isSuccess = false, Message = "Kayıt Silme Başarısız" });
            return Ok(new { isSuccess = true, Message = "Kayıt Başarıyla Silindi" });
        }

    }
}
