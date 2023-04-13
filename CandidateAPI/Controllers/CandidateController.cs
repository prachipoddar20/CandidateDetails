using CandidateAPI.Models;
using CandidateAPI.Services;
using CandidateAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace CandidateAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CandidateController : ControllerBase
    {
        private readonly ILogger<CandidateController> _logger;
        private ICandidateService _candidateService;
        private IMemoryCache _cache;
        public CandidateController(ILogger<CandidateController> logger, ICandidateService candidateService, IMemoryCache cache)
        {
            _logger = logger;
            _candidateService = candidateService;
            _cache = cache;
        }

        /// <summary>
        /// Gets the list of all candidates.
        /// </summary>
        /// <returnsReturns List of Candidates with http status code 200 OK when request is successful</returns>
        [HttpGet]
        public async Task<IEnumerable<Candidate>> Get()
        {
            return await _candidateService.GetCandidates();
        }

        /// <summary>
        /// Creates a candidate data in the database.
        /// </summary>
        /// <param name="candidate"></param>
        [HttpPost("add")]
        public void AddCandidate([FromBody] Candidate candidate)
        {
            _candidateService.AddCandidates(candidate);
        }

        /// <summary>
        ///  Gets the list of candidates with details based on their years of experience in the given skill in descending order.
        /// </summary>
        /// <param name="skill"></param>
        /// <returns>Returns the list of candidates with details with response status code 200</returns>
        [HttpGet("{skill}")]
        public async Task<IEnumerable<CandidateViewModel>> CandidateForSkill([FromRoute] string skill)
        {
            var response = await _candidateService.GetMostExperincedCandidates(skill);

            return response;
        }

        /// <summary>
        ///  Gets the list of skills for a candidate using its name. Also caches the data for faster future retrievals.
        /// </summary>
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <returns>Returns Candidate details along with all skills and skill details with status code 200 when request is successful</returns>
        [HttpGet("skill/{firstname}/{lastname}")]
        public async Task<CandidateSkillViewModel> GetAllSkillsForCandidateNameAsync([FromRoute] string firstname, [FromRoute] string lastname)
        {
            _logger.Log(LogLevel.Information, "Trying to fetch the data from cache.");
            if (_cache.TryGetValue(firstname + lastname, out CandidateSkillViewModel response))
            {
                _logger.Log(LogLevel.Information, "Data found in cache.");
            }
            else
            {
                _logger.Log(LogLevel.Information, "Data not found in cache. Fetching from database.");

                response = await _candidateService.GetAllSkillsForCandidateNameAsync(firstname, lastname);

                _cache.Set(firstname + lastname, response);
                
            }

            return response;
        }

        /// <summary>
        /// Gets the list of skills for a candidate using its id. Also caches the data for faster future retrievals.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns Candidate details along with all skills and skill details with status code 200 when request is successful</returns>
        [HttpGet("skill/{id}")]
        public async Task<CandidateSkillViewModel> GetAllSkillsForCandidateIdAsync([FromRoute] int id)
        {

            _logger.Log(LogLevel.Information, "Trying to fetch the data from cache.");
            if (_cache.TryGetValue(id, out CandidateSkillViewModel response))
            {
                _logger.Log(LogLevel.Information, "Data found in cache.");
            }
            else
            {
                _logger.Log(LogLevel.Information, "Data not found in cache. Fetching from database.");

                response = await _candidateService.GetAllSkillsForCandidateIdAsync(id);

                _cache.Set(id, response);
            }

            return response;
        }

    }
}
