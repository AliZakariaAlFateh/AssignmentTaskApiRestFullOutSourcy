using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Dtos
{
    public class CreateCustomerDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = default!;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email must be a valid email address")]
        public string Email { get; set; } = default!;

        public string Phone { get; set; } = default!;
    }
}
