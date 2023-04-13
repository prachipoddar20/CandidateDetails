using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CandidateAPI.ViewModels
{
    public class CandidateSkillViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public long PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }

        public string CandidateCurrentCity { get; set; }
        public IEnumerable<SkillViewModel> CandidateSkills { get; set; }

        
    }
}
