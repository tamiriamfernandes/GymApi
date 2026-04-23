using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GymApi.Application.DTOs
{
    public class CreateTrainerDto
    {
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; } = default!;
        public string? RegistrationNumber { get; private set; } // ex: CREF
    }
}
