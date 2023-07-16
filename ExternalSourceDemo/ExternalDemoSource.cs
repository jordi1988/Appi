﻿using Domain.Entities;
using Domain.Interfaces;
using static Ui.Appi.Commands.FindItemsCommand;

namespace ExternalSourceDemo
{
    public class ExternalDemoSource : ISource
    {
        private readonly Settings? _settings;
        public string TypeName { get; set; } = typeof(ExternalDemoSource).Name;
        public string Name { get; set; } = "Demo Assembly";
        public string Description { get; set; } = "Returns hard-coded hello world.";
        public bool IsActive { get; set; } = true;
        public int SortOrder { get; set; } = 50;
        public string? Path { get; set; } = null;

        public ExternalDemoSource(Settings? settings) : base()
        {
            _settings = settings;
        }

        public async Task<IEnumerable<Result>> ReadAsync()
        {
            var output = new List<ExternalSourceDemoResult>() 
            { 
                new() { Name = "Hello", Description = _settings?.Query ?? "World" } 
            };

            return await Task.FromResult(output);
        }
    }

    public class ExternalSourceDemoResult : Result
    {
        public override IEnumerable<ActionItem> GetActions()
        {
            return Enumerable.Empty<ActionItem>();
        }

        public override string ToString()
        {
            return $"{Name} {Description}!";
        }
    }
}