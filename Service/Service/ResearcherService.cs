using BAL.Repoistory;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Service
{
    public class ResearcherService
    {
        private readonly ResearcherRepo _repo = new ResearcherRepo();

        public List<Researcher> GetAll()
        {
            return _repo.GetAll();
        }

        public Researcher? GetById(int id)
        {
            return _repo.GetById(id);
        }
        public List<Researcher> SearchByName(string name)
        {
            return _repo.Find(r => r.FullName.ToLower().Contains(name.ToLower())).ToList();
        }

        public List<Researcher> Search(int? id, string? name)
        {
            var query = _repo.Query();
            if (id.HasValue)
            {
                query = query.Where(r => r.ResearcherId == id.Value);
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(r => r.FullName.ToLower().Contains(name.ToLower()));
            }
            return query.ToList();
        }
    }
}
