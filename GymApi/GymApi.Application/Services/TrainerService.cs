using GymApi.Application.DTOs;
using GymApi.Application.Interfaces;
using GymApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymApi.Application.Services
{
    public class TrainerService : ITrainerService
    {
        private readonly IRepository<Trainer> _repository;
        public TrainerService(IRepository<Trainer> repository)
        {
            _repository = repository;
        }

        public async Task<Guid> CreateAsync(CreateTrainerDto dto)
        {
            var trainer = new Trainer(
                dto.Name,
                dto.RegistrationNumber
            );

            await _repository.AddAsync(trainer);

            return trainer.Id;
        }
        public async Task<(List<Trainer> Items, int TotalCount)> GetPagedAsync(int page, int pageSize)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 10;

            return await _repository.GetPagedAsync(page, pageSize);
        }

        public async Task<Guid> UpdateAsync(Trainer trainer)
        {
            await _repository.UpdateAsync(trainer);

            return trainer.Id;
        }

        public async Task<Trainer> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
