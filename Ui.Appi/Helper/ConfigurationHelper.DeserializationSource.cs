using Domain.Entities;
using Domain.Interfaces;
using System.Text.Json.Serialization;

namespace Ui.Appi.Helper
{
    internal static partial class ConfigurationHelper
    {
        internal sealed class DeserializationSource : ISource
        {
            public string TypeName { get; set; } = typeof(DeserializationSource).Name;
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public bool IsActive { get; set; }
            public int SortOrder { get; set; }
            public string? Path { get; set; }

            public async Task<IEnumerable<Result>> ReadAsync()
            {
                return await Task.FromResult(Enumerable.Empty<Result>());
            }
        }
    }
}
