using ePTS.Entities.Gradebooks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Reference
{
    [Table("RefGradebookStatus")]
    public class RefGradebookStatus
    {
        public RefGradebookStatus()
        {
            Gradebooks = new HashSet<Gradebook>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Gradebook Status")]
        [Comment("The unique identifier for each assessment item type in the table")]
        [Column(Order = 1)]
        public int RefGradebookStatusId { get; set; }

        [Display(Name = "Gradebook Status Code")]
        [MaxLength(100)]
        [Comment("A short code that represents the gradebook status")]
        [Column(Order = 2)]
        public string? Code { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Gradebook Status", Prompt = "Enter the gradebook status")]
        [MaxLength(150)]
        [Comment("The name of the assessment item type ")]
        [Column(Order = 3)]
        public string? GradebookStatus { get; set; } = null!;

        [Display(Name = "Sort Order", Prompt = "Enter the sort order")]
        [Comment("A numeric value that represents the order in which the assessment item type should be displayed")]
        [Column(Order = 4)]
        public int? SortOrder { get; set; }

        public virtual ICollection<Gradebook> Gradebooks { get; set; }

    }
}

