using Business.Abstract;
using Business.Utilities.Result;
using Business.Utilities.ValidationRules;
using Business.Utilities.ValidationRules.FluentValidation;
using DataAccess.Dapper;
using Entities.Concrete;

namespace Business.Concrete
{

    public interface IProjeKaynakService : IBaseService<ProjeKaynak>
    {

    }
    public class ProjeKaynakManager : IProjeKaynakService
    {

        private readonly IProjeKaynakDal _projeKaynakDal;
        private readonly IKaynakDal _kaynakDal;

        public ProjeKaynakManager(IProjeKaynakDal projeKaynakDal, IKaynakDal kaynakDal)
        {
            _projeKaynakDal = projeKaynakDal;
            _kaynakDal = kaynakDal;
        }

        public async Task<IDataResult<int>> AddAsync(ProjeKaynak entity)
        {
            if (entity != null)
            {
                var errorMessages = ValidationTool.Validate(new KaynakValidator(), entity);

                if (!string.IsNullOrEmpty(errorMessages))
                {
                    return new DataResult<int>(0, false, errorMessages);
                }
                entity.Id = 0;
                var isExist = await _projeKaynakDal.IsExist(entity);

                if (isExist)
                    return new DataResult<int>(0, false, "Kaynak mevcut. Yeniden ekleyemezsiniz!");

                var result = await _projeKaynakDal.Add(entity);

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

            var result = await _projeKaynakDal.Delete(id);

            return new Result(true, "Success");
        }

        public async Task<IDataResult<ProjeKaynak>> Get(int id)
        {
            if (id <= 0)
                return new DataResult<ProjeKaynak>(new ProjeKaynak(), false, "Error");

            var result = await _projeKaynakDal.Get(id);

            return new DataResult<ProjeKaynak>(result, true, "Success");
        }

        public async Task<IDataResult<List<ProjeKaynak>>> GetAll()
        {
            var result = await _projeKaynakDal.GetAll();

            return new DataResult<List<ProjeKaynak>>(result.ToList(), true);
        }

        public async Task<IResult> Update(ProjeKaynak entity)
        {
            if (entity != null)
            {
                var currentKaynak = await _projeKaynakDal.Get(entity.Id);

                if (currentKaynak == null)
                    return new DataResult<int>(0, false, "Kaynak bulunamadı.");

                var getKaynak = await _kaynakDal.Get(entity.KaynakId);

                if (entity.KaynakMiktari > currentKaynak.KaynakMiktari)
                {
                    if (getKaynak.KaynakMiktari < entity.KaynakMiktari - currentKaynak.KaynakMiktari)
                    {
                        return new DataResult<int>(0, false, "Yeterli kaynak bulunmamaktadır.");
                    }
                    else
                    {
                        getKaynak.KaynakMiktari = (getKaynak.KaynakMiktari + currentKaynak.KaynakMiktari) - entity.KaynakMiktari;
                    }

                }
                else
                {
                    getKaynak.KaynakMiktari += currentKaynak.KaynakMiktari - entity.KaynakMiktari;
                }

                await _kaynakDal.Update(getKaynak);

                var result = await _projeKaynakDal.Update(entity);

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
