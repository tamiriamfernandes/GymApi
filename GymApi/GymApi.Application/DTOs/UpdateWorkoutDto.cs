using System;
using System.Collections.Generic;
using System.Text;

namespace GymApi.Application.DTOs;

public class UpdateWorkoutDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid TrainerId { get; set; }
    public Guid StudentId { get; set; }
}
