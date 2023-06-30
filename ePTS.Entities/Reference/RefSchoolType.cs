using ePTS.Entities.Core;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Reference
{
    [Table("RefSchoolType")]
    public class RefSchoolType
    {
        public RefSchoolType()
        {
            Schools = new HashSet<School>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "School Type")]
        [Comment("The foreign key identifier of the type of education institution as classified by its primary focus (i.e. Primary, Secondary).")]
        [Column(Order = 1)]
        public int RefSchoolTypeId { get; set; }

        [Display(Name = "School Type Code")]
        [MaxLength(100)]
        [Comment("A short code that represents the school type")]
        [Column(Order = 2)]
        public string? Code { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "School Type", Prompt = "Enter the school type")]
        [MaxLength(150)]
        [Column(Order = 3)]
        public string? SchoolType { get; set; } = null!;

        [Display(Name = "Sort Order", Prompt = "Enter the sort order")]
        [Column(Order = 4)]
        public int? SortOrder { get; set; }

        public virtual ICollection<School> Schools { get; set; }

    }
}

