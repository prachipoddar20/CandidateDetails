﻿namespace CandidateAPI.Models
{
    public class Address
    {
        public int Id { get; set; }
        public int CandidateId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }
}
