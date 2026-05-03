using GymApi.Application.DTOs;

namespace GymApi.Application.Interfaces;

public interface IProgressService
{
    Task<IEnumerable<WorkoutHistoryResponseDto>> GetStudentWorkoutHistoryAsync(Guid studentId);

    Task<ExerciseProgressResponseDto?> GetExerciseProgressAsync(Guid studentId, Guid exerciseId);

    Task<StudentFrequencyResponseDto> GetWeeklyFrequencyAsync(Guid studentId);

    Task<StudentDashboardResponseDto> GetDashboardAsync(Guid studentId);
}
