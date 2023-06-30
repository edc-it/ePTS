using ePTS.Entities.Reference;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Gradebooks
{
    [Table("GradebookEnrollment")]
    public class GradebookEnrollment : BaseEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Gradebook Enrollment")]
        [Comment("Unique identifier for each gradebook enrollment record")]
        [Column(Order = 1)]
        public Guid GradebookEnrollmentId { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Gradebook", Prompt = "Select the Gradebook")]
        [Comment("A reference to the gradebook where the enrollment was registered. This is a foreign key that references the Gradebook table")]
        [Column(Order = 2)]
        public Guid GradebookId { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Registration Date", Prompt = "Enter the registration date")]
        [Comment("Date on which the gradebook enrollment was registered or added to the database")]
        [DataType(DataType.Date)]
        [Column(Order = 3)]
        public DateTime RegistrationDate { get; set; }

        [Display(Name = "Participant Type")]
        [Comment("A reference to the type of participant (e.g. learner, teacher) enrolled. This is a foreign key that references the ParticipantType table")]
        [Column(Order = 6)]
        public int RefParticipantTypeId { get; set; }

        [Display(Name = "Male", Prompt = "Enter the number of males enrolled")]
        [Comment("The number of male participants enrolled in the school at the specified grade level")]
        [Column(Order = 7)]
        public int Male { get; set; }

        [Display(Name = "Female", Prompt = "Enter the number of females enrolled")]
        [Comment("The number of female participants enrolled in the school at the specified grade level")]
        [Column(Order = 8)]
        public int Female { get; set; }

        // Foreign keys
        [ForeignKey("GradebookId")]
        [Display(Name = "Gradebook")]
        public virtual Gradebook? Gradebooks { get; set; }

        [ForeignKey("RefParticipantTypeId")]
        [Display(Name = "ParticipantTypes")]
        public virtual RefParticipantType? ParticipantTypes { get; set; }

    }
}

