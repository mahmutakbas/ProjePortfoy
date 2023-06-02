using Business.Abstract;
using Business.Utilities.Result;
using Business.Utilities.ValidationRules;
using Business.Utilities.ValidationRules.FluentValidation;
using DataAccess.Dapper;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public interface IProjeDetayService : IBaseService<ProjeDetay>
    {
        Task<IDataResult<List<ProjeDetay>>> GetByProjectId(int id);

    }
    public class ProjeDetayManager : IProjeDetayService
    {
        private readonly IProjeDetayDal _projeDetayDal;

        public ProjeDetayManager(IProjeDetayDal projeDetayDal)
        {
            _projeDetayDal = projeDetayDal;
        }

        public async Task<IDataResult<int>> AddAsync(ProjeDetay entity)
        {
            if (entity != null)
            {
                var errorMessages = ValidationTool.Validate(new ProjeDetayValidator(), entity);

                if (!string.IsNullOrEmpty(errorMessages))
                {
                    return new DataResult<int>(0, false, errorMessages);
                }

                var result = await _projeDetayDal.Add(entity);

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

            var result = await _projeDetayDal.Delete(id);

            return new Result(true, "Success");
        }

        public async Task<IDataResult<ProjeDetay>> Get(int id)
        {
            if (id <= 0)
                return new DataResult<ProjeDetay>(new ProjeDetay(), false, "Error");

            var result = await _projeDetayDal.Get(id);

            return new DataResult<ProjeDetay>(result, true, "Success");
        }

        public async Task<IDataResult<List<ProjeDetay>>> GetAll()
        {
            var result = await _projeDetayDal.GetAll();

            return new DataResult<List<ProjeDetay>>(result.ToList(), true);
        }

        public async Task<IDataResult<List<ProjeDetay>>> GetByProjectId(int id)
        {
            var result = await _projeDetayDal.GetByProjectId(id);

            return new DataResult<List<ProjeDetay>>(result.ToList(), true);
        }

        public async Task<IResult> Update(ProjeDetay entity)
        {
            if (entity != null)
            {
                var errorMessages = ValidationTool.Validate(new ProjeDetayValidator(), entity);

                if (!string.IsNullOrEmpty(errorMessages))
                {
                    return new DataResult<int>(0, false, errorMessages);
                }

                var isTrue = await _projeDetayDal.Get(entity.Id);

                if (isTrue == null)
                    return new DataResult<int>(0, false, "Proje Detayı bulunamadı.");

                var result = await _projeDetayDal.Update(entity);

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
