using Core.Abstractions;
using Core.Models;
using Infrastructure.Services;
using Infrastructure.Sources.File;
using TextCopy;

namespace Infrastructure.FileDemo
{
    public class FileDemoResult : FileResult
    {
        private readonly string _fileName;
        private readonly int _lineNumber;
        private readonly IHandlerHelper _handlerHelper;

        public FileDemoResult(string fileName, int lineNumber, IHandlerHelper handlerHelper)
        {
            _fileName = fileName;
            _lineNumber = lineNumber;
            _handlerHelper = handlerHelper ?? throw new ArgumentNullException(nameof(handlerHelper));
        }

        public override IEnumerable<ActionItem> GetActions()
        {
            var actions = new List<ActionItem>
            {
                new() { Name = $"Copy {Name} to clipboard", Action = () => { ClipboardService.SetText(Description); } },
                new() { Name = $"Open file at line {_lineNumber} in `notepad++`", Action = () => ProcessService.Start("notepad++.exe", @$"{_fileName} -n{_lineNumber}") },
                _handlerHelper.Back(),
                _handlerHelper.Exit()
            };

            return actions;
        }
    }
}
