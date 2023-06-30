using ePTS.Data.Extensions;
using ePTS.Entities;
using ePTS.Entities.Assessments;
using ePTS.Entities.Audit;
using ePTS.Entities.Core;
using ePTS.Entities.Gradebooks;
using ePTS.Entities.Identity;
using ePTS.Entities.Reference;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using System.Reflection.Emit;
using System.Text.Json;

namespace ePTS.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationRole> ApplicationRoles { get; set; } = null!;
        public DbSet<ApplicationUser> ApplicationUsers { get; set; } = null!;
        public DbSet<ApplicationUserOrganization> ApplicationUserOrganizations { get; set; } = null!;
        public DbSet<Organization> Organizations { get; set; } = null!;
        public DbSet<School> Schools { get; set; } = null!;
        public DbSet<SchoolAcademicYear> SchoolAcademicYears { get; set; } = null!;
        public DbSet<Gradebook> Gradebooks { get; set; } = null!;
        public DbSet<GradebookEnrollment> GradebookEnrollments { get; set; } = null!;
        public DbSet<GradebookAssessment> GradebookAssessments { get; set; } = null!;
        public DbSet<GradebookPeriod> GradebookPeriods { get; set; } = null!;
        public DbSet<GradebookAssessmentPeriod> GradebookAssessmentPeriods { get; set; } = null!;
        public DbSet<AssessmentResult> AssessmentResults { get; set; } = null!;
        public DbSet<AssessmentPerformanceLevel> AssessmentPerformanceLevels { get; set; } = null!;
        public DbSet<Assessment> Assessments { get; set; } = null!;
        public DbSet<AssessmentItem> AssessmentItems { get; set; } = null!;
        public DbSet<RefAcademicYear> AcademicYears { get; set; } = null!;
        public DbSet<RefAcademicYearStatus> AcademicYearStatus { get; set; } = null!;
        public DbSet<RefAssessmentCategory> AssessmentCategories { get; set; } = null!;
        public DbSet<RefAssessmentPlatformType> AssessmentPlatformTypes { get; set; } = null!;
        public DbSet<RefAssessmentStatus> AssessmentStatus { get; set; } = null!;
        public DbSet<RefAssessmentTerm> AssessmentTerms { get; set; } = null!;
        public DbSet<RefAssessmentType> AssessmentTypes { get; set; } = null!;
        public DbSet<RefAssessmentWeek> AssessmentWeeks { get; set; } = null!;
        public DbSet<RefGradebookAssessmentStatus> GradebookAssessmentStatus { get; set; } = null!;
        public DbSet<RefGradebookStatus> GradebookStatus { get; set; } = null!;
        public DbSet<RefGradeLevel> GradeLevels { get; set; } = null!;
        public DbSet<RefLocation> Locations { get; set; } = null!;
        public DbSet<RefLocationType> LocationTypes { get; set; } = null!;
        public DbSet<RefOrganizationType> OrganizationTypes { get; set; } = null!;
        public DbSet<RefParticipantType> ParticipantTypes { get; set; } = null!;
        public DbSet<RefPerformanceLevel> PerformanceLevels { get; set; } = null!;
        public DbSet<RefSchoolAdministrationType> SchoolAdministrationTypes { get; set; } = null!;
        public DbSet<RefSchoolLanguage> SchoolLanguages { get; set; } = null!;
        public DbSet<RefSchoolLocation> SchoolLocations { get; set; } = null!;
        public DbSet<RefSchoolStatus> SchoolStatus { get; set; } = null!;
        public DbSet<RefSchoolType> SchoolTypes { get; set; } = null!;
        public DbSet<RefSex> Sex { get; set; } = null!;
        public DbSet<RefGradebookPeriodType> GradebookPeriodTypes { get; set; } = null!;
        public DbSet<RefGradebookPeriodStatus> GradebookPeriodStatus { get; set; } = null!;

        // Audit Log
        public DbSet<AuditLog> AuditLogs { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Add Query Filters to all entities
            // https://docs.microsoft.com/en-us/ef/core/querying/filters
            builder.Entity<Organization>(entity =>
            {
                // Global query filter to ignore rows where IsDeleted is true
                entity.HasQueryFilter(p => !p.IsDeleted);
                // Set the column type to SQL date (not datetime)
                entity.Property(p => p.RegistrationDate).HasColumnType("date");
                entity.UseTptMappingStrategy();
            });

            builder.Entity<SchoolAcademicYear>(entity =>
            {
                entity.HasQueryFilter(p => !p.IsDeleted);
                // Composite index on these two columns
                entity.HasIndex(s => new { s.OrganizationId, s.RefAcademicYearId })
                // The combination of values in these columns must be unique across all rows
                .IsUnique();
                entity.Property(p => p.RegistrationDate).HasColumnType("date");

            });

            builder.Entity<Gradebook>(entity =>
            {
                entity.HasQueryFilter(p => !p.IsDeleted);
                entity.HasIndex(s => new { s.SchoolAcademicYearId, s.RefGradeLevelId }).IsUnique();
                entity.Property(p => p.RegistrationDate).HasColumnType("date");
            });

            builder.Entity<GradebookEnrollment>(entity =>
            {
                entity.HasQueryFilter(p => !p.IsDeleted);
                entity.HasIndex(s => new { s.GradebookId, s.RefParticipantTypeId }).IsUnique();
                entity.Property(p => p.RegistrationDate).HasColumnType("date");
            });

            builder.Entity<GradebookAssessment>(entity =>
            {
                entity.HasQueryFilter(p => !p.IsDeleted);
                entity.HasIndex(s => new { s.GradebookId, s.GradebookAssessmentPeriodId }).IsUnique();
                entity.Property(p => p.RegistrationDate).HasColumnType("date");
            });

            builder.Entity<GradebookPeriod>(entity =>
            {
                entity.HasQueryFilter(p => !p.IsDeleted);
                entity.Property(p => p.RegistrationDate).HasColumnType("date");
            });

            builder.Entity<GradebookAssessmentPeriod>(entity =>
            {
                entity.HasQueryFilter(p => !p.IsDeleted);
            });

            builder.Entity<AssessmentResult>(entity =>
            {
                entity.HasQueryFilter(p => !p.IsDeleted);
                entity.HasIndex(s => new { s.GradebookAssessmentId, s.AssessmentItemId }).IsUnique();
                entity.Property(p => p.RegistrationDate).HasColumnType("date");
            });

            builder.Entity<AssessmentPerformanceLevel>(entity =>
            {
                entity.HasQueryFilter(p => !p.IsDeleted);
                entity.HasIndex(s => new { s.GradebookAssessmentId, s.RefPerformanceLevelId, s.RefSexId }).IsUnique();
                entity.Property(p => p.RegistrationDate).HasColumnType("date");
            });

            builder.Entity<Assessment>(entity =>
            {
                entity.HasQueryFilter(p => !p.IsDeleted);
                entity.Property(p => p.RegistrationDate).HasColumnType("date");
            });

            builder.Entity<AssessmentItem>(entity =>
            {
                entity.HasQueryFilter(p => !p.IsDeleted);
            });

            builder.Entity<ApplicationUserOrganization>(entity =>
            {
                entity.HasQueryFilter(p => !p.IsDeleted);
            });


            // Rename the default ASP.NET Identity table names to "Application...".
            // Customize the ASP.NET Identity model and overrided the defaults
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "ApplicationUser");
                entity.Property(e => e.Id).HasColumnName("UserId");
                entity.HasQueryFilter(e => !e.IsDeleted);
            });

            builder.Entity<ApplicationRole>(entity =>
            {
                entity.ToTable(name: "ApplicationRole");
                entity.Property(e => e.Id).HasColumnName("RoleId");
            });

            builder.Entity<IdentityUserClaim<Guid>>(entity =>
            {
                entity.ToTable("ApplicationUserClaim");
                entity.Property(e => e.Id).HasColumnName("ClaimId");
            });

            builder.Entity<IdentityUserRole<Guid>>(entity =>
            {
                entity.ToTable("ApplicationUserRole");
                entity.HasKey(e => new { e.UserId, e.RoleId });
            });

            builder.Entity<IdentityUserLogin<Guid>>(entity =>
            {
                entity.ToTable("ApplicationUserLogin");
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });
            });

            builder.Entity<IdentityRoleClaim<Guid>>(entity =>
            {
                entity.ToTable("ApplicationRoleClaim");
                entity.Property(e => e.Id).HasColumnName("ClaimId");
            });

            builder.Entity<IdentityUserToken<Guid>>(entity =>
            {
                entity.ToTable("ApplicationUserToken");
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
            });

            // Seed database
            // builder.Seed();

        }


        /// <summary>
        /// Overrides the base SaveChanges method to provide options for accepting all changes and triggering <see cref="OnBeforeSaving"/>
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">Flag to indicate if all changes should be accepted upon successful save</param>
        /// <returns>The number of state entries written to the database</returns>
        /// <remarks>
        /// Soft-Delete functionality will cascade to all associated dependent entities via navigation properties that are not marked as dependent
        /// </remarks>
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }


        /// <summary>
        /// Saves changes made in this context to the underlying database asynchronously.
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">Indicates whether all changes should be accepted when applied successfully.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous save operation. The task result contains the number of state entries written to the database.</returns>
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        /// <summary>
        /// Override method to act on the DbContext before the changes are saved to the database.
        /// Sets the necessary fields for newly added, modified and deleted entities for auditing purposes.
        /// </summary>
        private void OnBeforeSaving()
        {

            string? userName = null;
            var httpContext = new HttpContextAccessor().HttpContext;
            //var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                userName = httpContext.User?.Identity?.Name;
            }

            ChangeTracker.DetectChanges();
            List<AuditLog> tempAuditLogs = new List<AuditLog>();

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is IAuditable)
                {
                    AuditLog? auditLog = null;
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            // Code for EntityState.Added
                            auditLog = new AuditLog
                            {
                                UserId = userName,
                                Type = "Added",
                                TableName = entry.Metadata.GetTableName(),
                                DateTime = DateTimeOffset.UtcNow,
                                NewValues = JsonSerializer.Serialize(entry.CurrentValues.ToObject()),
                                PrimaryKey = entry.Metadata.FindPrimaryKey()?.Properties.Select(x => x.Name).FirstOrDefault()
                            };
                            break;

                        case EntityState.Modified:
                            // Code for EntityState.Modified
                            auditLog = new AuditLog
                            {
                                UserId = userName,
                                Type = "Modified",
                                TableName = entry.Metadata.GetTableName(),
                                DateTime = DateTimeOffset.UtcNow,
                                OldValues = JsonSerializer.Serialize(entry.OriginalValues.ToObject()),
                                NewValues = JsonSerializer.Serialize(entry.CurrentValues.ToObject()),
                                AffectedColumns = string.Join(", ", entry.Properties.Where(p => p.IsModified).Select(p => p.Metadata.Name)),
                                PrimaryKey = entry.Metadata.FindPrimaryKey()?.Properties.Select(x => x.Name).FirstOrDefault()
                            };
                            break;
                    }
                    // Add audit log to the temporary list
                    if (auditLog != null)
                    {
                        tempAuditLogs.Add(auditLog);
                    }
                }

                if (entry.Entity is BaseEntity)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entry.CurrentValues["IsDeleted"] = false;
                            entry.CurrentValues["CreatedDate"] = DateTimeOffset.UtcNow;
                            entry.CurrentValues["CreatedBy"] = userName;
                            break;

                        case EntityState.Modified:
                            entry.State = EntityState.Modified;
                            entry.CurrentValues["IsDeleted"] = false;
                            entry.CurrentValues["CreatedDate"] = entry.GetDatabaseValues()?.GetValue<object>("CreatedDate");
                            entry.CurrentValues["CreatedBy"] = entry.GetDatabaseValues()?.GetValue<object>("CreatedBy");
                            entry.CurrentValues["ModifiedDate"] = DateTimeOffset.UtcNow;
                            entry.CurrentValues["ModifiedBy"] = userName;
                            break;

                        case EntityState.Deleted:
                            entry.State = EntityState.Modified;
                            //Soft delete cascade
                            //https://github.com/dotnet/efcore/issues/11240
                            entry.CurrentValues["IsDeleted"] = true;
                            entry.CurrentValues["CreatedDate"] = entry.GetDatabaseValues()?.GetValue<object>("CreatedDate");
                            entry.CurrentValues["CreatedBy"] = entry.GetDatabaseValues()?.GetValue<object>("CreatedBy");
                            entry.CurrentValues["ModifiedDate"] = entry.GetDatabaseValues()?.GetValue<object>("ModifiedDate");
                            entry.CurrentValues["ModifiedBy"] = entry.GetDatabaseValues()?.GetValue<object>("ModifiedBy");
                            entry.CurrentValues["DeletedDate"] = DateTimeOffset.UtcNow;
                            entry.CurrentValues["DeletedBy"] = userName;
                            foreach (var navigationEntry in entry.Navigations.Where(x => !((IReadOnlyNavigation)x.Metadata).IsOnDependent))
                            {
                                if (navigationEntry is CollectionEntry collectionEntry)
                                {
                                    foreach (var dependentEntry in collectionEntry.CurrentValue!)
                                    {
                                        HandleDependent(Entry(dependentEntry));
                                    }
                                }
                                else
                                {
                                    var dependentEntry = navigationEntry.CurrentValue;
                                    if (dependentEntry != null)
                                    {
                                        HandleDependent(Entry(dependentEntry));
                                    }
                                }
                            }
                            break;
                    }
                }
            }

            // Add audit logs from the temporary list to the DbSet
            foreach (var auditLog in tempAuditLogs)
            {
                AuditLogs.Add(auditLog);
            }
        }

        private static void HandleDependent(EntityEntry entry)
        {
            entry.CurrentValues["IsDeleted"] = true;
        }
    }
}