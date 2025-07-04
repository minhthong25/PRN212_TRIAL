using BAL.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Repoistory
{
    public class ResearcherRepo
    {
        private readonly Su25researchDbContext _dbContext = new Su25researchDbContext();

        public Researcher? GetById(int id)
        {
            return _dbContext.Researchers.Find(id);
        }

        public List<Researcher> GetAll()
        {
            return _dbContext.Researchers.ToList();
        }

        public void Add(Researcher researcher)
        {
            _dbContext.Researchers.Add(researcher);
            _dbContext.SaveChanges();
        }

        public void Update(Researcher researcher)
        {
            _dbContext.Researchers.Update(researcher);
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var researcher = _dbContext.Researchers.Find(id);
            if (researcher != null)
            {
                _dbContext.Researchers.Remove(researcher);
                _dbContext.SaveChanges();
            }
        }
        public IQueryable<Researcher> Query()
        {
            return _dbContext.Researchers.AsQueryable();
        }
        public List<Researcher> Find(Expression<Func<Researcher, bool>> predicate)
        {
            return _dbContext.Researchers.Where(predicate).ToList();
        }
    }
}
