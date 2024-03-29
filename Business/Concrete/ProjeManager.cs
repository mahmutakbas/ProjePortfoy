﻿using Business.Abstract;
using Business.Utilities.Result;
using Business.Utilities.ValidationRules;
using Business.Utilities.ValidationRules.FluentValidation;
using DataAccess.Dapper;
using Entities.Concrete;
using Entities.DTOs;

namespace Business.Concrete
{
    public interface IProjeService : IBaseService<Proje>
    {
        Task<IResult> UpdateStatu(ProjeStatus entity);
        Task<IDataResult<List<object>>> GetAllDto();
    }
    public class ProjeManager : IProjeService
    {
        private readonly IProjeDal _projeDal;
        private readonly IProjeKaynakDal _projeKaynakDal;
        private readonly IKaynakDal _kaynakDal;

        public ProjeManager(IProjeDal projeDal, IProjeKaynakDal projeKaynakDal, IKaynakDal kaynakDal)
        {
            _projeDal = projeDal;
            _projeKaynakDal = projeKaynakDal;
            _kaynakDal = kaynakDal;
        }

        public async Task<IDataResult<int>> AddAsync(Proje entity)
        {
            if (entity != null)
            {
                var errorMessages = ValidationTool.Validate(new ProjeValidator(), entity);

                if (!string.IsNullOrEmpty(errorMessages))
                {
                    return new DataResult<int>(0, false, errorMessages);
                }
                entity.Id = 0;
                var isExist = await _projeDal.IsExist(entity);

                if (isExist)
                    return new DataResult<int>(0, false, "Proje Adı mevcut. Proje adını değiştirin!");

                var result = await _projeDal.Add(entity);

                if (result < 1)
                {
                    return new DataResult<int>(0, false, "Kayıt Yapılamadı");
                }

                return new DataResult<int>(result, true, "Success");
            }
            else
            {
                return new DataResult<int>(0, false, "Entity is Null");
            }
        }

        public async Task<IResult> Delete(int id)
        {
            if (id <= 0)
                return new Result(false, "Error");

            var result = await _projeDal.Delete(id);

            return new Result(true, "Success");
        }

        public async Task<IDataResult<Proje>> Get(int id)
        {
            if (id <= 0)
                return new DataResult<Proje>(new Proje(), false, "Error");

            var result = await _projeDal.Get(id);

            return new DataResult<Proje>(result, true, "Success");

        }

        public Task<IDataResult<List<Proje>>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<IDataResult<List<object>>> GetAllDto()
        {
            var result = await _projeDal.GetAllDto();

            return new DataResult<List<object>>(result.ToList(), true);
        }

        public async Task<IResult> Update(Proje entity)
        {
            if (entity != null)
            {
                var errorMessages = ValidationTool.Validate(new ProjeValidator(), entity);

                if (!string.IsNullOrEmpty(errorMessages))
                {
                    return new DataResult<int>(0, false, errorMessages);
                }

                var isTrue = await _projeDal.Get(entity.Id);

                if (isTrue == null)
                    return new DataResult<int>(0, false, "Proje bulunamadı.");

                var isExist = await _projeDal.IsExist(entity);

                if (isExist)
                    return new DataResult<int>(0, false, "Proje Adı mevcut. Proje adını değiştirin!");

                var result = await _projeDal.Update(entity);

                if (result < 1)
                {
                    return new DataResult<int>(0, false, "Error");
                }

                return new DataResult<int>(result, true, "Success");
            }
            else
            {
                return new DataResult<int>(0, false, "Entity is Null");
            }
        }

        public async Task<IResult> UpdateStatu(ProjeStatus entity)
        {
            if (entity != null)
            {
                var isTrue = await _projeDal.Get(entity.Id);

                if (isTrue == null)
                    return new DataResult<int>(0, false, "Proje bulunamadı.");


                if (entity.Status.ToLower().Equals("tamamlandı"))
                {
                    var getResources = await _projeKaynakDal.GetByProjectIdDto(entity.Id);
                    //pk.id, pk.projeid, k.KaynakAdi, pk.kaynakmiktari

                    if (getResources.Count() > 0)
                    {
                        foreach (var item in getResources.ToList())
                        {


                            Kaynak getKaynak = await _kaynakDal.Get(item.KaynakId);

                            if (getKaynak != null)
                            {
                                getKaynak.KaynakMiktari = getKaynak.KaynakMiktari + Convert.ToInt32(item.KaynakMiktari);

                                await _kaynakDal.Update(getKaynak);

                                ProjeKaynak projeKaynak = new ProjeKaynak()
                                {
                                    Id = Convert.ToInt32(item.Id),
                                    KaynakId = Convert.ToInt32(item.KaynakId),
                                    KaynakMiktari = 0,
                                    ProjeId = entity.Id
                                };

                                await _projeKaynakDal.Update(projeKaynak);
                            }
                        }
                    }
                }


                var result = await _projeDal.UpdateStatu(entity);

                if (result < 1)
                {
                    return new DataResult<int>(0, false, "Error");
                }

                return new DataResult<int>(result, true, "Success");
            }
            else
            {
                return new DataResult<int>(0, false, "Entity is Null");
            }
        }
    }
}
