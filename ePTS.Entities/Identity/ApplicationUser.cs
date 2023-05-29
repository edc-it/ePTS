using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Identity
{
    [Table("ApplicationUser")]
    public class ApplicationUser : IdentityUser<Guid>
    {
        [Display(Name = "First Name")]
        [MaxLength(70)]
        [Comment("The first name or given name of a user")]
        [Column(Order = 4)]
        public string? FirstName { get; set; }

        [Display(Name = "Surname/Last Name")]
        [MaxLength(70)]
        [Comment("The last name or family name of a user")]
        [Column(Order = 5)]
        public string? LastName { get; set; }

        [Display(Name = "Created By")]
        [MaxLength(50)]
        [Comment("The user or entity responsible for creating or adding the record")]
        [Column(Order = 6)]
        public string? CreatedBy { get; set; }

        [Display(Name = "Created Date")]
        [Comment("The date and time when the record was created")]
        [Column(Order = 7)]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Modified By")]
        [MaxLength(50)]
        [Comment("The user or entity responsible for modifying the record")]
        [Column(Order = 8)]
        public string? ModifiedBy { get; set; }

        [Display(Name = "Modified Date")]
        [Comment("The date and time when the record was last modified")]
        [Column(Order = 9)]
        public DateTime ModifiedDate { get; set; }

        [Display(Name = "Deleted By")]
        [MaxLength(50)]
        [Comment("The user or entity responsible for marking the record as deleted")]
        [Column(Order = 10)]
        public string? DeletedBy { get; set; }

        [Display(Name = "Deleted Date")]
        [Comment("The date and time when the record was marked as deleted")]
        [Column(Order = 11)]
        public DateTime DeletedDate { get; set; }

        [Display(Name = "Is Deleted?")]
        [Comment("A flag indicating whether the record is marked as deleted (true) or active (false)")]
        [Column(Order = 12)]
        public bool IsDeleted { get; set; }


    }
}

