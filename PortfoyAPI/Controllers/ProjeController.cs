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


            var resultDetail = await _projeDetayService.GetByProjectId(id);

            var resultSource = await _projeKaynakService.GetByProjectId(id);
            var resultKpi = await _projeKPIService.GetByProjectId(id);
            var resultRisk = await _riskService.GetByProjectId(id);
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
                ResourcePercent = result.Data.KaynakYuzdesi,
                StartDate = result.Data.BaslangicTarihi,
                Status = result.Data.ProjeDurum
            };

            proje.risks = _mapper.Map<List<Risk>, List<RiskDto>>(resultRisk.Data).ToArray();
            proje.Subtasks = _mapper.Map<List<ProjeDetay>, List<ProjeDetayDto>>(resultDetail.Data).ToArray();

            proje.DepartmentId = (resultDepartment.Data);

            if (resultSource.Data.Count > 0)
            {
                proje.Resource = new KaynakDto[resultSource.Data.Count];

                var data = resultSource.Data;
                for (int i = 0; i < data.Count; i++)
                {
                    var kaynak = await _kaynakService.Get(data[i].KaynakId);
                    KaynakDto kaynakDto = new KaynakDto();
                    kaynakDto.Name = kaynak.Data.KaynakAdi;
                    kaynakDto.Id = data[i].KaynakId;
                    kaynakDto.DepartmentId = kaynak.Data.DepartmanId;
                    kaynakDto.Item = data[i].KaynakMiktari;
                    kaynakDto.DepartmentName = (await _departmanService.Get(kaynak.Data.DepartmanId))?.Data?.DepartmanAdi;

                    proje.Resource[i] = kaynakDto;
                }
            }

            if (resultKpi.Data.Count > 0)
            {
                proje.Kpis = new KPIDto[resultKpi.Data.Count];

                var data = resultKpi.Data;
                for (int i = 0; i < data.Count; i++)
                {
                    KPIDto kPI =new KPIDto();

                    var kpi = await _projeKPIService.Get(data[i].Id);
                    kPI.Id = data[i].Id;
                    kPI.Name = data[i].Name;
                    kPI.Goal = kpi.Data.Goal;
                    proje.Kpis[i] = kPI;
                }
            }

            if (resultCategory.Data != null)
            {
                var data = resultCategory.Data;
                
                proje.Type = (await _projeKategoriService.Get(data.Id))?.Data;
            }

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

            var resultDto = _mapper.Map<List<ProjeKPI>, List<ProjeKPIDto>>(result.Data);

            return Ok(resultDto);
        }
        [HttpGet("getsource/{id}")]
        public async Task<IActionResult> GetSourceByProjectId(int id)
        {
            var result = await _projeKaynakService.GetByProjectId(id);

            if (result.Data.Count == 0)
                return Ok(null);

            var resultDto = _mapper.Map<List<ProjeKaynak>, List<ProjeKaynakDto>>(result.Data);

            return Ok(resultDto);
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
