using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CandidateAPI.ViewModels
{
    public class CandidateViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public long PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int TotalNoOfYearsExperience { get; set; }
        public List<string> skillList { get; set; }
        public List<string> CompanysList { get; set; }
    }
}
