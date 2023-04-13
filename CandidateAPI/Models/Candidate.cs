using System;
using System.Collections.Generic;

namespace CandidateAPI.Models
{
    public class Candidate
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public long PhoneNumber { get; set; }
        public string DateOfBirth { get; set; }

    }
}
