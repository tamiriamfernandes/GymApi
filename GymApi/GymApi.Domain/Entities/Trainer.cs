using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GymApi.Domain.Entities;

public class Trainer
{
    public Guid Id { get; private set; }

    [StringLength(100, MinimumLength = 1)]
    public string Name { get; private set; } = default!;
    public string? RegistrationNumber { get; private set; } // ex: CREF

    // 🔗 vínculo opcional com usuário (login)
    public Guid? UserId { get; private set; }
    //public User? User { get; private set; }

    // navegação
    //public ICollection<Workout> Workouts { get; private set; } = new List<Workout>();

    // EF Core
    protected Trainer() { }

    public Trainer(string name, string? registrationNumber = null)
    {
        Id = Guid.NewGuid();
        Name = name;
        RegistrationNumber = registrationNumber;
    }

    public Trainer UpdateInfo(string name, string? registrationNumber)
    {
        Name = name;
        RegistrationNumber = registrationNumber;

        return this;
    }

    public void LinkUser(Guid userId)
    {
        UserId = userId;
    }
}
