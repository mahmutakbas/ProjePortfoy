using Business.Abstract;
using Business.Utilities.Result;
using Business.Utilities.ValidationRules;
using Business.Utilities.ValidationRules.FluentValidation;
using DataAccess.Dapper;
using Entities.Concrete;

namespace Business.Concrete
{

    public interface IGorevService : IBaseService<Gorev>
    {

    }

    public class GorevManager : IGorevService
    {
        private readonly IGorevDal _gorevDal;

        public GorevManager(IGorevDal gorevDal)
        {
            _gorevDal = gorevDal;
        }

        public async Task<IDataResult<int>> AddAsync(Gorev entity)
        {
            if (entity != null)
            {
                var errorMessages = ValidationTool.Validate(new GorevValidator(), entity);

                if (!string.IsNullOrEmpty(errorMessages))
                {
                    return new DataResult<int>(0, false, errorMessages);
                }

                var result = await _gorevDal.Add(entity);

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

            var result = await _gorevDal.Delete(id);

            return new Result(true, "Success");
        }

        public async Task<IDataResult<Gorev>> Get(int id)
        {
            if (id <= 0)
                return new DataResult<Gorev>(new Gorev(), false, "Error");

            var result = await _gorevDal.Get(id);

            return new DataResult<Gorev>(result, true, "Success");
        }

        public async Task<IDataResult<List<Gorev>>> GetAll()
        {
            var result = await _gorevDal.GetAll();

            return new DataResult<List<Gorev>>(result.ToList(), true);
        }

        public async Task<IResult> Update(Gorev entity)
        {
            if (entity != null)
            {
                var errorMessages = ValidationTool.Validate(new GorevValidator(), entity);

                if (!string.IsNullOrEmpty(errorMessages))
                {
                    return new DataResult<int>(0, false, errorMessages);
                }

                var isTrue = await _gorevDal.Get(entity.Id);

                if (isTrue == null)
                    return new DataResult<int>(0, false, "Görev bulunamadı.");

                var result = await _gorevDal.Update(entity);

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
