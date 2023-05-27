using AutoMapper;
using Business.Concrete;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PortfoyProje.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class KPIController : ControllerBase
    {
        private readonly IKPIService _kPIService;
        private readonly IMapper _mapper;

        public KPIController(IKPIService kPIService,IMapper mapper)
        {
            _kPIService = kPIService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _kPIService.GetAll();

          var resultDto =   _mapper.Map<List<KPI>, List<KPIDto>>(result.Data);

            return Ok(resultDto);
        }

        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id < 1)
                return BadRequest("Item Not Found");

            var result = await _kPIService.Get(id);

            if (result.Data == null)
                return NotFound(result);

            var resultDto = _mapper.Map<KPI, KPIDto>(result.Data);

            return Ok(resultDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(KPIDto kPI)
        {
            var kPIDto = _mapper.Map<KPIDto, KPI>(kPI);

            var result = await _kPIService.AddAsync(kPIDto);

            if (!result.Success)
                return BadRequest(new { isSuccess = false, Message = "Kayıt Başarısız" });
            return Ok(new { isSuccess = true, Message = "Kayıt Başarılı" });
        }

        [HttpPut]
        public async Task<IActionResult> Update(KPIDto kPI)
        {
            var kPIDto = _mapper.Map<KPIDto, KPI>(kPI);

            var result = await _kPIService.Update(kPIDto);

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
