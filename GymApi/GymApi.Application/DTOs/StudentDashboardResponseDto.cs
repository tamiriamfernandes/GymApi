namespace GymApi.Application.DTOs;

public class StudentDashboardResponseDto
{
    public int TotalCompletedWorkouts { get; set; }
    public int CurrentWeekWorkouts { get; set; }
    public double AverageWorkoutDuration { get; set; }
    public DateTime? LastWorkoutDate { get; set; }
    public int CurrentStreak { get; set; }
}
