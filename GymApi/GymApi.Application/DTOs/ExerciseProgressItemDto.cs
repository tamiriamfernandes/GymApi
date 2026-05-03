namespace GymApi.Application.DTOs;

public class ExerciseProgressItemDto
{
    public DateTime ExecutionDate { get; set; }
    public decimal? PerformedWeight { get; set; }
    public int? PerformedRepetitions { get; set; }
    public int? PerformedSets { get; set; }
}
