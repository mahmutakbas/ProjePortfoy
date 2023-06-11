using Business.Abstract;
using Business.Utilities.Result;
using DataAccess.Dapper;
using Entities.Concrete;

namespace Business.Concrete
{

    public interface IProjeKaynakService 
    {
        Task<IDataResult<object>> AddAsync(ProjeKaynak entity);
        Task<IDataResult<object>> Update(ProjeKaynak entity);
        Task<IResult> Delete(int id);
        Task<IDataResult<ProjeKaynak>> Get(int id);
        Task<IDataResult<List<ProjeKaynak>>> GetAll();
        Task<IDataResult<List<object>>> GetByProjectId(int id);
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

        public async Task<IDataResult<object>> AddAsync(ProjeKaynak entity)
        {
            if (entity != null)
            {
                var exist = await _projeKaynakDal.IsExist(entity);
                if (exist)
                    return new DataResult<object>(null, false, "Bu kaynak daha önceden seçmiş olduğunuz projeye eklendi.");


                var getKaynak = await _kaynakDal.Get(entity.KaynakId);

                if (getKaynak.KaynakMiktari < entity.KaynakMiktari)
                {

                    var getFinishTime = await _projeKaynakDal.GetFinishTimeProject(entity);
                    if (getFinishTime == null)
                    {
                        return new DataResult<object>(null, false, "Yeterli kaynak bulunamadı.");
                    }
                    else
                    {
                        return new DataResult<object>(getFinishTime, false, "Yeterli kaynak bulunamadı.");
                    }
                }

                getKaynak.KaynakMiktari -= entity.KaynakMiktari;

                await _kaynakDal.Update(getKaynak);

                entity.Id = 0;
                var result = await _projeKaynakDal.Add(entity);

                if (result < 1)
                {
                    return new DataResult<object>(0, false, "Kayıt Yapılamadı");
                }

                return new DataResult<object>(result, true, "Success");
            }
            else
            {
                return new DataResult<object>(0, false, "Entity is Null");
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

        public async Task<IDataResult<List<object>>> GetByProjectId(int id)
        {
            var result = await _projeKaynakDal.GetByProjectId(id);

            return new DataResult<List<object>>(result.ToList(), true);
        }

        public async Task<IDataResult<object>> Update(ProjeKaynak entity)
        {
            if (entity != null)
            {
                var currentKaynak = await _projeKaynakDal.Get(entity.Id);

                if (currentKaynak == null)
                    return new DataResult<object>(0, false, "Kaynak bulunamadı.");



                var getKaynak = await _kaynakDal.Get(entity.KaynakId);

                if (entity.KaynakMiktari > currentKaynak.KaynakMiktari)
                {
                    if (getKaynak.KaynakMiktari < entity.KaynakMiktari - currentKaynak.KaynakMiktari)
                    {
                        var getFinishTime = await _projeKaynakDal.GetFinishTimeProject(entity);
                        if (getFinishTime != null)
                        {
                            return new DataResult<object>(null, false, "Yeterli kaynak bulunamadı.");
                        }
                        else
                        {
                            return new DataResult<object>(getFinishTime, false, "Yeterli kaynak bulunamadı.");
                        }
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
                    return new DataResult<object>(0, false, "Error");
                }

                return new DataResult<object>(result, true, "Success");
            }
            else
            {
                return new DataResult<object>(0, false, "Entity is Null");
            }
        }
    }
}
