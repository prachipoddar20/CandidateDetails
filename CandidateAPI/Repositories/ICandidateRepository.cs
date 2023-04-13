using CandidateAPI.Models;
using CandidateAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CandidateAPI.Repositories
{
    public interface ICandidateRepository
    {
        Task<ICollection<Candidate>> GetAllAsync();
        void AddCandidateAsync(Candidate candidate);

        Task<IEnumerable<CandidateViewModel>> GetMostExpCandidates(string skill);

        Task<CandidateSkillViewModel> GetAllSkillsForCandidateNameAsync(string firstname, string lastname);

        Task<CandidateSkillViewModel> GetAllSkillsForCandidateIdAsync(int id);
    }
}