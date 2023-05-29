using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Enrollments
{
    [Table("SchoolEnrollment")]
    public class SchoolEnrollment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "School Enrollment")]
        [Comment("Unique identifier for each school enrollment record")]
        [Column(Order = 1)]
        public Guid SchoolEnrollmentId { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Registration Date", Prompt = "Enter the registration date")]
        [Comment("Date on which the schooll enrollment was registered or added to the database")]
        [Column(Order = 2)]
        public DateTime RegistrationDate { get; set; }

        [Display(Name = "Academic Year", Prompt = "Select the academic year")]
        [Comment("A reference to the academic year in which the enrollment was made. This is a foreign key that references the AcademicYear table")]
        [Column(Order = 3)]
        public int? AcademicYearId { get; set; }

        [Display(Name = "Organization", Prompt = "Select the organization name")]
        [Comment("Reference to the school to which the enrollment belongs to. This is a foreign key that references the School table")]
        [Column(Order = 4)]
        public Guid? OrganizationId { get; set; }

        [Display(Name = "Participant Type")]
        [Comment("A reference to the type of participant (e.g. learner, teacher) enrolled. This is a foreign key that references the ParticipantType table")]
        [Column(Order = 5)]
        public int? RefParticipantTypeId { get; set; }

        [Display(Name = "Grade Level", Prompt = "Select the grade level")]
        [Comment("A reference to the grade level of the participants enrolled. This is a foreign key that references the GradeLevel table")]
        [Column(Order = 6)]
        public int? RefGradeLevelId { get; set; }

        [Display(Name = "Male", Prompt = "Enter the number of males")]
        [Comment("The number of male participants enrolled in the school at the specified grade level")]
        [Column(Order = 7)]
        public int Male { get; set; }

        [Display(Name = "Female", Prompt = "Enter the number of females")]
        [Comment("The number of female participants enrolled in the school at the specified grade level")]
        [Column(Order = 8)]
        public int Female { get; set; }


    }
}

