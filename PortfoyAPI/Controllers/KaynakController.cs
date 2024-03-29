﻿using AutoMapper;
using Business.Concrete;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace PortfoyProje.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class KaynakController : ControllerBase
    {
        private readonly IKaynakService _kaynakService;
        private readonly IMapper _mapper;
        public KaynakController(IKaynakService kaynakService, IMapper mapper)
        {
            _kaynakService = kaynakService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _kaynakService.GetAll();

            if (result.Data.Count == 0)
                return Ok(null);

            var resultDto = _mapper.Map<List<Kaynak>, List<KaynakDto>>(result.Data);

            return Ok(resultDto);
        }

        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id < 1)
                return BadRequest("Item Not Found");

            var result = await _kaynakService.Get(id);

            if (result.Data == null)
                return NotFound(result);

            var resultDto = _mapper.Map<Kaynak, KaynakDto>(result.Data);

            return Ok(resultDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(KaynakDto kaynak)
        {
            var kaynkaDto = _mapper.Map<KaynakDto, Kaynak>(kaynak);

            var result = await _kaynakService.AddAsync(kaynkaDto);

            if (!result.Success)
                return BadRequest(new { isSuccess = false, Message = result.Message });

            return Ok(new { isSuccess = true, Message = "Kayıt Başarılı" });
        }

        [HttpPut]
        public async Task<IActionResult> Update(KaynakDto kaynak)
        {
            var kaynkaDto = _mapper.Map<KaynakDto, Kaynak>(kaynak);

            var result = await _kaynakService.Update(kaynkaDto);

            if (!result.Success)
                return BadRequest(new { isSuccess = false, Message = result.Message });
            return Ok(new { isSuccess = true, Message = "Kayıt Başarıyla Güncellendi" });
        }

        [HttpDelete]
        public async Task<IActionResult> Delte(int id)
        {
            var result = await _kaynakService.Delete(id);

            if (!result.Success)
                return BadRequest(new { isSuccess = false, Message = "Kayıt Silme Başarısız" });
            return Ok(new { isSuccess = true, Message = "Kayıt Başarıyla Silindi" });
        }
    }
}
