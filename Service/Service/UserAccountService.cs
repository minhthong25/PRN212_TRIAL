using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAL.Repoistory;
using Repository.Models;

namespace BLL.Service
{
    public class UserAccountService
    {
        private readonly UserAccountRepo _repo = new UserAccountRepo();
        public List<UserAccount> GetAll()
        {
            return _repo.GetAll();
        }
        public UserAccount? GetById(int id)
        {
            return _repo.GetById(id);
        }
        public UserAccount? Get(string email)
        {
            return _repo.Get(email);
        }
        public bool CheckLogin(string username, string password)
        {
            var user = _repo.GetAll().FirstOrDefault(u => u.Email == username && u.Password == password);
            return user != null;
        }
    }
}
