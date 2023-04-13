using System.Collections.Generic;
namespace CandidateAPI.Models
{
    public class Skill
    {
        public int Id { get; set; }

        public int CandidateId { get; set; }
        public string Name { get; set; }
    }
}
