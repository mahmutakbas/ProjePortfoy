using Business.Abstract;
using Business.Utilities.Result;
using Business.Utilities.ValidationRules;
using Business.Utilities.ValidationRules.FluentValidation;
using DataAccess.Dapper;
using Entities.Concrete;

namespace Business.Concrete
{
    public interface IKaynakService : IBaseService<Kaynak>
    {

    }
    public class KaynakManager : IKaynakService
    {

        private readonly IKaynakDal _kaynakDal;
        private readonly IProjeKaynakDal _projeKaynakDal;

        public KaynakManager(IKaynakDal kaynakDal, IProjeKaynakDal projeKaynakDal)
        {
            _kaynakDal = kaynakDal;
            _projeKaynakDal = projeKaynakDal;
        }

        public async Task<IDataResult<int>> AddAsync(Kaynak entity)
        {
            if (entity != null)
            {
                var errorMessages = ValidationTool.Validate(new KaynakValidator(), entity);

                if (!string.IsNullOrEmpty(errorMessages))
                {
                    return new DataResult<int>(0, false, errorMessages);
                }
                entity.Id = 0;
                var isExist = await _kaynakDal.IsExist(entity);

                if (isExist)
                    return new DataResult<int>(0, false, "Kaynak Adı mevcut. Kaynak adını değiştirin!");

                var result = await _kaynakDal.Add(entity);

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


            var isExist = await _projeKaynakDal.IsUse(id);

            if (isExist)
            {
                return new Result(false, "Kaynak kullanıldığı için silinemez.");
            }

            var result = await _kaynakDal.Delete(id);

            return new Result(true, "Success");
        }

        public async Task<IDataResult<Kaynak>> Get(int id)
        {
            if (id <= 0)
                return new DataResult<Kaynak>(new Kaynak(), false, "Error");

            var result = await _kaynakDal.Get(id);

            return new DataResult<Kaynak>(result, true, "Success");
        }

        public async Task<IDataResult<List<Kaynak>>> GetAll()
        {
            var result = await _kaynakDal.GetAll();

            return new DataResult<List<Kaynak>>(result.ToList(), true);
        }

        public async Task<IResult> Update(Kaynak entity)
        {
            if (entity != null)
            {
                var errorMessages = ValidationTool.Validate(new KaynakValidator(), entity);

                if (!string.IsNullOrEmpty(errorMessages))
                {
                    return new DataResult<int>(0, false, errorMessages);
                }

                var isTrue = await _kaynakDal.Get(entity.Id);

                if (isTrue == null)
                    return new DataResult<int>(0, false, "Kaynak bulunamadı.");

                var isExist = await _kaynakDal.IsExist(entity);

                if (isExist)
                    return new DataResult<int>(0, false, "Kaynak Adı mevcut. Proje adını değiştirin!");

                var result = await _kaynakDal.Update(entity);

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
