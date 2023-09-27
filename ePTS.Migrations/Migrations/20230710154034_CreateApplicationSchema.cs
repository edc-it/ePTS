using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ePTS.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class CreateApplicationSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessageTemplate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Level = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeStamp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Exception = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JsonLog = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Properties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogEvent = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationRole",
                columns: table => new
                {
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationRole", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUser",
                columns: table => new
                {
                    FirstName = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true, comment: "The first name or given name of a user"),
                    LastName = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true, comment: "The last name or family name of a user"),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "The user or entity responsible for creating or adding the record"),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true, comment: "The date and time when the record was created"),
                    ModifiedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "The user or entity responsible for modifying the record"),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true, comment: "The date and time when the record was last modified"),
                    DeletedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "The user or entity responsible for marking the record as deleted"),
                    DeletedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true, comment: "The date and time when the record was marked as deleted"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "A flag indicating whether the record is marked as deleted (true) or active (false)"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUser", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    OldValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AffectedColumns = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrimaryKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefAcademicYearStatus",
                columns: table => new
                {
                    RefAcademicYearStatusId = table.Column<int>(type: "int", nullable: false, comment: "The unique identifier for each academic year status in the table")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "A short code that represents the academic year status"),
                    AcademicYearStatus = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "The name of the academic year status"),
                    SortOrder = table.Column<int>(type: "int", nullable: true, comment: "A numeric value that represents the order in which the academic year statuses should be displayed")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefAcademicYearStatus", x => x.RefAcademicYearStatusId);
                });

            migrationBuilder.CreateTable(
                name: "RefAssessmentPlatformType",
                columns: table => new
                {
                    RefAssessmentPlatformTypeId = table.Column<int>(type: "int", nullable: false, comment: "The unique identifier for each assessment item type in the table")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "A short code that represents the assessment platform type"),
                    AssessmentPlatformType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "The name of the assessment item type "),
                    SortOrder = table.Column<int>(type: "int", nullable: true, comment: "A numeric value that represents the order in which the assessment item type should be displayed")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefAssessmentPlatformType", x => x.RefAssessmentPlatformTypeId);
                });

            migrationBuilder.CreateTable(
                name: "RefAssessmentStatus",
                columns: table => new
                {
                    RefAssessmentStatusId = table.Column<int>(type: "int", nullable: false, comment: "The unique identifier for each assessment item type in the table")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "A short code that represents the assessment status"),
                    AssessmentStatus = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "The name of the assessment item type "),
                    SortOrder = table.Column<int>(type: "int", nullable: true, comment: "A numeric value that represents the order in which the assessment item type should be displayed")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefAssessmentStatus", x => x.RefAssessmentStatusId);
                });

            migrationBuilder.CreateTable(
                name: "RefAssessmentTerm",
                columns: table => new
                {
                    RefAssessmentTermId = table.Column<int>(type: "int", nullable: false, comment: "The unique identifier for each assessment item type in the table")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "A short code that represents the assessment term"),
                    AssessmentTerm = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "The name of the assessment item type "),
                    SortOrder = table.Column<int>(type: "int", nullable: true, comment: "A numeric value that represents the order in which the assessment item type should be displayed")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefAssessmentTerm", x => x.RefAssessmentTermId);
                });

            migrationBuilder.CreateTable(
                name: "RefAssessmentType",
                columns: table => new
                {
                    RefAssessmentTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "A short code that represents the assessment type"),
                    AssessmentType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefAssessmentType", x => x.RefAssessmentTypeId);
                });

            migrationBuilder.CreateTable(
                name: "RefAssessmentWeek",
                columns: table => new
                {
                    RefAssessmentWeekId = table.Column<int>(type: "int", nullable: false, comment: "The unique identifier for each assessment item type in the table")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "A short code that represents the assessment week"),
                    AssessmentWeek = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "The name of the assessment item type "),
                    SortOrder = table.Column<int>(type: "int", nullable: true, comment: "A numeric value that represents the order in which the assessment item type should be displayed")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefAssessmentWeek", x => x.RefAssessmentWeekId);
                });

            migrationBuilder.CreateTable(
                name: "RefGradebookAssessmentStatus",
                columns: table => new
                {
                    RefGradebookAssessmentStatusId = table.Column<int>(type: "int", nullable: false, comment: "The unique identifier for each assessment item type in the table")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "A short code that represents the gradebook assessment status"),
                    GradebookAssessmentStatus = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "The name of the assessment item type "),
                    SortOrder = table.Column<int>(type: "int", nullable: true, comment: "A numeric value that represents the order in which the assessment item type should be displayed")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefGradebookAssessmentStatus", x => x.RefGradebookAssessmentStatusId);
                });

            migrationBuilder.CreateTable(
                name: "RefGradebookPeriodStatus",
                columns: table => new
                {
                    RefGradebookPeriodStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    GradebookPeriodStatus = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefGradebookPeriodStatus", x => x.RefGradebookPeriodStatusId);
                });

            migrationBuilder.CreateTable(
                name: "RefGradebookPeriodType",
                columns: table => new
                {
                    RefGradebookPeriodTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "A short code that represents the gradebook period type"),
                    GradebookPeriodType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefGradebookPeriodType", x => x.RefGradebookPeriodTypeId);
                });

            migrationBuilder.CreateTable(
                name: "RefGradebookStatus",
                columns: table => new
                {
                    RefGradebookStatusId = table.Column<int>(type: "int", nullable: false, comment: "The unique identifier for each assessment item type in the table")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "A short code that represents the gradebook status"),
                    GradebookStatus = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "The name of the assessment item type "),
                    SortOrder = table.Column<int>(type: "int", nullable: true, comment: "A numeric value that represents the order in which the assessment item type should be displayed")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefGradebookStatus", x => x.RefGradebookStatusId);
                });

            migrationBuilder.CreateTable(
                name: "RefGradeLevel",
                columns: table => new
                {
                    RefGradeLevelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "A short code that represents the grade level"),
                    GradeLevel = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    GradeLevelId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true, comment: "Abbreviated representation or code used to denote different educational or academic levels, such as G1 for Grade 1, or P1 for Primary 1"),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefGradeLevel", x => x.RefGradeLevelId);
                });

            migrationBuilder.CreateTable(
                name: "RefLocationType",
                columns: table => new
                {
                    RefLocationTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "A short code that represents the location type"),
                    LocationType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    LocationLevel = table.Column<int>(type: "int", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefLocationType", x => x.RefLocationTypeId);
                });

            migrationBuilder.CreateTable(
                name: "RefOrganizationType",
                columns: table => new
                {
                    RefOrganizationTypeId = table.Column<int>(type: "int", nullable: false, comment: "The foreign key identifier of the type of organization or entity. ")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "A short code that represents the organization type"),
                    IsOrganizationUnit = table.Column<bool>(type: "bit", nullable: false, comment: "A Boolean value indicating whether this entity is a container"),
                    SortOrder = table.Column<int>(type: "int", nullable: true),
                    IsSchool = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefOrganizationType", x => x.RefOrganizationTypeId);
                });

            migrationBuilder.CreateTable(
                name: "RefParticipantType",
                columns: table => new
                {
                    RefParticipantTypeId = table.Column<int>(type: "int", nullable: false, comment: "The unique identifier for each assessment item type in the table")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "A short code that represents the participant type"),
                    ParticipantType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "The name of the assessment item type "),
                    SortOrder = table.Column<int>(type: "int", nullable: true, comment: "A numeric value that represents the order in which the assessment item type should be displayed")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefParticipantType", x => x.RefParticipantTypeId);
                });

            migrationBuilder.CreateTable(
                name: "RefPerformanceLevel",
                columns: table => new
                {
                    RefPerformanceLevelId = table.Column<int>(type: "int", nullable: false, comment: "The unique identifier for each assessment item type in the table")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "A short code that represents the performance level"),
                    PerformanceLevel = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "The name of the assessment item type "),
                    MinPerformanceLevel = table.Column<double>(type: "float", nullable: true, comment: "The minimum performance level"),
                    MaxPerformanceLevel = table.Column<double>(type: "float", nullable: true, comment: "The maximum performance level"),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "The color for the performance level"),
                    SortOrder = table.Column<int>(type: "int", nullable: true, comment: "A numeric value that represents the order in which the assessment item type should be displayed"),
                    PerformanceLevelText = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefPerformanceLevel", x => x.RefPerformanceLevelId);
                });

            migrationBuilder.CreateTable(
                name: "RefSchoolAdministrationType",
                columns: table => new
                {
                    RefSchoolAdministrationTypeId = table.Column<int>(type: "int", nullable: false, comment: "The unique identifier for each assessment item type in the table")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "A short code that represents the school administration type"),
                    SchoolAdministrationType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "The name of the assessment item type "),
                    SortOrder = table.Column<int>(type: "int", nullable: true, comment: "A numeric value that represents the order in which the assessment item type should be displayed")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefSchoolAdministrationType", x => x.RefSchoolAdministrationTypeId);
                });

            migrationBuilder.CreateTable(
                name: "RefSchoolLanguage",
                columns: table => new
                {
                    RefSchoolLanguageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "A short code that represents the school language"),
                    SchoolLanguage = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefSchoolLanguage", x => x.RefSchoolLanguageId);
                });

            migrationBuilder.CreateTable(
                name: "RefSchoolLocation",
                columns: table => new
                {
                    RefSchoolLocationId = table.Column<int>(type: "int", nullable: false, comment: "The foreign key identifier of the type of location of the school (i.e. Urban, Rural).")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "A short code that represents the school location"),
                    SchoolLocation = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefSchoolLocation", x => x.RefSchoolLocationId);
                });

            migrationBuilder.CreateTable(
                name: "RefSchoolStatus",
                columns: table => new
                {
                    RefSchoolStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "A short code that represents the school status"),
                    SchoolStatus = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefSchoolStatus", x => x.RefSchoolStatusId);
                });

            migrationBuilder.CreateTable(
                name: "RefSchoolType",
                columns: table => new
                {
                    RefSchoolTypeId = table.Column<int>(type: "int", nullable: false, comment: "The foreign key identifier of the type of education institution as classified by its primary focus (i.e. Primary, Secondary).")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "A short code that represents the school type"),
                    SchoolType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefSchoolType", x => x.RefSchoolTypeId);
                });

            migrationBuilder.CreateTable(
                name: "RefSex",
                columns: table => new
                {
                    RefSexId = table.Column<int>(type: "int", nullable: false, comment: "The foreign key identifier of the sex of the Person.")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sex = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SexId = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false, comment: "Abbreviated representation or code used to denote the sex of an individual, such as F for Female"),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefSex", x => x.RefSexId);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationRoleClaim",
                columns: table => new
                {
                    ClaimId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationRoleClaim", x => x.ClaimId);
                    table.ForeignKey(
                        name: "FK_ApplicationRoleClaim_ApplicationRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "ApplicationRole",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserClaim",
                columns: table => new
                {
                    ClaimId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserClaim", x => x.ClaimId);
                    table.ForeignKey(
                        name: "FK_ApplicationUserClaim_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserLogin",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserLogin", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_ApplicationUserLogin_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserRole",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserRole_ApplicationRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "ApplicationRole",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserRole_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserToken",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserToken", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_ApplicationUserToken_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefAcademicYear",
                columns: table => new
                {
                    RefAcademicYearId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AcademicYear = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "A short code that represents the academic year"),
                    Description = table.Column<string>(type: "nvarchar(384)", maxLength: 384, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date on which the academic year starts"),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date on which the academic year ends"),
                    RefAcademicYearStatusId = table.Column<int>(type: "int", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefAcademicYear", x => x.RefAcademicYearId);
                    table.ForeignKey(
                        name: "FK_RefAcademicYear_RefAcademicYearStatus_RefAcademicYearStatusId",
                        column: x => x.RefAcademicYearStatusId,
                        principalTable: "RefAcademicYearStatus",
                        principalColumn: "RefAcademicYearStatusId");
                });

            migrationBuilder.CreateTable(
                name: "Assessment",
                columns: table => new
                {
                    AssessmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Unique identifier for each assessment record in the table"),
                    RegistrationDate = table.Column<DateTime>(type: "date", nullable: false, comment: "Date on which the assessment was registered or added to the database"),
                    AssessmentName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "A short code that represents the assessment"),
                    RefAssessmentTypeId = table.Column<int>(type: "int", nullable: true, comment: "A reference to the type of assessment. This is a foreign key that references the RefAssessmentType table"),
                    RefAssessmentStatusId = table.Column<int>(type: "int", nullable: true, comment: "A reference to the status of assessment. This is a foreign key that references the RefAssessmentStatus table"),
                    RefGradeLevelId = table.Column<int>(type: "int", nullable: true, comment: "A reference to the grade level of the assessment. This is a foreign key that references the GradeLevel table"),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assessment", x => x.AssessmentId);
                    table.ForeignKey(
                        name: "FK_Assessment_RefAssessmentStatus_RefAssessmentStatusId",
                        column: x => x.RefAssessmentStatusId,
                        principalTable: "RefAssessmentStatus",
                        principalColumn: "RefAssessmentStatusId");
                    table.ForeignKey(
                        name: "FK_Assessment_RefAssessmentType_RefAssessmentTypeId",
                        column: x => x.RefAssessmentTypeId,
                        principalTable: "RefAssessmentType",
                        principalColumn: "RefAssessmentTypeId");
                    table.ForeignKey(
                        name: "FK_Assessment_RefGradeLevel_RefGradeLevelId",
                        column: x => x.RefGradeLevelId,
                        principalTable: "RefGradeLevel",
                        principalColumn: "RefGradeLevelId");
                });

            migrationBuilder.CreateTable(
                name: "GradebookPeriod",
                columns: table => new
                {
                    GradebookPeriodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RefGradebookPeriodTypeId = table.Column<int>(type: "int", nullable: true),
                    RegistrationDate = table.Column<DateTime>(type: "date", nullable: false, comment: "Date on which the gradebook period was registered or added to the database"),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "A short code that represents the gradebook period"),
                    GradebookPeriodName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RefGradeLevelId = table.Column<int>(type: "int", nullable: true, comment: "A reference to the grade level of the gradebook period. This is a foreign key that references the GradeLevel table"),
                    RefGradebookPeriodStatusId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradebookPeriod", x => x.GradebookPeriodId);
                    table.ForeignKey(
                        name: "FK_GradebookPeriod_RefGradeLevel_RefGradeLevelId",
                        column: x => x.RefGradeLevelId,
                        principalTable: "RefGradeLevel",
                        principalColumn: "RefGradeLevelId");
                    table.ForeignKey(
                        name: "FK_GradebookPeriod_RefGradebookPeriodStatus_RefGradebookPeriodStatusId",
                        column: x => x.RefGradebookPeriodStatusId,
                        principalTable: "RefGradebookPeriodStatus",
                        principalColumn: "RefGradebookPeriodStatusId");
                    table.ForeignKey(
                        name: "FK_GradebookPeriod_RefGradebookPeriodType_RefGradebookPeriodTypeId",
                        column: x => x.RefGradebookPeriodTypeId,
                        principalTable: "RefGradebookPeriodType",
                        principalColumn: "RefGradebookPeriodTypeId");
                });

            migrationBuilder.CreateTable(
                name: "RefAssessmentCategory",
                columns: table => new
                {
                    RefAssessmentCategoryId = table.Column<int>(type: "int", nullable: false, comment: "The unique identifier for each assessment category in the table")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "A short code that represents the assessment category"),
                    AssessmentCategory = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "The name of the assessment category"),
                    AssessmentCategoryDescription = table.Column<string>(type: "nvarchar(384)", maxLength: 384, nullable: true, comment: "The description of the assessment category"),
                    RefGradeLevelId = table.Column<int>(type: "int", nullable: true, comment: "The grade level for the assessment category"),
                    SortOrder = table.Column<int>(type: "int", nullable: true, comment: "A numeric value that represents the order in which the assessment categories should be displayed")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefAssessmentCategory", x => x.RefAssessmentCategoryId);
                    table.ForeignKey(
                        name: "FK_RefAssessmentCategory_RefGradeLevel_RefGradeLevelId",
                        column: x => x.RefGradeLevelId,
                        principalTable: "RefGradeLevel",
                        principalColumn: "RefGradeLevelId");
                });

            migrationBuilder.CreateTable(
                name: "RefLocation",
                columns: table => new
                {
                    RefLocationId = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false, comment: "A unique identifier for the geographic location record of the Organization"),
                    LocationName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, comment: "The name of the location"),
                    RefLocationTypeId = table.Column<int>(type: "int", nullable: true, comment: "A reference to the type of location (e.g., country, state, province, city, etc.)"),
                    ParentLocationId = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true, comment: "A reference to the parent location of this location."),
                    Latitude = table.Column<double>(type: "float", nullable: true, comment: "Latitude coordinates of the location in decimal degrees format"),
                    Longitude = table.Column<double>(type: "float", nullable: true, comment: "Longitude coordinates of the location in decimal degrees format"),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefLocation", x => x.RefLocationId);
                    table.ForeignKey(
                        name: "FK_RefLocation_RefLocationType_RefLocationTypeId",
                        column: x => x.RefLocationTypeId,
                        principalTable: "RefLocationType",
                        principalColumn: "RefLocationTypeId");
                    table.ForeignKey(
                        name: "FK_RefLocation_RefLocation_ParentLocationId",
                        column: x => x.ParentLocationId,
                        principalTable: "RefLocation",
                        principalColumn: "RefLocationId");
                });

            migrationBuilder.CreateTable(
                name: "GradebookAssessmentPeriod",
                columns: table => new
                {
                    GradebookAssessmentPeriodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GradebookPeriodId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "A short code that represents the gradebook assessment period"),
                    GradebookAssessmentPeriodName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    RefAssessmentTermId = table.Column<int>(type: "int", nullable: true),
                    RefAssessmentWeekId = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Date on which the period starts"),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Date on which the period ends"),
                    SortOrder = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradebookAssessmentPeriod", x => x.GradebookAssessmentPeriodId);
                    table.ForeignKey(
                        name: "FK_GradebookAssessmentPeriod_GradebookPeriod_GradebookPeriodId",
                        column: x => x.GradebookPeriodId,
                        principalTable: "GradebookPeriod",
                        principalColumn: "GradebookPeriodId");
                    table.ForeignKey(
                        name: "FK_GradebookAssessmentPeriod_RefAssessmentTerm_RefAssessmentTermId",
                        column: x => x.RefAssessmentTermId,
                        principalTable: "RefAssessmentTerm",
                        principalColumn: "RefAssessmentTermId");
                    table.ForeignKey(
                        name: "FK_GradebookAssessmentPeriod_RefAssessmentWeek_RefAssessmentWeekId",
                        column: x => x.RefAssessmentWeekId,
                        principalTable: "RefAssessmentWeek",
                        principalColumn: "RefAssessmentWeekId");
                });

            migrationBuilder.CreateTable(
                name: "AssessmentItem",
                columns: table => new
                {
                    AssessmentItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssessmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RefAssessmentCategoryId = table.Column<int>(type: "int", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    AssessmentItemNumber = table.Column<int>(type: "int", nullable: true),
                    AssessmentItemText = table.Column<string>(type: "nvarchar(384)", maxLength: 384, nullable: true),
                    AssessmentItemDescription = table.Column<string>(type: "nvarchar(384)", maxLength: 384, nullable: true),
                    MaximumScore = table.Column<double>(type: "float", nullable: true),
                    MinimumScore = table.Column<double>(type: "float", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentItem", x => x.AssessmentItemId);
                    table.ForeignKey(
                        name: "FK_AssessmentItem_Assessment_AssessmentId",
                        column: x => x.AssessmentId,
                        principalTable: "Assessment",
                        principalColumn: "AssessmentId");
                    table.ForeignKey(
                        name: "FK_AssessmentItem_RefAssessmentCategory_RefAssessmentCategoryId",
                        column: x => x.RefAssessmentCategoryId,
                        principalTable: "RefAssessmentCategory",
                        principalColumn: "RefAssessmentCategoryId");
                });

            migrationBuilder.CreateTable(
                name: "Organization",
                columns: table => new
                {
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Unique identifier for each organization, such as educational institutions or school districts"),
                    RegistrationDate = table.Column<DateTime>(type: "date", nullable: false, comment: "Date on which the organization was registered or added to the database"),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "A short code that represents the unique organization"),
                    OrganizationName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, comment: "The name of the organization"),
                    RefOrganizationTypeId = table.Column<int>(type: "int", nullable: true, comment: "A reference to the type of organization to which this entity belongs (e.g. school, district, etc.)"),
                    RefLocationId = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true, comment: "A reference to the geographic location of the organization"),
                    Address = table.Column<string>(type: "nvarchar(384)", maxLength: 384, nullable: true, comment: "The physical address of the organization, which could include street name, street number, zip code, etc"),
                    ParentOrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the parent organization of this organization, if any (e.g. a district could have multiple schools as its child organizations)"),
                    Latitude = table.Column<double>(type: "float", nullable: true, comment: "Latitude coordinates of the organization's location in decimal degrees format"),
                    Longitude = table.Column<double>(type: "float", nullable: true, comment: "Longitude coordinates of the organization's location in decimal degrees format"),
                    IsOrganizationUnit = table.Column<bool>(type: "bit", nullable: false, comment: "A Boolean value indicating whether this entity is a container"),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organization", x => x.OrganizationId);
                    table.ForeignKey(
                        name: "FK_Organization_Organization_ParentOrganizationId",
                        column: x => x.ParentOrganizationId,
                        principalTable: "Organization",
                        principalColumn: "OrganizationId");
                    table.ForeignKey(
                        name: "FK_Organization_RefLocation_RefLocationId",
                        column: x => x.RefLocationId,
                        principalTable: "RefLocation",
                        principalColumn: "RefLocationId");
                    table.ForeignKey(
                        name: "FK_Organization_RefOrganizationType_RefOrganizationTypeId",
                        column: x => x.RefOrganizationTypeId,
                        principalTable: "RefOrganizationType",
                        principalColumn: "RefOrganizationTypeId");
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserOrganization",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserOrganization", x => new { x.UserId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserOrganization_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserOrganization_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organization",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "School",
                columns: table => new
                {
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Unique identifier for each organization, such as educational institutions or school districts"),
                    SchoolCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "A short code that represents the unique school (such as the school EMIS number)"),
                    RefSchoolTypeId = table.Column<int>(type: "int", nullable: true, comment: "A reference to the type of school, such as primary, secondary, or vocational. This is a foreign key that references the SchoolType table"),
                    RefSchoolLocationId = table.Column<int>(type: "int", nullable: true, comment: "A reference to the location of the school, such as rural or urban. This is a foreign key that references the SchoolLocation table"),
                    RefSchoolAdministrationTypeId = table.Column<int>(type: "int", nullable: true, comment: "A reference to the type of administration for the school, such as private or public. This is a foreign key that references the SchoolAdministrationType table"),
                    RefSchoolLanguageId = table.Column<int>(type: "int", nullable: true, comment: "A reference to the language of instruction for the school. This is a foreign key that references the SchoolLanguage table"),
                    RefSchoolStatusId = table.Column<int>(type: "int", nullable: true, comment: "A reference to the status of the school, such as active or inactive. This is a foreign key that references the SchoolStatus table"),
                    HeadTeacher = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true, comment: "The name of the head teacher or principal of the school"),
                    OpeningDate = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "The date when the school was opened"),
                    ClosingDate = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "The date when the school was closed")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_School", x => x.OrganizationId);
                    table.ForeignKey(
                        name: "FK_School_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organization",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_School_RefSchoolAdministrationType_RefSchoolAdministrationTypeId",
                        column: x => x.RefSchoolAdministrationTypeId,
                        principalTable: "RefSchoolAdministrationType",
                        principalColumn: "RefSchoolAdministrationTypeId");
                    table.ForeignKey(
                        name: "FK_School_RefSchoolLanguage_RefSchoolLanguageId",
                        column: x => x.RefSchoolLanguageId,
                        principalTable: "RefSchoolLanguage",
                        principalColumn: "RefSchoolLanguageId");
                    table.ForeignKey(
                        name: "FK_School_RefSchoolLocation_RefSchoolLocationId",
                        column: x => x.RefSchoolLocationId,
                        principalTable: "RefSchoolLocation",
                        principalColumn: "RefSchoolLocationId");
                    table.ForeignKey(
                        name: "FK_School_RefSchoolStatus_RefSchoolStatusId",
                        column: x => x.RefSchoolStatusId,
                        principalTable: "RefSchoolStatus",
                        principalColumn: "RefSchoolStatusId");
                    table.ForeignKey(
                        name: "FK_School_RefSchoolType_RefSchoolTypeId",
                        column: x => x.RefSchoolTypeId,
                        principalTable: "RefSchoolType",
                        principalColumn: "RefSchoolTypeId");
                });

            migrationBuilder.CreateTable(
                name: "SchoolAcademicYear",
                columns: table => new
                {
                    SchoolAcademicYearId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Unique identifier for each academic year record"),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Unique identifier of the school that the academic year belongs to. This is a foreign key that references the School table"),
                    RefAcademicYearId = table.Column<int>(type: "int", nullable: false, comment: "A reference to the academic year, such as 2022, 2023, etc. This is a foreign key that references the RefAcademicYear table"),
                    RegistrationDate = table.Column<DateTime>(type: "date", nullable: false, comment: "Date on which the school academic year was registered or added to the database"),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "the starting date for the academic year"),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "The ending date for the academic year"),
                    RefAcademicYearStatusId = table.Column<int>(type: "int", nullable: true, comment: "A reference to the status of the academic year, such as started, not started, completed, or closed. This is a foreign key that references the RefAcademicYearStatus table"),
                    IsMissingEnrollment = table.Column<bool>(type: "bit", nullable: true, comment: "Indicates whether the academic year is missing enrollment data"),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolAcademicYear", x => x.SchoolAcademicYearId);
                    table.ForeignKey(
                        name: "FK_SchoolAcademicYear_RefAcademicYearStatus_RefAcademicYearStatusId",
                        column: x => x.RefAcademicYearStatusId,
                        principalTable: "RefAcademicYearStatus",
                        principalColumn: "RefAcademicYearStatusId");
                    table.ForeignKey(
                        name: "FK_SchoolAcademicYear_RefAcademicYear_RefAcademicYearId",
                        column: x => x.RefAcademicYearId,
                        principalTable: "RefAcademicYear",
                        principalColumn: "RefAcademicYearId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolAcademicYear_School_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "School",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Gradebook",
                columns: table => new
                {
                    GradebookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Unique identifier for each gradebook"),
                    SchoolAcademicYearId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Reference to the academic year to which the gradebook belongs. This is a foreign key that references the SchoolAcademicYear table"),
                    RefGradeLevelId = table.Column<int>(type: "int", nullable: false, comment: "A reference to the grade level of the gradebook. This is a foreign key that references the GradeLevel table"),
                    RegistrationDate = table.Column<DateTime>(type: "date", nullable: false, comment: "Date on which the gradebook was registered or added to the database"),
                    GradebookPeriodId = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "A reference to the default gradebook period for a grade school, such as term 1, term 2, or term 3. This is a foreign key that references the SchoolType table"),
                    AssessmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Reference to the default assessment form for a grade. It includes assessments and their associated items. This is a foreign key that references the Assessment table"),
                    RefAssessmentPlatformTypeId = table.Column<int>(type: "int", nullable: true, comment: "A reference to the type of platform for the assessment, such as web or android. This is a foreign key that references the RefAssessmentPlatformType table"),
                    RefGradebookStatusId = table.Column<int>(type: "int", nullable: true, comment: "A reference to the status of a gradebook, such as active or inactive. This is a foreign key that references the RefGradebookStatus table"),
                    IsMissingGradebookAssessments = table.Column<bool>(type: "bit", nullable: true, comment: "Indicates whether the gradebook is missing assessments"),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gradebook", x => x.GradebookId);
                    table.ForeignKey(
                        name: "FK_Gradebook_Assessment_AssessmentId",
                        column: x => x.AssessmentId,
                        principalTable: "Assessment",
                        principalColumn: "AssessmentId");
                    table.ForeignKey(
                        name: "FK_Gradebook_GradebookPeriod_GradebookPeriodId",
                        column: x => x.GradebookPeriodId,
                        principalTable: "GradebookPeriod",
                        principalColumn: "GradebookPeriodId");
                    table.ForeignKey(
                        name: "FK_Gradebook_RefAssessmentPlatformType_RefAssessmentPlatformTypeId",
                        column: x => x.RefAssessmentPlatformTypeId,
                        principalTable: "RefAssessmentPlatformType",
                        principalColumn: "RefAssessmentPlatformTypeId");
                    table.ForeignKey(
                        name: "FK_Gradebook_RefGradeLevel_RefGradeLevelId",
                        column: x => x.RefGradeLevelId,
                        principalTable: "RefGradeLevel",
                        principalColumn: "RefGradeLevelId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Gradebook_RefGradebookStatus_RefGradebookStatusId",
                        column: x => x.RefGradebookStatusId,
                        principalTable: "RefGradebookStatus",
                        principalColumn: "RefGradebookStatusId");
                    table.ForeignKey(
                        name: "FK_Gradebook_SchoolAcademicYear_SchoolAcademicYearId",
                        column: x => x.SchoolAcademicYearId,
                        principalTable: "SchoolAcademicYear",
                        principalColumn: "SchoolAcademicYearId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GradebookAssessment",
                columns: table => new
                {
                    GradebookAssessmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Unique identifier for each gradebook assessment record in the table"),
                    GradebookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Reference to the gradebook to which the assessment aggregate belongs to. This is a foreign key that references the Gradebook table"),
                    GradebookAssessmentPeriodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "A reference to the gradebook assessment period of the gradebook. This is a foreign key that references the GradebookAssessmentPeriod table"),
                    RegistrationDate = table.Column<DateTime>(type: "date", nullable: false, comment: "Date on which the gradebook assessment was registered or added to the database"),
                    AssessedFemale = table.Column<int>(type: "int", nullable: false),
                    AssessedMale = table.Column<int>(type: "int", nullable: false),
                    RefGradebookAssessmentStatusId = table.Column<int>(type: "int", nullable: true, comment: "A reference to the status of the gradebook, such as active or inactive. This is a foreign key that references the RefGradebookStatus table"),
                    IsMissingAssessmentResults = table.Column<bool>(type: "bit", nullable: true, comment: "Indicates whether the assessment results are missing or not"),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradebookAssessment", x => x.GradebookAssessmentId);
                    table.ForeignKey(
                        name: "FK_GradebookAssessment_GradebookAssessmentPeriod_GradebookAssessmentPeriodId",
                        column: x => x.GradebookAssessmentPeriodId,
                        principalTable: "GradebookAssessmentPeriod",
                        principalColumn: "GradebookAssessmentPeriodId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GradebookAssessment_Gradebook_GradebookId",
                        column: x => x.GradebookId,
                        principalTable: "Gradebook",
                        principalColumn: "GradebookId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GradebookAssessment_RefGradebookAssessmentStatus_RefGradebookAssessmentStatusId",
                        column: x => x.RefGradebookAssessmentStatusId,
                        principalTable: "RefGradebookAssessmentStatus",
                        principalColumn: "RefGradebookAssessmentStatusId");
                });

            migrationBuilder.CreateTable(
                name: "GradebookEnrollment",
                columns: table => new
                {
                    GradebookEnrollmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Unique identifier for each gradebook enrollment record"),
                    GradebookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "A reference to the gradebook where the enrollment was registered. This is a foreign key that references the Gradebook table"),
                    RegistrationDate = table.Column<DateTime>(type: "date", nullable: false, comment: "Date on which the gradebook enrollment was registered or added to the database"),
                    RefParticipantTypeId = table.Column<int>(type: "int", nullable: false, comment: "A reference to the type of participant (e.g. learner, teacher) enrolled. This is a foreign key that references the ParticipantType table"),
                    Male = table.Column<int>(type: "int", nullable: false, comment: "The number of male participants enrolled in the school at the specified grade level"),
                    Female = table.Column<int>(type: "int", nullable: false, comment: "The number of female participants enrolled in the school at the specified grade level"),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradebookEnrollment", x => x.GradebookEnrollmentId);
                    table.ForeignKey(
                        name: "FK_GradebookEnrollment_Gradebook_GradebookId",
                        column: x => x.GradebookId,
                        principalTable: "Gradebook",
                        principalColumn: "GradebookId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GradebookEnrollment_RefParticipantType_RefParticipantTypeId",
                        column: x => x.RefParticipantTypeId,
                        principalTable: "RefParticipantType",
                        principalColumn: "RefParticipantTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssessmentPerformanceLevel",
                columns: table => new
                {
                    AssessmentPerformanceLevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Unique identifier for each assessment performance level record in the table"),
                    GradebookAssessmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Reference to the gradebook assessment to which the assessment performance level belongs to. This is a foreign key that references the GradebookAssessment table"),
                    RegistrationDate = table.Column<DateTime>(type: "date", nullable: false, comment: "Date on which the assessment performance level was registered or added to the database"),
                    RefPerformanceLevelId = table.Column<int>(type: "int", nullable: false, comment: "A reference to the performance level of an assessment, such as minimum, desirable, or outstanding. This is a foreign key that references the RefPerformanceLevel table"),
                    RefSexId = table.Column<int>(type: "int", nullable: false, comment: "A reference to the sex of a learner or teacher, such as male, or female. This is a foreign key that references the RefSex table"),
                    Score = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentPerformanceLevel", x => x.AssessmentPerformanceLevelId);
                    table.ForeignKey(
                        name: "FK_AssessmentPerformanceLevel_GradebookAssessment_GradebookAssessmentId",
                        column: x => x.GradebookAssessmentId,
                        principalTable: "GradebookAssessment",
                        principalColumn: "GradebookAssessmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssessmentPerformanceLevel_RefPerformanceLevel_RefPerformanceLevelId",
                        column: x => x.RefPerformanceLevelId,
                        principalTable: "RefPerformanceLevel",
                        principalColumn: "RefPerformanceLevelId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssessmentPerformanceLevel_RefSex_RefSexId",
                        column: x => x.RefSexId,
                        principalTable: "RefSex",
                        principalColumn: "RefSexId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssessmentResult",
                columns: table => new
                {
                    AssessmentResultId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Unique identifier for each assessment result record in the table"),
                    GradebookAssessmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Reference to the gradebook assessment of a grade, such as grade 1 term 1 week 5. This is a foreign key that references the GradebookAssessment table"),
                    RegistrationDate = table.Column<DateTime>(type: "date", nullable: false, comment: "Date on which the assessment result was registered or added to the database"),
                    AssessmentItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "A reference to the item of an assessment category. It represents the specific question items associated with a particular category, such as the fuency category, within a specific grade level. This is a foreign key that references the RefAssessmentItem table"),
                    ScoreFemale = table.Column<int>(type: "int", nullable: true, comment: "Represents the count of accurate responses provided by female participants"),
                    ScoreMale = table.Column<int>(type: "int", nullable: true, comment: "Represents the count of accurate responses provided by male participants"),
                    Score = table.Column<int>(type: "int", nullable: true, comment: "The cumulative number of correct responses from all participants"),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentResult", x => x.AssessmentResultId);
                    table.ForeignKey(
                        name: "FK_AssessmentResult_AssessmentItem_AssessmentItemId",
                        column: x => x.AssessmentItemId,
                        principalTable: "AssessmentItem",
                        principalColumn: "AssessmentItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssessmentResult_GradebookAssessment_GradebookAssessmentId",
                        column: x => x.GradebookAssessmentId,
                        principalTable: "GradebookAssessment",
                        principalColumn: "GradebookAssessmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "ApplicationRole",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationRoleClaim_RoleId",
                table: "ApplicationRoleClaim",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "ApplicationUser",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "ApplicationUser",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserClaim_UserId",
                table: "ApplicationUserClaim",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserLogin_UserId",
                table: "ApplicationUserLogin",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserOrganization_OrganizationId",
                table: "ApplicationUserOrganization",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserRole_RoleId",
                table: "ApplicationUserRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Assessment_RefAssessmentStatusId",
                table: "Assessment",
                column: "RefAssessmentStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Assessment_RefAssessmentTypeId",
                table: "Assessment",
                column: "RefAssessmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Assessment_RefGradeLevelId",
                table: "Assessment",
                column: "RefGradeLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentItem_AssessmentId",
                table: "AssessmentItem",
                column: "AssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentItem_RefAssessmentCategoryId",
                table: "AssessmentItem",
                column: "RefAssessmentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentPerformanceLevel_GradebookAssessmentId_RefPerformanceLevelId_RefSexId",
                table: "AssessmentPerformanceLevel",
                columns: new[] { "GradebookAssessmentId", "RefPerformanceLevelId", "RefSexId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentPerformanceLevel_RefPerformanceLevelId",
                table: "AssessmentPerformanceLevel",
                column: "RefPerformanceLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentPerformanceLevel_RefSexId",
                table: "AssessmentPerformanceLevel",
                column: "RefSexId");

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentResult_AssessmentItemId",
                table: "AssessmentResult",
                column: "AssessmentItemId");

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentResult_GradebookAssessmentId_AssessmentItemId",
                table: "AssessmentResult",
                columns: new[] { "GradebookAssessmentId", "AssessmentItemId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Gradebook_AssessmentId",
                table: "Gradebook",
                column: "AssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Gradebook_GradebookPeriodId",
                table: "Gradebook",
                column: "GradebookPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_Gradebook_RefAssessmentPlatformTypeId",
                table: "Gradebook",
                column: "RefAssessmentPlatformTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Gradebook_RefGradebookStatusId",
                table: "Gradebook",
                column: "RefGradebookStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Gradebook_RefGradeLevelId",
                table: "Gradebook",
                column: "RefGradeLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Gradebook_SchoolAcademicYearId_RefGradeLevelId",
                table: "Gradebook",
                columns: new[] { "SchoolAcademicYearId", "RefGradeLevelId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GradebookAssessment_GradebookAssessmentPeriodId",
                table: "GradebookAssessment",
                column: "GradebookAssessmentPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_GradebookAssessment_GradebookId_GradebookAssessmentPeriodId",
                table: "GradebookAssessment",
                columns: new[] { "GradebookId", "GradebookAssessmentPeriodId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GradebookAssessment_RefGradebookAssessmentStatusId",
                table: "GradebookAssessment",
                column: "RefGradebookAssessmentStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_GradebookAssessmentPeriod_GradebookPeriodId",
                table: "GradebookAssessmentPeriod",
                column: "GradebookPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_GradebookAssessmentPeriod_RefAssessmentTermId",
                table: "GradebookAssessmentPeriod",
                column: "RefAssessmentTermId");

            migrationBuilder.CreateIndex(
                name: "IX_GradebookAssessmentPeriod_RefAssessmentWeekId",
                table: "GradebookAssessmentPeriod",
                column: "RefAssessmentWeekId");

            migrationBuilder.CreateIndex(
                name: "IX_GradebookEnrollment_GradebookId_RefParticipantTypeId",
                table: "GradebookEnrollment",
                columns: new[] { "GradebookId", "RefParticipantTypeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GradebookEnrollment_RefParticipantTypeId",
                table: "GradebookEnrollment",
                column: "RefParticipantTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GradebookPeriod_RefGradebookPeriodStatusId",
                table: "GradebookPeriod",
                column: "RefGradebookPeriodStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_GradebookPeriod_RefGradebookPeriodTypeId",
                table: "GradebookPeriod",
                column: "RefGradebookPeriodTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GradebookPeriod_RefGradeLevelId",
                table: "GradebookPeriod",
                column: "RefGradeLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Organization_ParentOrganizationId",
                table: "Organization",
                column: "ParentOrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Organization_RefLocationId",
                table: "Organization",
                column: "RefLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Organization_RefOrganizationTypeId",
                table: "Organization",
                column: "RefOrganizationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RefAcademicYear_RefAcademicYearStatusId",
                table: "RefAcademicYear",
                column: "RefAcademicYearStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_RefAssessmentCategory_RefGradeLevelId",
                table: "RefAssessmentCategory",
                column: "RefGradeLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_RefLocation_ParentLocationId",
                table: "RefLocation",
                column: "ParentLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_RefLocation_RefLocationTypeId",
                table: "RefLocation",
                column: "RefLocationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_School_RefSchoolAdministrationTypeId",
                table: "School",
                column: "RefSchoolAdministrationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_School_RefSchoolLanguageId",
                table: "School",
                column: "RefSchoolLanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_School_RefSchoolLocationId",
                table: "School",
                column: "RefSchoolLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_School_RefSchoolStatusId",
                table: "School",
                column: "RefSchoolStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_School_RefSchoolTypeId",
                table: "School",
                column: "RefSchoolTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolAcademicYear_OrganizationId_RefAcademicYearId",
                table: "SchoolAcademicYear",
                columns: new[] { "OrganizationId", "RefAcademicYearId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SchoolAcademicYear_RefAcademicYearId",
                table: "SchoolAcademicYear",
                column: "RefAcademicYearId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolAcademicYear_RefAcademicYearStatusId",
                table: "SchoolAcademicYear",
                column: "RefAcademicYearStatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationLogs");

            migrationBuilder.DropTable(
                name: "ApplicationRoleClaim");

            migrationBuilder.DropTable(
                name: "ApplicationUserClaim");

            migrationBuilder.DropTable(
                name: "ApplicationUserLogin");

            migrationBuilder.DropTable(
                name: "ApplicationUserOrganization");

            migrationBuilder.DropTable(
                name: "ApplicationUserRole");

            migrationBuilder.DropTable(
                name: "ApplicationUserToken");

            migrationBuilder.DropTable(
                name: "AssessmentPerformanceLevel");

            migrationBuilder.DropTable(
                name: "AssessmentResult");

            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "GradebookEnrollment");

            migrationBuilder.DropTable(
                name: "ApplicationRole");

            migrationBuilder.DropTable(
                name: "ApplicationUser");

            migrationBuilder.DropTable(
                name: "RefPerformanceLevel");

            migrationBuilder.DropTable(
                name: "RefSex");

            migrationBuilder.DropTable(
                name: "AssessmentItem");

            migrationBuilder.DropTable(
                name: "GradebookAssessment");

            migrationBuilder.DropTable(
                name: "RefParticipantType");

            migrationBuilder.DropTable(
                name: "RefAssessmentCategory");

            migrationBuilder.DropTable(
                name: "GradebookAssessmentPeriod");

            migrationBuilder.DropTable(
                name: "Gradebook");

            migrationBuilder.DropTable(
                name: "RefGradebookAssessmentStatus");

            migrationBuilder.DropTable(
                name: "RefAssessmentTerm");

            migrationBuilder.DropTable(
                name: "RefAssessmentWeek");

            migrationBuilder.DropTable(
                name: "Assessment");

            migrationBuilder.DropTable(
                name: "GradebookPeriod");

            migrationBuilder.DropTable(
                name: "RefAssessmentPlatformType");

            migrationBuilder.DropTable(
                name: "RefGradebookStatus");

            migrationBuilder.DropTable(
                name: "SchoolAcademicYear");

            migrationBuilder.DropTable(
                name: "RefAssessmentStatus");

            migrationBuilder.DropTable(
                name: "RefAssessmentType");

            migrationBuilder.DropTable(
                name: "RefGradeLevel");

            migrationBuilder.DropTable(
                name: "RefGradebookPeriodStatus");

            migrationBuilder.DropTable(
                name: "RefGradebookPeriodType");

            migrationBuilder.DropTable(
                name: "RefAcademicYear");

            migrationBuilder.DropTable(
                name: "School");

            migrationBuilder.DropTable(
                name: "RefAcademicYearStatus");

            migrationBuilder.DropTable(
                name: "Organization");

            migrationBuilder.DropTable(
                name: "RefSchoolAdministrationType");

            migrationBuilder.DropTable(
                name: "RefSchoolLanguage");

            migrationBuilder.DropTable(
                name: "RefSchoolLocation");

            migrationBuilder.DropTable(
                name: "RefSchoolStatus");

            migrationBuilder.DropTable(
                name: "RefSchoolType");

            migrationBuilder.DropTable(
                name: "RefLocation");

            migrationBuilder.DropTable(
                name: "RefOrganizationType");

            migrationBuilder.DropTable(
                name: "RefLocationType");
        }
    }
}
