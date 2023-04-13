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

        [HttpGet]
        public async Task<IEnumerable<Candidate>> Get()
        {
            return await _candidateService.GetCandidates();
        }

        [HttpPost("add")]
        public void AddCandidate([FromBody] Candidate candidate)
        {
            _candidateService.AddCandidates(candidate);
        }

        [HttpGet("{skill}")]
        public async Task<IEnumerable<CandidateViewModel>> CandidateForSkill([FromRoute] string skill)
        {
            var response = await _candidateService.GetMostExpCandidates(skill);

            return response;
        }

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
