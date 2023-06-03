using Business.Abstract;
using Business.Utilities.Result;
using Business.Utilities.ValidationRules;
using Business.Utilities.ValidationRules.FluentValidation;
using DataAccess.Dapper;
using Entities.Concrete;
using Entities.DTOs;

namespace Business.Concrete
{
    public interface IDepartmanService : IBaseService<Departman>
    {
        Task<IDataResult<List<DepartmanChartDto>>> GetDepartmentChart();
    }
    public class DepartmanManager : IDepartmanService
    {
        private readonly IDepartmentDal _departmentDal;

        public DepartmanManager(IDepartmentDal departmentDal)
        {
            _departmentDal = departmentDal;
        }

        public async Task<IDataResult<int>> AddAsync(Departman entity)
        {
            if (entity != null)
            {
                var errorMessages = ValidationTool.Validate(new DepartmanValidator(), entity);

                if (!string.IsNullOrEmpty(errorMessages))
                {
                    return new DataResult<int>(0, false, errorMessages);
                }
                entity.Id = 0;
                var isExist = await _departmentDal.IsExist(entity);

                if (isExist)
                    return new DataResult<int>(0, false, "Departman mevcut. Departman adını değiştirin!");

                var result = await _departmentDal.Add(entity);

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

            var result = await _departmentDal.Delete(id);

            return new Result(true, "Success");
        }

        public async Task<IDataResult<Departman>> Get(int id)
        {
            if (id <= 0)
                return new DataResult<Departman>(new Departman(), false, "Error");

            var result = await _departmentDal.Get(id);

            return new DataResult<Departman>(result, true, "Success");
        }

        public async Task<IDataResult<List<Departman>>> GetAll()
        {
            var result = await _departmentDal.GetAll();

            return new DataResult<List<Departman>>(result.ToList(), true);
        }

        public async Task<IDataResult<List<DepartmanChartDto>>> GetDepartmentChart()
        {
            var result =await  _departmentDal.GetDepartmentChart();

            return new DataResult<List<DepartmanChartDto>>(result.ToList(), true);
        }

        public async Task<IResult> Update(Departman entity)
        {
            if (entity != null)
            {
                var errorMessages = ValidationTool.Validate(new DepartmanValidator(), entity);

                if (!string.IsNullOrEmpty(errorMessages))
                {
                    return new DataResult<int>(0, false, errorMessages);
                }

                var isTrue = await _departmentDal.Get(entity.Id);

                if (isTrue == null)
                    return new DataResult<int>(0, false, "Deperatman bulunmadı.");

                var isExist = await _departmentDal.IsExist(entity);

                if (isExist)
                    return new DataResult<int>(0, false, "Deperatman Adı mevcut. Deperatman adını değiştirin!");

                var result = await _departmentDal.Update(entity);

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
