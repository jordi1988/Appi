using Core.Abstractions;
using Core.Models;

namespace Infrastructure.Services
{
    public class MemoryResultStateService : IResultStateService
    {
        private IEnumerable<PromptGroup>? _results;

        public IEnumerable<PromptGroup> Load()
        {
            return _results ?? Enumerable.Empty<PromptGroup>();
        }

        public void Save(IEnumerable<PromptGroup> allResults)
        {
            _results = allResults;
        }
    }
}
