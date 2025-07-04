using BAL.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Repoistory
{
    public class UserAccountRepo
    {
        private readonly Su25researchDbContext _dbContext = new Su25researchDbContext();

        public UserAccount? GetById(int id)
        {
            return _dbContext.UserAccounts.Find(id);
        }

        public UserAccount? Get(string email)
        {
            return _dbContext.UserAccounts.FirstOrDefault(u => u.Email == email);
        }

        public List<UserAccount> GetAll()
        {
            return _dbContext.UserAccounts.ToList();
        }
    }
}
