using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Gradebooks
{
    // Represents a gradebook period entity.
    [Table("GradebookPeriod")]
    [Comment("Represents a gradebook period entity.")]
    public class GradebookPeriod : BaseEntity
    {
        public GradebookPeriod()
        {
            Gradebooks = new HashSet<Gradebook>();
            GradebookAssessmentPeriods = new HashSet<GradebookAssessmentPeriod>();
        }

        // Unique identifier for each gradebook period.
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Gradebook Period")]
        [Comment("Unique identifier for each gradebook period.")]
        [Column(Order = 1)]
        public Guid GradebookPeriodId { get; set; }

        // Reference to the gradebook period type.
        [Display(Name = "Gradebook Period Type")]
        [Column(Order = 2)]
        public int? RefGradebookPeriodTypeId { get; set; }

        // Registration date of the gradebook period.
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Registration Date", Prompt = "Enter the registration date")]
        [Comment("Date on which the gradebook period was registered or added to the database.")]
        [DataType(DataType.Date)]
        [Column(Order = 3)]
        public DateTime RegistrationDate { get; set; }

        // Short code that represents the gradebook period.
        [Display(Name = "Gradebook Period Code")]
        [MaxLength(100)]
        [Comment("A short code that represents the gradebook period.")]
        [Column(Order = 4)]
        public string? Code { get; set; }

        // Name of the gradebook period.
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Gradebook Period")]
        [MaxLength(100)]
        [Column(Order = 5)]
        public string? GradebookPeriodName { get; set; }

        // Reference to the grade level of the gradebook period.
        [Display(Name = "Grade Level")]
        [Comment("A reference to the grade level of the gradebook period. This is a foreign key that references the GradeLevel table.")]
        [Column(Order = 6)]
        public int? RefGradeLevelId { get; set; }

        // Reference to the status of the gradebook period.
        [Display(Name = "Gradebook Period Status")]
        [Column(Order = 7)]
        public int? RefGradebookPeriodStatusId { get; set; }

        // Navigation property referencing the Gradebook entity.
        public virtual ICollection<Gradebook> Gradebooks { get; set; }

        // Navigation property referencing the GradebookAssessmentPeriod entity.
        public virtual ICollection<GradebookAssessmentPeriod> GradebookAssessmentPeriods { get; set; }
    }
}

