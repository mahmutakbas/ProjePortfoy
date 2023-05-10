using Business.Abstract;
using Business.Utilities.Result;
using Business.Utilities.ValidationRules.FluentValidation;
using Business.Utilities.ValidationRules;
using DataAccess.Dapper;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public interface IProjeKategoriService : IBaseService<ProjeKategori>
    {

    }
    public class ProjeKategoriManager : IProjeKategoriService
    {
        private readonly IProjeKategoriDal _projeKategoriDal;

        public ProjeKategoriManager(IProjeKategoriDal projeKategoriDal)
        {
            _projeKategoriDal = projeKategoriDal;
        }

        public async Task<IDataResult<int>> AddAsync(ProjeKategori entity)
        {
            if (entity != null)
            {
                var errorMessages = ValidationTool.Validate(new ProjeKategoriValidator(), entity);

                if (!string.IsNullOrEmpty(errorMessages))
                {
                    return new DataResult<int>(0, false, errorMessages);
                }
                entity.Id = 0;
                var isExist = await _projeKategoriDal.IsExist(entity);

                if (isExist)
                    return new DataResult<int>(0, false, "Proje Kategori Adı mevcut. Proje Kategori adını değiştirin!");

                var result = await _projeKategoriDal.Add(entity);

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

            var result = await _projeKategoriDal.Delete(id);

            return new Result(true, "Success");
        }

        public async Task<IDataResult<ProjeKategori>> Get(int id)
        {
            if (id <= 0)
                return new DataResult<ProjeKategori>(new ProjeKategori(), false, "Error");

            var result = await _projeKategoriDal.Get(id);

            return new DataResult<ProjeKategori>(result, true, "Success");
        }

        public async Task<IDataResult<List<ProjeKategori>>> GetAll()
        {
            var result = await _projeKategoriDal.GetAll();

            return new DataResult<List<ProjeKategori>>(result.ToList(), true);
        }

        public async Task<IResult> Update(ProjeKategori entity)
        {
            if (entity != null)
            {
                var errorMessages = ValidationTool.Validate(new ProjeKategoriValidator(), entity);

                if (!string.IsNullOrEmpty(errorMessages))
                {
                    return new DataResult<int>(0, false, errorMessages);
                }

                var isTrue = await _projeKategoriDal.Get(entity.Id);

                if (isTrue == null)
                    return new DataResult<int>(0, false, "Proje Kategori bulunamadı.");

                var isExist = await _projeKategoriDal.IsExist(entity);

                if (isExist)
                    return new DataResult<int>(0, false, "Proje Kategori Adı mevcut. Proje Kategori adını değiştirin!");

                var result = await _projeKategoriDal.Update(entity);

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
