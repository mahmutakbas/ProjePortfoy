using Dapper;
using Entities.Concrete;
using MySql.Data.MySqlClient;
using static Dapper.SqlMapper;

namespace DataAccess.Dapper
{
    public interface IProjeKaynakDal : IBaseRepository<ProjeKaynak>
    {
        Task<bool> IsExist(ProjeKaynak entity);
        Task<bool> IsUse(int kaynakId);
        Task<IEnumerable<object>> GetByProjectId(int projeId);
        Task<object> GetFinishTimeProject(ProjeKaynak kaynak);
        Task<IEnumerable<ProjeKaynak>> GetByProjectIdDto(int projeId);

    }
    public class ProjeKaynakDto : IProjeKaynakDal
    {
        public async Task<int> Add(ProjeKaynak entity)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.ExecuteAsync("INSERT INTO ProjeKaynak (ProjeId, KaynakId, KaynakMiktari) VALUES (@ProjeId, @KaynakId, @KaynakMiktari)", new { ProjeId = entity.ProjeId, KaynakId = entity.KaynakId, KaynakMiktari = entity.KaynakMiktari });

                return result;
            }
        }

        public async Task<int> Delete(int id)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.ExecuteAsync("DELETE FROM ProjeKaynak WHERE ID = @Id"
                    , new { Id = id });
                return result;
            }
        }

        public async Task<ProjeKaynak> Get(int id)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.QueryFirstOrDefaultAsync<ProjeKaynak>("SELECT * FROM ProjeKaynak WHERE ID = @Id"
                    , new { Id = id });
                return result;
            }
        }

        public async Task<IEnumerable<ProjeKaynak>> GetAll()
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.QueryAsync<ProjeKaynak>("SELECT * FROM ProjeKaynak");
                return result;
            }
        }

        public async Task<IEnumerable<object>> GetByProjectId(int projeId)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.QueryAsync<object>("SELECT pk.id, pk.projeid,k.Id AS KaynakId, k.KaynakAdi, pk.kaynakmiktari FROM ProjeKaynak pk  INNER  JOIN Kaynaks k ON pk.KaynakId=k.Id WHERE ProjeId = @ProjeId"
                    , new { ProjeId = projeId });
                return result;
            }
        }

        public async Task<IEnumerable<ProjeKaynak>> GetByProjectIdDto(int projeId)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.QueryAsync<ProjeKaynak>("SELECT pk.id AS Id, pk.projeid AS ProjeId,k.Id AS KaynakId,  pk.kaynakmiktari AS KaynakMiktari FROM ProjeKaynak pk  INNER  JOIN Kaynaks k ON pk.KaynakId=k.Id WHERE ProjeId = @ProjeId"
                    , new { ProjeId = projeId });
                return result;
            }
        }

        public async Task<object> GetFinishTimeProject(ProjeKaynak kaynak)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.QueryFirstOrDefaultAsync<object>(@"SELECT p.ProjeAdi AS ProjeAdi, p.BitisTarihi AS ProjeBitisTarihi
                                                                    FROM ProjeKaynak pk
                                                                             INNER JOIN Projes p ON pk.ProjeId = p.Id
                                                                    WHERE pk.KaynakId = @KaynakId
                                                                    GROUP BY p.ProjeAdi, p.BitisTarihi
                                                                    HAVING SUM(pk.KaynakMiktari) >= @Istenilenkaynak
                                                                       AND p.BitisTarihi < (SELECT BaslangicTarihi FROM Projes WHERE Id = @ProjeId) ORDER BY p.BitisTarihi ASC LIMIT 1;", new { ProjeId = kaynak.ProjeId, KaynakId = kaynak.KaynakId, Istenilenkaynak = kaynak.KaynakMiktari });
                return result;
            }
        }



        public async Task<bool> IsExist(ProjeKaynak entity)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.QueryFirstOrDefaultAsync<Kaynak>("SELECT * FROM ProjeKaynak WHERE KaynakId=@KaynakId  && ProjeId=@ProjeId AND ID <> @Id", new { KaynakId = entity.KaynakId, ProjeId = entity.ProjeId, Id = entity.Id });

                return result == null ? false : true;
            }
        }

        public async Task<bool> IsUse(int kaynakId)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.QueryFirstOrDefaultAsync<Kaynak>("SELECT * FROM ProjeKaynak WHERE KaynakId=@KaynakId LIMIT 1", new { KaynakId = kaynakId });
                return result == null ? false : true;
            }
        }

        public async Task<int> Update(ProjeKaynak entity)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.ExecuteAsync("UPDATE ProjeKaynak Set ProjeId = @ProjeId, KaynakId=@KaynakId, KaynakMiktari=@KaynakMiktari WHERE Id=@Id", new { Id = entity.Id, ProjeId = entity.ProjeId, KaynakId = entity.KaynakId, KaynakMiktari = entity.KaynakMiktari });
                return result;
            }
        }


    }
}
