using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BelgeYonetimi.Data
{
    public class UserRequest
    {
        public int Id { get; set; }

        [Required,MaxLength(50)]
        public string UserName { get; set; }

        [Required, MaxLength(50)]
        public string UserLastName { get; set; }
        public string Explanation { get; set; }

        [Required]
        public string Document { get; set; }
        public string DocumentName { get; set; }
        public bool ConsiderationStatus { get; set; }
        public DateTime TimeOfConsideration { get; set; }
        public DateTime CreationTime { get; set; } = DateTime.Now;


        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
