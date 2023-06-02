using AutoMapper;
using Business.Concrete;
using DataAccess.Dapper;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PortfoyProje.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class RiskController : ControllerBase
    {
        private readonly IRiskService _riskService;
        private readonly IMapper _mapper;
        public RiskController(IRiskService riskService, IMapper mapper)
        {
            _riskService = riskService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _riskService.GetAll();

            if (result.Data.Count == 0)
                return Ok(null);

            var resultDto = _mapper.Map<List<Risk>, List<RiskDto>>(result.Data);

            return Ok(resultDto);
        }

        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id < 1)
                return BadRequest("Item Not Found");

            var result = await _riskService.Get(id);

            if (result.Data == null)
                return NotFound(result);

            var resultDto = _mapper.Map<Risk, RiskDto>(result.Data);

            return Ok(resultDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RiskDto riskDto)
        {

            var risk = _mapper.Map<RiskDto, Risk>(riskDto);
            var result = await _riskService.AddAsync(risk);

            if (!result.Success)
                return BadRequest(new { isSuccess = false, Message = "Kayıt Başarısız" });
            return Ok(new { isSuccess = true, Message = "Kayıt Başarılı" });
        }

        [HttpPut]
        public async Task<IActionResult> Update(RiskDto riskDto)
        {

            var risk = _mapper.Map<RiskDto, Risk>(riskDto);

            var result = await _riskService.Update(risk);

            if (!result.Success)
                return BadRequest(new { isSuccess = false, Message = result.Message });
            return Ok(new { isSuccess = true, Message = "Kayıt Başarıyla Güncellendi" });
        }

        [HttpDelete]
        public async Task<IActionResult> Delte(int id)
        {
            var result = await _riskService.Delete(id);

            if (!result.Success)
                return BadRequest(new { isSuccess = false, Message = "Kayıt Silme Başarısız" });
            return Ok(new { isSuccess = true, Message = "Kayıt Başarıyla Silindi" });
        }
    }
}
