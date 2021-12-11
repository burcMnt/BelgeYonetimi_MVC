﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BelgeYonetimi.Data
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage ="FirstName is Required"),MaxLength(50)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is Required"), MaxLength(50)]
        public string LastName { get; set; }
        public DateTime RegistrationTime { get; set; } = DateTime.Now;

        public List<UserRequest> UserRequests { get; set; }
    }
}
