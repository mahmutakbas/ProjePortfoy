using Dapper;
using Entities.Concrete;
using MySql.Data.MySqlClient;

namespace DataAccess.Dapper
{
    public interface IKPIDal : IBaseRepository<KPI>
    {
        Task<bool> IsExist(KPI entity);

    }
    public class KPIDal : IKPIDal
    {
        public async Task<int> Add(KPI entity)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.ExecuteAsync("INSERT INTO KPIs ( KPIAdi, Aciklama, Puan) VALUES (@KPIAdi, @Aciklama, @Puan);", new { KPIAdi = entity.KPIAdi, Aciklama = entity.Aciklama, Puan = entity.Puan });
                return result;
            }
        }

        public async Task<int> Delete(int id)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.ExecuteAsync("DELETE FROM KPIs WHERE ID = @Id", new { Id = id });
                return result;
            }
        }

        public async Task<KPI> Get(int id)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.QueryFirstOrDefaultAsync<KPI>("SELECT * FROM KPIs WHERE ID = @Id", new { Id = id });
                return result;
            }
        }

        public async Task<IEnumerable<KPI>> GetAll()
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.QueryAsync<KPI>("SELECT * FROM KPIs");
                return result;
            }
        }

        public async Task<bool> IsExist(KPI entity)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.QueryFirstOrDefaultAsync<KPI>("SELECT * FROM KPIs WHERE KPIAdi=@KPIAdi AND ID <> @Id", new { KPIAdi = entity.KPIAdi, Id = entity.Id });

                return result == null ? false : true;
            }
        }

        public async Task<int> Update(KPI entity)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.ExecuteAsync("UPDATE KPIs SET KPIAdi=@KPIAdi, Aciklama=@Aciklama, Puan=@Puan WHERE ID = @Id;", new { Id = entity.Id, KPIAdi = entity.KPIAdi, Aciklama = entity.Aciklama, Puan = entity.Puan });
                return result;
            }
        }
    }
}
