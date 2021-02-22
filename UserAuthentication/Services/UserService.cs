using System.Collections.Generic;
using System.Linq;
using UserAuthentication.Data;
using UserAuthentication.Data.Entities;
using UserAuthentication.Helpers;

namespace UserAuthentication.Services
{
    public class UserService : IUserService
    {
        private UserContext _context;
        public UserService(UserContext context)
        {
            _context = context;
        }

        public User Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            User user = _context.Users.FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                return null;
            }

            if (!PasswordHelper.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            return user;
        }

        public User Create(User user, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new InfoException("Password is required");
            }

            if (_context.Users.Any(u => u.Username == user.Username))
            {
                throw new InfoException("Username already taken");
            }

            byte[] passwordHash;
            byte[] passwordSalt;
            PasswordHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public void Delete(int id)
        {
            User user = _context.Users.Find(id);

            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        public IEnumerable<User> Get()
        {
            return _context.Users;
        }

        public User Get(int id)
        {
            return _context.Users.FirstOrDefault(u => u.UserId == id);
        }

        public void Update(User user, string password)
        {
            User updateUser = _context.Users.Find(user.UserId);

            if (updateUser == null)
            {
                throw new InfoException("User not found");
            }

            if (!string.IsNullOrWhiteSpace(user.Username) && user.Username != updateUser.Username)
            {
                if (_context.Users.Any(u => u.Username == user.Username))
                {
                    throw new InfoException("Username already taken");
                }

                updateUser.Username = user.Username;
            }

            if (!string.IsNullOrWhiteSpace(user.Firstname))
            {
                updateUser.Firstname = user.Firstname;
            }

            if (!string.IsNullOrWhiteSpace(user.Lastname))
            {
                updateUser.Lastname = user.Lastname;
            }

            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash;
                byte[] passwordSalt;
                PasswordHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);

                updateUser.PasswordHash = passwordHash;
                updateUser.PasswordSalt = passwordSalt;
            }

            _context.Users.Update(updateUser);
            _context.SaveChanges();
        }
    }
}
