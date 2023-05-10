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
    public interface IKPIService : IBaseService<KPI>
    {

    }

    public class KPIManager : IKPIService
    {
        private readonly IKPIDal _KPIDal;

        public KPIManager(IKPIDal kPIDal)
        {
            _KPIDal = kPIDal;
        }

        public async Task<IDataResult<int>> AddAsync(KPI entity)
        {
            if (entity != null)
            {
                var errorMessages = ValidationTool.Validate(new KPIValidator(), entity);

                if (!string.IsNullOrEmpty(errorMessages))
                {
                    return new DataResult<int>(0, false, errorMessages);
                }
                entity.Id = 0;
                var isExist = await _KPIDal.IsExist(entity);

                if (isExist)
                    return new DataResult<int>(0, false, "KPI  mevcut. KPI adını değiştirin!");

                var result = await _KPIDal.Add(entity);

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

            var result = await _KPIDal.Delete(id);

            return new Result(true, "Success");
        }

        public async Task<IDataResult<KPI>> Get(int id)
        {
            if (id <= 0)
                return new DataResult<KPI>(new KPI(), false, "Error");

            var result = await _KPIDal.Get(id);

            return new DataResult<KPI>(result, true, "Success");
        }

        public async Task<IDataResult<List<KPI>>> GetAll()
        {
            var result = await _KPIDal.GetAll();

            return new DataResult<List<KPI>>(result.ToList(), true);
        }

        public async Task<IResult> Update(KPI entity)
        {
            if (entity != null)
            {
                var errorMessages = ValidationTool.Validate(new KPIValidator(), entity);

                if (!string.IsNullOrEmpty(errorMessages))
                {
                    return new DataResult<int>(0, false, errorMessages);
                }

                var isTrue = await _KPIDal.Get(entity.Id);

                if (isTrue == null)
                    return new DataResult<int>(0, false, "KPI bulunamadı.");

                var isExist = await _KPIDal.IsExist(entity);

                if (isExist)
                    return new DataResult<int>(0, false, "KPI mevcut. KPI adını değiştirin!");

                var result = await _KPIDal.Update(entity);

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
