using AutoMapper;
using Business.Concrete;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PortfoyProje.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class DepartmanController : ControllerBase
    {
        private readonly IDepartmanService _departmanService;
        private readonly IMapper _mapper;

        public DepartmanController(IDepartmanService departmanService, IMapper mapper)
        {
            _departmanService = departmanService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _departmanService.GetAll();

            var resultDto = _mapper.Map<List<Departman>, List<DepartmanDTO>>(result.Data);

            return Ok(resultDto);
        }

        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id < 1)
                return BadRequest("Item Not Found");

            var result = await _departmanService.Get(id);

            if (result.Data == null)
                return NotFound(result);

            var resultDto = _mapper.Map<Departman, DepartmanDTO>(result.Data);

            return Ok(resultDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(DepartmanDTO departman)
        {
            var resultDto = _mapper.Map<DepartmanDTO,Departman>(departman);

            var result = await _departmanService.AddAsync(resultDto);

            if (!result.Success)
                return BadRequest(new { isSuccess = false, Message = result.Message });
            return Ok(new { isSuccess = true, Message = "Kayıt Başarılı" });
        }

        [HttpPut]
        public async Task<IActionResult> Update(DepartmanDTO departman)
        {
            var resultDto = _mapper.Map<DepartmanDTO, Departman>(departman);

            var result = await _departmanService.Update(resultDto);

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
