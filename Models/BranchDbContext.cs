using System.Data.Entity;

namespace BranchManagementApp.Models
{
    public class BranchDbContext : DbContext
    {
        // Constructor
        public BranchDbContext() : base("name=BranchDbContext") // Connection string name
        {
        }

        // Define DbSet for Branch
        public DbSet<Branch> Branches { get; set; }
    }
}
