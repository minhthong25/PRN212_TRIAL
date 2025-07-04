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
    public class ResearchProjectRepo
    {
        private Su25researchDbContext _dbContext;

        public ResearchProjectRepo()
        {
            _dbContext = new Su25researchDbContext();
        }
        public ResearchProject? GetById(int id)
        {
            return _dbContext.ResearchProjects.Find(id);
        }

        public List<ResearchProject> GetAll()
        {
            return _dbContext.ResearchProjects.ToList();
        }

        public void Add(ResearchProject project)
        {
            _dbContext = new Su25researchDbContext();

            _dbContext.ResearchProjects.Add(project);
            _dbContext.SaveChanges();
        }

        public void Update(ResearchProject project)
        {
            _dbContext = new Su25researchDbContext();

            var tracker = _dbContext.Attach(project);
            tracker.State = EntityState.Modified;

            _dbContext.SaveChanges();
        }


        public void Delete(int id)
        {
            _dbContext = new Su25researchDbContext();

            var project = _dbContext.ResearchProjects.Find(id);
            if (project != null)
            {
                _dbContext.ResearchProjects.Remove(project);
                _dbContext.SaveChanges();
            }
        }
    }
}
