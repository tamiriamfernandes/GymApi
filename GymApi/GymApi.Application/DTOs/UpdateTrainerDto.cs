using System;
using System.Collections.Generic;
using System.Text;

namespace GymApi.Application.DTOs
{
    public class UpdateTrainerDto
    {

        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? RegistrationNumber { get; private set; } // ex: CREF
    }
}
