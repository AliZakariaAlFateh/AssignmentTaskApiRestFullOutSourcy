using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Dtos
{
    public class CreateOrderDto
    {
        //public int CustomerId { get; set; }
        //public List<int> ProductIds { get; set; } = new();
        [Required(ErrorMessage = "Customer ID is required.")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "At least one product is required.")]
        [MinLength(1, ErrorMessage = "At least one product is required.")]
        public List<int> ProductIds { get; set; } = new();
    }
}
