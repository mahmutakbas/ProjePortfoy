using Business.Abstract;
using Business.Utilities.Result;
using Business.Utilities.ValidationRules;
using Business.Utilities.ValidationRules.FluentValidation;
using DataAccess.Dapper;
using Entities.DTOs.UserDto;

namespace Business.Concrete
{
    public interface IUserService : IBaseService<User>
    {
        Task<User> Login(string username, string password);
        //https://www.c-sharpcorner.com/article/jwt-token-authentication-using-the-net-core-6-web-api/
    }
    public class UserManager : IUserService
    {
        private readonly IUserDal _userDal;

        public UserManager(IUserDal userDal)
        {
            _userDal = userDal;
        }

        public async Task<IDataResult<int>> AddAsync(User entity)
        {
            if (entity is not null)
            {
                var errorMessages = ValidationTool.Validate(new UserValidator(), entity);

                if (!string.IsNullOrEmpty(errorMessages))
                {
                    return new DataResult<int>(0, false, errorMessages);
                }


                var isExist = await _userDal.UserExists(entity?.Username!);

                if (isExist)
                    return new DataResult<int>(0, false, "User mevcut. User adını değiştirin!");

                var result = await _userDal.Add(entity!);

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

            var result = await _userDal.Delete(id);

            return new Result(true, "Success");
        }

        public async Task<IDataResult<User>> Get(int id)
        {
            if (id <= 0)
                return new DataResult<User>(new User(), false, "Error");

            var result = await _userDal.Get(id);

            return new DataResult<User>(result, true, "Success");
        }

        public async Task<IDataResult<List<User>>> GetAll()
        {
            var result = await _userDal.GetAll();

            return new DataResult<List<User>>(result.ToList(), true);
        }

        public async Task<User> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(password))
                return new User();

            var result = await _userDal.Login(username, password);

            if (result == null)
                return new User();

            return result;
        }

        public async Task<IResult> Update(User entity)
        {
            if (entity != null)
            {
                var errorMessages = ValidationTool.Validate(new UserValidator(), entity);

                if (!string.IsNullOrEmpty(errorMessages))
                {
                    return new DataResult<int>(0, false, errorMessages);
                }


                var isExist = await _userDal.UserExists(entity?.Username!);

                if (isExist)
                    return new DataResult<int>(0, false, "User mevcut. User adını değiştirin!");

                var result = await _userDal.Update(entity!);

                if (result < 1)
                {
                    return new DataResult<int>(0, false, "Error");
                }

                return new DataResult<int>(result, true
                    , "Success");
            }
            else
            {
                return new DataResult<int>(0, false, "Entity is Null");
            }
        }
    }
}
