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
    public interface IRiskService : IBaseService<Risk>
    {
        Task<IDataResult<List<Risk>>> GetByProjectId(int id);
    }
    public class RiskManager : IRiskService
    {
        private readonly IRiskDal _riskDal;

        public RiskManager(IRiskDal riskDal)
        {
            _riskDal = riskDal;
        }

        public async Task<IDataResult<int>> AddAsync(Risk entity)
        {
            if (entity != null)
            {
                var errorMessages = ValidationTool.Validate(new RiskValidator(), entity);

                if (!string.IsNullOrEmpty(errorMessages))
                {
                    return new DataResult<int>(0, false, errorMessages);
                }

                var result = await _riskDal.Add(entity);

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

            var result = await _riskDal.Delete(id);

            return new Result(true, "Success");
        }

        public async Task<IDataResult<Risk>> Get(int id)
        {
            if (id <= 0)
                return new DataResult<Risk>(new Risk(), false, "Error");

            var result = await _riskDal.Get(id);

            return new DataResult<Risk>(result, true, "Success");
        }

        public async Task<IDataResult<List<Risk>>> GetAll()
        {
            var result = await _riskDal.GetAll();

            return new DataResult<List<Risk>>(result.ToList(), true);
        }

        public async Task<IDataResult<List<Risk>>> GetByProjectId(int id)
        {
            var result = await _riskDal.GetRiskByProjectId(id);

            return new DataResult<List<Risk>>(result.ToList(), true);
        }

        public async Task<IResult> Update(Risk entity)
        {
            if (entity != null)
            {
                var errorMessages = ValidationTool.Validate(new RiskValidator(), entity);

                if (!string.IsNullOrEmpty(errorMessages))
                {
                    return new DataResult<int>(0, false, errorMessages);
                }

                var isTrue = await _riskDal.Get(entity.Id);

                if (isTrue == null)
                    return new DataResult<int>(0, false, "Risk bulunamadı.");

                var result = await _riskDal.Update(entity);

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
