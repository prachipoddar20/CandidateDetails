using CandidateAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CandidateAPI.Data
{
    public class CandidateDataContext : DbContext
    {
        public CandidateDataContext(DbContextOptions<CandidateDataContext> options)
            : base(options)
        { }

        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Experience> Experiences { get; set; }
    }
}
