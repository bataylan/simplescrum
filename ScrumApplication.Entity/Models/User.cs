﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumApplication.Entity.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [Display(Name = "User name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "User mail")]
        public string Mail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        public int TotalPoint { get; set; }

        public virtual List<Member> Members { get; set; }
        public virtual List<Manager> Managers { get; set; }
    }
}
