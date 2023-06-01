﻿using Dapper;
using Entities.Concrete;
using MySql.Data.MySqlClient;

namespace DataAccess.Dapper
{
    public interface IDepartmentDal : IBaseRepository<Departman>
    {
        Task<bool> IsExist(Departman entity);
      /*
       Riskler çıkartılacak
       Departmanlar tıkladığında o epratmana ait kaynak
       Projenin { Riskler, Departmanlar, Kaynaklar}
       */
    }
    public class DepartmanDal : IDepartmentDal
    {
        public async Task<int> Add(Departman entity)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.ExecuteAsync("INSERT INTO Departmans (DepartmanAdi) VALUES (@DepartmanAdi);", new { DepartmanAdi = entity.DepartmanAdi });
                return result;
            }
        }

        public async Task<int> Delete(int id)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.ExecuteAsync("DELETE FROM Departmans WHERE Id=@Id", new { Id = id });
                return result;
            }
        }

        public async Task<Departman> Get(int id)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.QueryFirstOrDefaultAsync<Departman>("SELECT * FROM Departmans WHERE Id=@Id", new { Id = id });
                return result;
            }
        }

        public async Task<IEnumerable<Departman>> GetAll()
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.QueryAsync<Departman>("SELECT * FROM Departmans");
                return result;
            }
        }

        public async Task<bool> IsExist(Departman entity)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.QueryFirstOrDefaultAsync<Departman>("SELECT * FROM Departmans WHERE DepartmanAdi=@DepartmanAdi AND ID <> @Id", new { DepartmanAdi = entity.DepartmanAdi, Id = entity.Id });

                return result == null ? false : true;
            }
        }

        public async Task<int> Update(Departman entity)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.ExecuteAsync("UPDATE Departmans SET DepartmanAdi = @DepartmanAdi WHERE Id=@Id;", new { DepartmanAdi = entity.DepartmanAdi, Id=entity.Id });
                return result;
            }
        }
    }
}
