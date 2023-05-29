using ePTS.Entities.Assessments;
using ePTS.Entities.Core;
using ePTS.Entities.Enrollments;
using ePTS.Entities.Gradebooks;
using ePTS.Entities.Identity;
using ePTS.Entities.Reference;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
        public DbSet<SchoolEnrollment> SchoolEnrollments { get; set; } = null!;
        public DbSet<Gradebook> Gradebooks { get; set; } = null!;
        public DbSet<GradebookAssessment> GradebookAssessments { get; set; } = null!;
        public DbSet<GradebookPeriod> GradebookPeriods { get; set; } = null!;
        public DbSet<GradebookPeriodForm> GradebookPeriodForms { get; set; } = null!;
        public DbSet<AssessmentResult> AssessmentResults { get; set; } = null!;
        public DbSet<AssessmentPerformanceLevel> AssessmentPerformanceLevels { get; set; } = null!;
        public DbSet<Assessment> Assessments { get; set; } = null!;
        public DbSet<AssessmentForm> AssessmentForms { get; set; } = null!;
        public DbSet<AssessmentFormItem> AssessmentFormItems { get; set; } = null!;
        public DbSet<RefAcademicYear> AcademicYears { get; set; } = null!;
        public DbSet<RefAcademicYearStatus> AcademicYearStatuses { get; set; } = null!;
        public DbSet<RefAssessmentCategory> AssessmentCategories { get; set; } = null!;
        public DbSet<RefAssessmentFormStatus> AssessmentFormStatuses { get; set; } = null!;
        public DbSet<RefAssessmentPeriod> AssessmentPeriods { get; set; } = null!;
        public DbSet<RefAssessmentPlatformType> AssessmentPlatformTypes { get; set; } = null!;
        public DbSet<RefAssessmentResultDataType> AssessmentResultDataTypes { get; set; } = null!;
        public DbSet<RefAssessmentStatus> AssessmentStatuses { get; set; } = null!;
        public DbSet<RefAssessmentTerm> AssessmentTerms { get; set; } = null!;
        public DbSet<RefAssessmentType> AssessmentTypes { get; set; } = null!;
        public DbSet<RefAssessmentWeek> AssessmentWeeks { get; set; } = null!;
        public DbSet<RefGradebookAssessmentStatus> GradebookAssessmentStatuses { get; set; } = null!;
        public DbSet<RefGradebookStatus> GradebookStatuses { get; set; } = null!;
        public DbSet<RefGradeLevel> GradeLevels { get; set; } = null!;
        public DbSet<RefLocation> Locations { get; set; } = null!;
        public DbSet<RefLocationType> LocationTypes { get; set; } = null!;
        public DbSet<RefOrganizationType> OrganizationTypes { get; set; } = null!;
        public DbSet<RefParticipantType> ParticipantTypes { get; set; } = null!;
        public DbSet<RefPerformanceLevel> PerformanceLevels { get; set; } = null!;
        public DbSet<RefSchoolAdministrationType> SchoolAdministrationTypes { get; set; } = null!;
        public DbSet<RefSchoolLanguage> SchoolLanguages { get; set; } = null!;
        public DbSet<RefSchoolLocation> SchoolLocations { get; set; } = null!;
        public DbSet<RefSchoolStatus> SchoolStatuses { get; set; } = null!;
        public DbSet<RefSchoolType> SchoolTypes { get; set; } = null!;
        public DbSet<RefScoreMetricType> ScoreMetricTypes { get; set; } = null!;
        public DbSet<RefSex> Sex { get; set; } = null!;
        public DbSet<RefGradebookPeriodType> GradebookPeriodTypes { get; set; } = null!;
        public DbSet<RefGradebookPeriodStatus> GradebookPeriodStatuses { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Customized the ASP.NET Identity model and overrided the defaults
            // renaming the ASP.NET table names to "Application...".
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "ApplicationUser");
                entity.Property(e => e.Id).HasColumnName("UserId");
            });

            builder.Entity<ApplicationRole>(entity =>
            {
                entity.ToTable(name: "ApplicationRole");
                entity.Property(e => e.Id).HasColumnName("RoleId");
            });

            builder.Entity<IdentityUserClaim<Guid>>(entity =>
            {
                entity.ToTable("ApplicationUserClaim");
            });

            builder.Entity<IdentityUserRole<Guid>>(entity =>
            {
                entity.ToTable("ApplicationUserRole");
            });

            builder.Entity<IdentityUserLogin<Guid>>(entity =>
            {
                entity.ToTable("ApplicationUserLogin");
            });

            builder.Entity<IdentityRoleClaim<Guid>>(entity =>
            {
                entity.ToTable("ApplicationRoleClaim");
            });

            builder.Entity<IdentityUserToken<Guid>>(entity =>
            {
                entity.ToTable("ApplicationUserToken");
            });

            builder.Entity<IdentityUserClaim<Guid>>(entity =>
            {
                entity.ToTable("ApplicationUserClaim");
            });


        }
    }
}