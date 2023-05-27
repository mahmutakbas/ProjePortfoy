using AutoMapper;
using Business.Concrete;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebPortfoy.Controllers
{

    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class ProjeController : ControllerBase
    {
        private readonly IProjeService _projeService;
        private readonly IMapper _mapper;


        public ProjeController(IProjeService projeService,IMapper mapper)
        {
            _projeService = projeService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _projeService.GetAll();

            var resultDto = _mapper.Map<List<Proje>, List<ProjectDto>>(result.Data);

            return Ok(resultDto);
        }

        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id < 1)
                return BadRequest("Item Not Found");

            var result = await _projeService.Get(id);

            if (result.Data == null)
                return NotFound(result);

            var resultDto = _mapper.Map<Proje, ProjectDto>(result.Data);    

            return Ok(resultDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProjectDto proje)
        {

            var resultDto = _mapper.Map<ProjectDto, Proje>(proje);
            var result = await _projeService.AddAsync(resultDto);

            if (!result.Success)
                return BadRequest(new { isSuccess = false, Message = "Kayıt Başarısız" });
            return Ok(new { isSuccess = true, Message = "Kayıt Başarılı" });
        }

        [HttpPut]
        public async Task<IActionResult> Update(ProjectDto proje)
        {
            var resultDto = _mapper.Map<ProjectDto, Proje>(proje);

            var result = await _projeService.Update(resultDto);

            if(!result.Success)
                return BadRequest(new { isSuccess = false, Message = result.Message });
            return Ok(new { isSuccess = true, Message = "Kayıt Başarıyla Güncellendi" });
        }

        [HttpDelete]
        public async Task<IActionResult> Delte(int id)
        {
            var result = await _projeService.Delete(id);

            if(!result.Success)
                return BadRequest(new { isSuccess = false, Message = "Kayıt Silme Başarısız" });
            return Ok(new { isSuccess = true, Message = "Kayıt Başarıyla Silindi" });
        }
    }
}
