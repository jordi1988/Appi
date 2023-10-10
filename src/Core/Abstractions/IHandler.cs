using Core.Models;

namespace Core.Abstractions
{
    public interface IHandler
    {
        ResultItemBase? PromtForItemSelection(IEnumerable<PromptGroup> items);

        void PromtForActionInvokation(ResultItemBase? item);

        void DisplayItem(ResultItemBase? item);

        void DisplaySources(IEnumerable<ISource> sources);

        void CreateBreakdownChart(IEnumerable<PromptGroup> allResults);

        void SaveResultsToMemory(IEnumerable<PromptGroup> allResults);

        IEnumerable<PromptGroup> ReadResultsFromMemory();

        void ClearScreen();
    }
}
