using Domain.Entities;
using Domain.Interfaces;

namespace Ui.Appi.Sources.MoreOptions
{
    internal class MoreOptionsItemSource : ISource
    {
        public string TypeName { get; set; } = typeof(MoreOptionsItemSource).Name;
        public string Name { get; set; } = "More";
        public string Description { get; set; } = "Non-contextual options";
        public bool IsActive { get; set; } = true;
        public int SortOrder { get; set; } = 99;
        public string? Path { get; set; }

        public virtual async Task<IEnumerable<Result>> ReadAsync()
        {
            var output = new List<Result>
            {
                new MoreOptionsItemResult()
                {
                    Name = "More options",
                    Description = string.Empty
                }
            };

            return await Task.FromResult(output);
        }
    }
}
