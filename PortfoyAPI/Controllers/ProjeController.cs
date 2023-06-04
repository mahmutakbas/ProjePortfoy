using AutoMapper;
using Business.Concrete;
using DataAccess.Dapper;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Mozilla;

namespace WebPortfoy.Controllers
{

    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class ProjeController : ControllerBase
    {
        private readonly IProjeService _projeService;
        private readonly IRiskService _riskService;
        private readonly IProjeDetayService _projeDetayService;
        private readonly IProjeKPIService _projeKPIService;
        private readonly IProjeKaynakService _projeKaynakService;
        private readonly IProjeKategoriService _projeKategoriService;
        private readonly IKaynakService _kaynakService;
        private readonly IDepartmanService _departmanService;
        private readonly IMapper _mapper;

        public ProjeController(IProjeService projeService, IRiskService riskService, IProjeDetayService projeDetayService, IProjeKPIService projeKPIService, IProjeKaynakService projeKaynakService, IMapper mapper, IProjeKategoriService projeKategoriService, IKaynakService kaynakService,IDepartmanService departmanService)  
        {
            _projeService = projeService;
            _riskService = riskService;
            _projeDetayService = projeDetayService;
            _projeKPIService = projeKPIService;
            _projeKaynakService = projeKaynakService;
            _mapper = mapper;
            _projeKategoriService = projeKategoriService;
            _kaynakService = kaynakService;
            _departmanService = departmanService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _projeService.GetAllDto();

            

            return Ok(result.Data);
        }

        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id < 1)
                return BadRequest("Item Not Found");

            var result = await _projeService.Get(id);

            if (result.Data == null)
                return NotFound(result);
            
            var resultCategory = await _projeKategoriService.Get(result.Data.ProjeKategoriId);
            var resultDepartment = await _departmanService.Get(result.Data.DepartmanId);

            ProjeDDto proje = new ProjeDDto
            {
                Id = result.Data.Id,
                Name = result.Data.ProjeAdi,
                Budget = result.Data.ProjeButcesi,
                Customer = result.Data.ProjeMusteri,
                Description = result.Data.ProjeAciklama,
                FinishDate = result.Data.BitisTarihi,
                manCount = result.Data.IsciSayisi,
                Revenue = result.Data.ProjeGeliri,
                ResourcePercent = result.Data.KaynakYuzdesi,
                StartDate = result.Data.BaslangicTarihi,
                Status = result.Data.ProjeDurum
            };

            proje.DepartmentId = (resultDepartment.Data);

            proje.Type = resultCategory.Data;
            return Ok(proje);
        }

        [HttpGet("getdetail/{id}")]
        public async Task<IActionResult> GetDetailByProjectId(int id)
        {
            var result = await _projeDetayService.GetByProjectId(id);

            if (result.Data.Count == 0)
                return Ok(null);

            var resultDto = _mapper.Map<List<ProjeDetay>, List<ProjeDetayDto>>(result.Data);

            return Ok(resultDto);
        }

        [HttpGet("getkpi/{id}")]
        public async Task<IActionResult> GetKPIByProjectId(int id)
        {
            var result = await _projeKPIService.GetByProjectId(id);

            if (result.Data.Count == 0)
                return Ok(null);

            return Ok(result.Data);
        }
        [HttpGet("getsource/{id}")]
        public async Task<IActionResult> GetSourceByProjectId(int id)
        {
            var result = await _projeKaynakService.GetByProjectId(id);

            if (result.Data.Count == 0)
                return Ok(null);

            return Ok(result.Data);
        }
        [HttpGet("getrisk/{id}")]
        public async Task<IActionResult> GetRiskByProjectId(int id)
        {
            var result = await _riskService.GetByProjectId(id);

            if (result.Data.Count == 0)
                return Ok(null);

            var resultDto = _mapper.Map<List<Risk>, List<RiskDto>>(result.Data);

            return Ok(resultDto);
        }


        [HttpPost]
        public async Task<IActionResult> Create(ProjectDto proje)
        {

            var resultDto = _mapper.Map<ProjectDto, Proje>(proje);
            var result = await _projeService.AddAsync(resultDto);

            if (!result.Success)
                return BadRequest(new { isSuccess = false, Message = result.Message });
            return Ok(new { isSuccess = true, Message = "Kayıt Başarılı" });
        }

        [HttpPut]
        public async Task<IActionResult> Update(ProjectDto proje)
        {
            var resultDto = _mapper.Map<ProjectDto, Proje>(proje);

            var result = await _projeService.Update(resultDto);

            if (!result.Success)
                return BadRequest(new { isSuccess = false, Message = result.Message });
            return Ok(new { isSuccess = true, Message = "Kayıt Başarıyla Güncellendi" });
        }

        [HttpPut("status")]
        public async Task<IActionResult> UpdateStatus(ProjeStatus proje)
        {
           
       var result = await _projeService.UpdateStatu(proje);

            if (!result.Success)
                return BadRequest(new { isSuccess = false, Message = result.Message });
            return Ok(new { isSuccess = true, Message = "Kayıt Başarıyla Güncellendi" });
        }

        [HttpDelete]
        public async Task<IActionResult> Delte(int id)
        {
            var result = await _projeService.Delete(id);

            if (!result.Success)
                return BadRequest(new { isSuccess = false, Message = "Kayıt Silme Başarısız" });
            return Ok(new { isSuccess = true, Message = "Kayıt Başarıyla Silindi" });
        }
    }
}
