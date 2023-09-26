using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Identity
{
    // Represents the ApplicationRole entity.
    [Table("ApplicationRole")]
    public class ApplicationRole : IdentityRole<Guid>
    {
        // Description of the role.
        [Display(Name = "Description")]
        [MaxLength(250)]
        [Column(Order = 3)]
        public string? Description { get; set; }
    }

}

