using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Repository.Entity;
using Repository.Interface;

namespace Repository.Data;

public class ApplicationDbContext :DbContext
{
    private readonly ICurrentUserService currentUserService;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUserService currentUserService) :base(options)
    {
        this.currentUserService = currentUserService;
    }
    public DbSet<FileUpload> Files { get; set; }
    public DbSet<ErrorLogger> FileErrorLogger { get; set; }
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (EntityEntry<BaseAuditableEntity> entry in ChangeTracker.Entries<BaseAuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = currentUserService.UserId;
                    entry.Entity.Created = DateTime.Now;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModifiedBy = currentUserService.UserId;
                    entry.Entity.LastModified = DateTime.Now;
                    break;
            }
        }
        var result = await base.SaveChangesAsync(cancellationToken);
        return result;
    }
}
