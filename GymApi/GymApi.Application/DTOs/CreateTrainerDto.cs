using System;
using System.Collections.Generic;
using System.Text;

namespace GymApi.Application.DTOs
{
    public class CreateTrainerDto
    {
        public string Name { get; set; } = default!;
        public string? RegistrationNumber { get; private set; } // ex: CREF
    }
}
