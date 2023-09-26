using ePTS.Entities.Reference;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Gradebooks
{
    // Represents a gradebook enrollment record.
    [Table("GradebookEnrollment")]
    [Comment("Represents a gradebook enrollment record.")]
    public class GradebookEnrollment : BaseEntity
    {
        // Unique identifier for each gradebook enrollment record.
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Gradebook Enrollment")]
        [Comment("Unique identifier for each gradebook enrollment record.")]
        [Column(Order = 1)]
        public Guid GradebookEnrollmentId { get; set; }

        // Reference to the gradebook where the enrollment was registered.
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Gradebook", Prompt = "Select the Gradebook")]
        [Comment("A reference to the gradebook where the enrollment was registered. This is a foreign key that references the Gradebook table.")]
        [Column(Order = 2)]
        public Guid GradebookId { get; set; }

        // Date on which the gradebook enrollment was registered or added to the database.
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Registration Date", Prompt = "Enter the registration date")]
        [Comment("Date on which the gradebook enrollment was registered or added to the database.")]
        [DataType(DataType.Date)]
        [Column(Order = 3)]
        public DateTime RegistrationDate { get; set; }

        // Reference to the type of participant (e.g., learner, teacher) enrolled.
        [Display(Name = "Participant Type")]
        [Comment("A reference to the type of participant (e.g., learner, teacher) enrolled. This is a foreign key that references the ParticipantType table.")]
        [Column(Order = 6)]
        public int RefParticipantTypeId { get; set; }

        // The number of male participants enrolled in the school at the specified grade level.
        [Display(Name = "Male", Prompt = "Enter the number of males enrolled")]
        [Comment("The number of male participants enrolled in the school at the specified grade level.")]
        [Column(Order = 7)]
        public int Male { get; set; }

        // The number of female participants enrolled in the school at the specified grade level.
        [Display(Name = "Female", Prompt = "Enter the number of females enrolled")]
        [Comment("The number of female participants enrolled in the school at the specified grade level.")]
        [Column(Order = 8)]
        public int Female { get; set; }

        // Navigation property referencing the Gradebook entity.
        public virtual Gradebook? Gradebooks { get; set; }

        // Navigation property referencing the RefParticipantType entity.
        public virtual RefParticipantType? ParticipantTypes { get; set; }
    }
}

