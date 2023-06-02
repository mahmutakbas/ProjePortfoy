using AutoMapper;
using Business.Concrete;
using DataAccess.Dapper;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PortfoyProje.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class ProjeDetayController : ControllerBase
    {
        private readonly IProjeDetayService _projeDetayService;
        private readonly IMapper _mapper;

        public ProjeDetayController(IProjeDetayService projeDetayService,IMapper mapper)
        {
            _projeDetayService = projeDetayService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _projeDetayService.GetAll();

            if (result.Data.Count == 0)
                return Ok(null);

            var projeDetay = _mapper.Map<List<ProjeDetay>, List<ProjeDetayDto>>(result.Data);
            return Ok(projeDetay);
        }

        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id < 1)
                return BadRequest("Item Not Found");

            var result = await _projeDetayService.Get(id);

            if (result.Data == null)
                return NotFound(result);


            var projeDetay = _mapper.Map<ProjeDetay, ProjeDetayDto>(result.Data);

            return Ok(projeDetay);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProjeDetayDto projeDetayDto)
        {
            var projeDetay = _mapper.Map<ProjeDetayDto, ProjeDetay>(projeDetayDto); 

            var result = await _projeDetayService.AddAsync(projeDetay);

            if (!result.Success)
                return BadRequest(new { isSuccess = false, Message = "Kayıt Başarısız" });
            return Ok(new { isSuccess = true, Message = "Kayıt Başarılı" });
        }

        [HttpPut]
        public async Task<IActionResult> Update(ProjeDetayDto projeDetayDto)
        {
            var projeDetay = _mapper.Map<ProjeDetayDto, ProjeDetay>(projeDetayDto);

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
