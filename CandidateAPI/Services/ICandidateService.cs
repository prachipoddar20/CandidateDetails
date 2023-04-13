using CandidateAPI.Models;
using CandidateAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CandidateAPI.Services
{
    public interface ICandidateService
    {
        Task<IEnumerable<Candidate>> GetCandidates();

        void AddCandidates(Candidate candidate);

        Task<IEnumerable<CandidateViewModel>> GetMostExpCandidates(string skill);

        Task<CandidateSkillViewModel> GetAllSkillsForCandidateNameAsync(string firstname, string lastname);

        Task<CandidateSkillViewModel> GetAllSkillsForCandidateIdAsync(int id);
    }
}
