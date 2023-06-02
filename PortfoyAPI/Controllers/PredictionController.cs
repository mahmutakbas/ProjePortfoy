using AutoMapper;
using Business.Concrete;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Mvc;
using MLDataAccess;

namespace PortfoyAPI.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class PredictionController : ControllerBase
    {
        private readonly ISelectPortfoy _selectPortfoy;
        private readonly IProjeService _projeService;
        private readonly IMapper _mapper;

        public PredictionController(ISelectPortfoy selectPortfoy, IProjeService projeService,IMapper mapper)
        {
            _selectPortfoy = selectPortfoy;
            _projeService = projeService;
            _mapper=mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Predict(List<ProjectGetData> data)
        {
            var result = await _selectPortfoy.MLPredictionTest(data);

            List<ProjectDto> newObje = new List<ProjectDto>();

            foreach (var item in result)
            {
                var project = await _projeService.Get(int.Parse(item.Id.ToString()));

                var resultDto = _mapper.Map<Proje, ProjectDto>(project.Data);

                newObje.Add(resultDto);
            }

            if (result == null)
                return BadRequest(new { isSuccess = false, Message = "Tahmin Yaparken hata meydana geldi" });
            return Ok(new { isSuccess = false, Message = "Tahmin Başarılı", data = newObje });
        }
    }
}
