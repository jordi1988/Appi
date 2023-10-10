using Core.Models;

namespace Core.Abstractions
{
    public interface IHandlerHelper
    {
        ActionItem Back();

        ActionItem Exit();

        string EscapeMarkup(string? text);

        string RemoveMarkup(string? text);
    }
}
