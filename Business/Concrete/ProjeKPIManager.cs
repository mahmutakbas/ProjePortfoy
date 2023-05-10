using Business.Abstract;
using Business.Utilities.Result;
using Business.Utilities.ValidationRules;
using Business.Utilities.ValidationRules.FluentValidation;
using DataAccess.Dapper;
using Entities.Concrete;

namespace Business.Concrete
{
    public interface IProjeKPIService : IBaseService<ProjeKPI> { }
    public class ProjeKPIManager : IProjeKPIService
    {
        private readonly IProjeKPIDal _projeKPIDal;

        public ProjeKPIManager(IProjeKPIDal projeKPIDal)
        {
            _projeKPIDal = projeKPIDal;
        }

        public async Task<IDataResult<int>> AddAsync(ProjeKPI entity)
        {
            if (entity != null)
            {
                var errorMessages = ValidationTool.Validate(new ProjeKPIValidator(), entity);

                if (!string.IsNullOrEmpty(errorMessages))
                {
                    return new DataResult<int>(0, false, errorMessages);
                }

                var result = await _projeKPIDal.Add(entity);

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

            var result = await _projeKPIDal.Delete(id);

            return new Result(true, "Success");
        }

        public async Task<IDataResult<ProjeKPI>> Get(int id)
        {
            if (id <= 0)
                return new DataResult<ProjeKPI>(new ProjeKPI(), false, "Error");

            var result = await _projeKPIDal.Get(id);

            return new DataResult<ProjeKPI>(result, true, "Success");
        }

        public async Task<IDataResult<List<ProjeKPI>>> GetAll()
        {
            var result = await _projeKPIDal.GetAll();

            return new DataResult<List<ProjeKPI>>(result.ToList(), true);
        }

        public async Task<IResult> Update(ProjeKPI entity)
        {
            if (entity != null)
            {
                var errorMessages = ValidationTool.Validate(new ProjeKPIValidator(), entity);

                if (!string.IsNullOrEmpty(errorMessages))
                {
                    return new DataResult<int>(0, false, errorMessages);
                }

                var isTrue = await _projeKPIDal.Get(entity.Id);

                if (isTrue == null)
                    return new DataResult<int>(0, false, "Proje KPI bulunamadı.");

                var result = await _projeKPIDal.Update(entity);

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
