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

        public PredictionController(ISelectPortfoy selectPortfoy)
        {
            _selectPortfoy = selectPortfoy;
        }

        [HttpPost]
        public async Task<IActionResult> Predict(IEnumerable<ProjectGetData> data)
        {
            var result = await _selectPortfoy.MLPredictionTest(data);

            List<object> newObje = new List<object>();

            foreach (var item in result)
            {
                newObje.Add(new { Id = item.Id, Name = item.ProjeIsmi, Puan = item.Sonuc });

            }

            if (result == null)
                return BadRequest(new { isSuccess = false, Message = "Tahmin Yaparken hata meydana geldi" });
            return Ok(new { isSuccess = false, Message = "Tahmin Başarılı", data = newObje });
        }
    }
}
