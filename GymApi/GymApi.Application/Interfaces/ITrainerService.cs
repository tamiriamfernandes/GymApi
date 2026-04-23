using GymApi.Application.DTOs;
using GymApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymApi.Application.Interfaces
{
    public interface ITrainerService
    {
        Task<Guid> CreateAsync(CreateTrainerDto dto);
        Task<(List<Trainer> Items, int TotalCount)> GetPagedAsync(int page, int pageSize);
    }
}
