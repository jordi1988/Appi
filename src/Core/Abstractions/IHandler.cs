using Core.Models;

namespace Core.Abstractions
{
    public interface IHandler
    {
        ResultItemBase PromtForItemSelection(IEnumerable<PromptGroup> items);

        void PromtForActionInvokation(ResultItemBase item);

        void DisplayItem(ResultItemBase item);

        void DisplaySources(IEnumerable<ISource> sources);

        void CreateBreakdownChart(List<PromptGroup> allResults);
    }
}
