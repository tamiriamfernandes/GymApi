namespace GymApi.Application.DTOs;

public class StudentFrequencyResponseDto
{
    public Guid StudentId { get; set; }
    public DateTime WeekStart { get; set; }
    public DateTime WeekEnd { get; set; }
    public int TotalWorkouts { get; set; }
    public int CompletedWorkouts { get; set; }
}
