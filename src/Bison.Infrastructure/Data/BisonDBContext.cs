using Bison.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Bison.Infrastructure.Data;

public class BisonDBContext :  IdentityDbContext<ApplicationUser>
{
    public DbSet<Post> Posts { get; set; }
    public DbSet<Observation> Observations { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Proposal> Proposals { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Taxon> Taxons { get; set; }

    public DbSet<Image> Images { get; set; }

    public BisonDBContext(DbContextOptions<BisonDBContext> options) : base(options) { }
}