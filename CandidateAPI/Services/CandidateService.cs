using CandidateAPI.Models;
using CandidateAPI.Repositories;
using CandidateAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CandidateAPI.Services
{
    public class CandidateService : ICandidateService
    {
        private ICandidateRepository _candidateRepository;
        public CandidateService(ICandidateRepository candidateRepository)
        {
            _candidateRepository = candidateRepository;
        }

        public async Task<IEnumerable<Candidate>> GetCandidates()
        {
            return await _candidateRepository.GetAllAsync();
        }

        public void AddCandidates(Candidate candidate)
        {
            _candidateRepository.AddCandidateAsync(candidate);
        }


        public async Task<IEnumerable<CandidateViewModel>> GetMostExperincedCandidates(string skill)
        {
            try
            {
                var result = _candidateRepository.GetMostExperiencedCandidates(skill);
                return await result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<CandidateSkillViewModel> GetAllSkillsForCandidateNameAsync(string firstname, string lastname)
        {
            return await _candidateRepository.GetAllSkillsForCandidateNameAsync(firstname, lastname);
        }

        public async Task<CandidateSkillViewModel> GetAllSkillsForCandidateIdAsync(int id)
        {
            return await _candidateRepository.GetAllSkillsForCandidateIdAsync(id);
        }
    }
}
