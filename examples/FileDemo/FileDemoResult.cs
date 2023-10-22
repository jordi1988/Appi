using Core.Abstractions;
using Core.Models;
using Infrastructure.Services;
using Infrastructure.Sources.File;
using Microsoft.Extensions.Localization;
using TextCopy;

namespace Infrastructure.FileDemoExample
{
    public class FileDemoResult : FileResult
    {
        private readonly string _fileName;
        private readonly int _lineNumber;
        private readonly IHandlerHelper _handlerHelper;
        private readonly IStringLocalizer<FileDemoSource> _customLocalizer;

        public FileDemoResult(string fileName, int lineNumber, IHandlerHelper handlerHelper, IStringLocalizer<FileDemoSource> customLocalizer)
        {
            _fileName = fileName;
            _lineNumber = lineNumber;
            _handlerHelper = handlerHelper ?? throw new ArgumentNullException(nameof(handlerHelper));
            _customLocalizer = customLocalizer ?? throw new ArgumentNullException(nameof(customLocalizer));
        }

        public override IEnumerable<ActionItem> GetActions()
        {
            var actions = new List<ActionItem>
            {
                new() { 
                    Name = _customLocalizer["Copy {0} to clipboard", Name], 
                    Action = () => { ClipboardService.SetText(Description); } 
                },
                new() { 
                    Name = _customLocalizer["Open file at line {0} in notepad++", _lineNumber], 
                    Action = () => ProcessService.Start("notepad++.exe", @$"{_fileName} -n{_lineNumber}") 
                },
                _handlerHelper.Back(),
                _handlerHelper.Exit()
            };

            return actions;
        }
    }
}
