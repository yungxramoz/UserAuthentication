using System.Collections.Generic;
using UserAuthentication.Data.Entities;

namespace UserAuthentication.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        IEnumerable<User> Get();
        User Get(int id);
        User Create(User user, string password);
        void Update(User user, string password);
        void Delete(int id);
    }
}
