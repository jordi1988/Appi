using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IHandler
    {
        Result PromtForItemSelection(IEnumerable<PromptGroup> items);

        void PromtForActionInvokation(Result item);

        void DisplayItem(Result item);
    }
}
