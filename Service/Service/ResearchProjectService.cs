using BAL.Repoistory;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Service
{
    public class ResearchProjectService
    {
        private readonly ResearchProjectRepo _repo = new ResearchProjectRepo();
        public List<ResearchProject> GetAll()
        {
            var projects = _repo.GetAll();
            var researcherService = new ResearcherService();
            var researchers = researcherService.GetAll();
            foreach (var project in projects)
            {
                project.LeadResearcher = researchers.FirstOrDefault(r => r.ResearcherId == project.LeadResearcherId);
            }
            return projects;
        }

        public ResearchProject? GetById(int id)
        {
            return _repo.GetById(id);
        }

        public void AddService(ResearchProject project)
        {
            _repo.Add(project);
        }

        public void UpdateService(ResearchProject project)
        {
            _repo.Update(project);
        }

        public void DeleteService(int id)
        {
            _repo.Delete(id);
        }

        public List<ResearchProject> SearchService(int? id, string? title)
        {
            var query = _repo.GetAll().AsQueryable();
            if (id.HasValue)
            {
                query = query.Where(p => p.ProjectId == id.Value);
            }
            if (!string.IsNullOrWhiteSpace(title))
            {
                query = query.Where(p => p.ProjectTitle.ToLower().Contains(title.ToLower()));
            }
            var results = query.ToList();
            var researcherService = new ResearcherService();
            var researchers = researcherService.GetAll();
            foreach (var project in results)
            {
                project.LeadResearcher = researchers.FirstOrDefault(r => r.ResearcherId == project.LeadResearcherId);
            }
            return results;
        }
    }
}
