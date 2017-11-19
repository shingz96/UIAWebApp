using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UIAWebApp.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
