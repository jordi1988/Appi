using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Handler
{
    public class ConsoleHandler : IHandler
    {
        public Result PromtForItemSelection(IEnumerable<PromptGroup> items)
        {
            foreach (var item in items)
            {
                foreach (var innerItem in item.Items)
                {
                    Console.WriteLine(innerItem);
                }
            }

            return new NoItemResult();
        }

        public void DisplayItem(Result item)
        {
            Console.WriteLine($"({item.Id})\t{item.Name}\t{item.Description}");
        }

        public void PromtForActionInvokation(Result item)
        {
            // no item selected ...
        }
    }
}
