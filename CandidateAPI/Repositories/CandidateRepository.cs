using CandidateAPI.Data;
using CandidateAPI.Models;
using CandidateAPI.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace CandidateAPI.Repositories
{
    public class CandidateRepository : ICandidateRepository
    {
        private CandidateDataContext _context;
        private IMemoryCache _cache;
        public CandidateRepository(CandidateDataContext candidateDataContext, IMemoryCache cache)
        {
            _context = candidateDataContext;
            _cache = cache;
        }

        /// <summary>
        /// Fetches list of candidate with details from db
        /// </summary>
        /// <returns>Returns list of candidate</returns>
        public virtual async Task<ICollection<Candidate>> GetAllAsync()
        {
            return await _context.Set<Candidate>().ToListAsync();
        }

        /// <summary>
        /// Creates candidate in db
        /// </summary>
        /// <param name="candidate"></param>
        public virtual void AddCandidateAsync(Candidate candidate)
        {
            _context.Add(candidate);
            _context.SaveChanges();
        }

        /// <summary>
        /// Checks if the given skill exists in the system, if yes gets the candidate details with experience in the skill in decreasing order of years of experience
        /// </summary>
        /// <param name="skill"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public virtual async Task<IEnumerable<CandidateViewModel>> GetMostExperiencedCandidates(string skill)
        {
            //checking if skill exists
            if (_context.Skills.Any(s => s.Name == skill))
            {
                //find maximum years of experience for given skill
                var yearsOfExperience = (from e in _context.Experiences join s in _context.Skills on e.skillId equals s.Id where s.Name == skill select e.NumberOfYears).ToList();
                if (yearsOfExperience.Count == 0)
                {
                    throw new Exception("No experience found for the given skill.");
                }
                int maximumYearsofExperience = yearsOfExperience.Max();

                //list of candidates with details with maximum number of years of experience  for given skill
                var result = (from s in _context.Skills
                              join c in _context.Candidates on s.CandidateId equals c.Id
                              join e in _context.Experiences on s.Id equals e.skillId
                              where s.Name == skill
                              orderby e.NumberOfYears descending
                              select new CandidateViewModel()
                              {
                                  FirstName = c.FirstName,
                                  LastName = c.LastName,
                                  EmailAddress = c.EmailAddress,
                                  PhoneNumber = c.PhoneNumber,
                                  DateOfBirth = Convert.ToDateTime(c.DateOfBirth),
                                  TotalNoOfYearsExperience = maximumYearsofExperience,
                                  skillList = _context.Skills.Where(s => s.CandidateId == c.Id && s.Name == skill).Select(s => s.Name).ToList(),
                                  CompanysList = _context.Experiences.Where(exp => exp.CandidateId == c.Id && exp.skillId == s.Id).Select(exp => exp.CompanyName).ToList()
                              }).ToListAsync();

                return await result;
            }
            else
            {
                throw new Exception("Given skill does not exist.");
            }
        }

        /// <summary>
        /// Checks if user with the given name exists in db, if yes gets its details along with skill details
        /// </summary>
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <returns>Returns candidate details along with skill details</returns>
        /// <exception cref="Exception"></exception>
        public virtual async Task<CandidateSkillViewModel> GetAllSkillsForCandidateNameAsync(string firstname, string lastname)
        {
            //checks in cache first, if not found here then fetches from db
            _cache.TryGetValue(firstname + lastname, out CandidateSkillViewModel response);

            //check if user exists
            if(!(_context.Candidates.Any(c => c.FirstName == firstname && c.LastName == lastname)))
            {
                throw new Exception("Candidate not found with the given firstname and lastname");
            }



            var skillDetails = (from candidate in _context.Candidates
                                join skill in _context.Skills on candidate.Id equals skill.CandidateId
                                join e in _context.Experiences on skill.Id equals e.skillId
                                where candidate.FirstName == firstname && candidate.LastName == lastname
                                select new SkillViewModel()
                                {
                                    SkillId = skill.Id,
                                    SkillName = skill.Name,
                                    NoOfYearsExperience = e.NumberOfYears,
                                    CompanyName = e.CompanyName,
                                    StartDateToEnd = e.StartDate + " to " + (e.EndDate == null ? DateTime.Today.ToString("MM/dd/yyyy") : e.EndDate)
                                }).ToList();

            var result = from c in _context.Candidates
                         join a in _context.Addresses on c.Id equals a.CandidateId
                         where c.FirstName == firstname && c.LastName == lastname
                         select new CandidateSkillViewModel()
                         {
                             FirstName = c.FirstName,
                             LastName = c.LastName,
                             EmailAddress = c.EmailAddress,
                             PhoneNumber = c.PhoneNumber,
                             DateOfBirth = Convert.ToDateTime(c.DateOfBirth),
                             CandidateCurrentCity = a.City,
                             CandidateSkills = skillDetails
                         };

            int id = _context.Candidates.Where(c => c.FirstName == firstname && c.LastName == lastname).Select(c => c.Id).FirstOrDefault();

            //store in cache with both id and name as keys
            _cache.Set(id, result);
            _cache.Set(firstname+lastname, result);
            return await result.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Checks if user with the given id exists in db, if yes gets its details along with skill details
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns details along with skill details</returns>
        /// <exception cref="Exception"></exception>
        public virtual async Task<CandidateSkillViewModel> GetAllSkillsForCandidateIdAsync(int id)
        {
            //checks in cache first, if not found here then fetches from db
            _cache.TryGetValue(id, out CandidateSkillViewModel response);

            if (!(_context.Candidates.Any(c => c.Id == id)))
            {
                throw new Exception("Candidate not found with the given id");
            }

            var skillDetails = (from candidate in _context.Candidates
                                join skill in _context.Skills on candidate.Id equals skill.CandidateId
                                join e in _context.Experiences on skill.Id equals e.skillId
                                where candidate.Id == id
                                select new SkillViewModel()
                                {
                                    SkillId = skill.Id,
                                    SkillName = skill.Name,
                                    NoOfYearsExperience = e.NumberOfYears,
                                    CompanyName = e.CompanyName,
                                    StartDateToEnd = e.StartDate + " to " + (e.EndDate == null ? DateTime.Today.ToString("MM/dd/yyyy") : e.EndDate)
                                }).ToList();

            var result = from c in _context.Candidates
                         join a in _context.Addresses on c.Id equals a.CandidateId
                         where c.Id == id
                         select new CandidateSkillViewModel()
                         {
                             FirstName = c.FirstName,
                             LastName = c.LastName,
                             EmailAddress = c.EmailAddress,
                             PhoneNumber = c.PhoneNumber,
                             DateOfBirth = Convert.ToDateTime(c.DateOfBirth),
                             CandidateCurrentCity = a.City,
                             CandidateSkills = skillDetails
                         };

            string firstname = _context.Candidates.Where(c => c.Id == id).Select(c => c.FirstName).FirstOrDefault();
            string lastname = _context.Candidates.Where(c => c.Id == id).Select(c => c.LastName).FirstOrDefault();

            //store in cache with both id and name as keys
            _cache.Set(firstname+lastname, result);
            _cache.Set(id, result);
            return await result.FirstOrDefaultAsync();
        }



        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}