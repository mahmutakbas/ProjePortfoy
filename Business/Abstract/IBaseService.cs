using Business.Utilities.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IBaseService<T>
    {
        Task<IDataResult<int>> AddAsync(T entity);
        Task<IResult> Update(T entity);
        Task<IResult> Delete(int id);  
        Task<IDataResult<T>> Get(int id);
        Task<IDataResult<List<T>>> GetAll();
    }
}
