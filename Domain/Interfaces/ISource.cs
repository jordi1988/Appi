﻿using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ISource
    {
        string TypeName { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        bool IsActive { get; set; }
        int SortOrder { get; set; }
        string? Path { get; set; }

        Task<IEnumerable<Result>> ReadAsync();
    }
}