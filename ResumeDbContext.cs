using Microsoft.EntityFrameworkCore;
using ResumeManager.Models;

namespace ResumeManager
{
    public class ResumeDbContext : DbContext
    {
        public ResumeDbContext(DbContextOptions options) : base(options)
        {
        }
        public virtual DbSet<Applicant> Applicants { get; set; }
        public virtual DbSet<Experience> Experiences { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Member> Members { get; set; }

    }
}
