using Business.Abstract;
using Business.Utilities.Result;
using Business.Utilities.ValidationRules;
using Business.Utilities.ValidationRules.FluentValidation;
using DataAccess.Dapper;
using Entities.Concrete;

namespace Business.Concrete
{
    public interface IProjeKPIService : IBaseService<ProjeKPI>
    {
        Task<IDataResult<List<ProjeKPI>>> GetByProjectId(int id);
    }
    public class ProjeKPIManager : IProjeKPIService
    {
        private readonly IProjeKPIDal _projeKPIDal;
        private readonly IProjeDal _projeDal;

        public ProjeKPIManager(IProjeKPIDal projeKPIDal, IProjeDal projeDal)
        {
            _projeKPIDal = projeKPIDal;
            _projeDal = projeDal;
        }

        public async Task<IDataResult<int>> AddAsync(ProjeKPI entity)
        {
            if (entity != null)
            {

                var isTrue = await _projeDal.Get(entity.ProjeId);

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

        public async Task<IDataResult<List<ProjeKPI>>> GetByProjectId(int id)
        {
            var result = await _projeKPIDal.GetByProjectId(id);

            return new DataResult<List<ProjeKPI>>(result.ToList(), true);
        }

        public async Task<IResult> Update(ProjeKPI entity)
        {
            if (entity != null)
            {
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
