using System;
namespace CandidateAPI.Models
{
    public class Experience
    {
        public int Id { get; set; }
        public int skillId { get; set; }
        public int CandidateId { get; set; }
        public int NumberOfYears { get; set; }
        public string CompanyName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}
