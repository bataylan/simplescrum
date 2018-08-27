using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumApplication.Entity.Models
{
    public class AccountViewModel
    {
        public User User { get; set; }

        [Display(Name = "Current Pasword")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Display(Name = "New Pasword")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Display(Name = "Confirm New Pasword")]
        [DataType(DataType.Password)]
        public string ConfirmNewPassword { get; set; }

    }
}
