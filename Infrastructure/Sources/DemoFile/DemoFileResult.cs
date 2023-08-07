using Core.Models;
using Infrastructure.Services;
using Infrastructure.Sources.File;
using TextCopy;

namespace Infrastructure.Sources.DemoFile
{
    internal class DemoFileResult : FileResult
    {
        private readonly string _fileName;
        private readonly int _lineNumber;

        public DemoFileResult(string fileName, int lineNumber)
        {
            _fileName = fileName;
            _lineNumber = lineNumber;
        }

        public override IEnumerable<ActionItem> GetActions()
        {
            var actions = new List<ActionItem>
            {
                new() { Name = $"Copy {Name} to clipboard", Action = () => { ClipboardService.SetText(Description); } },
                new() { Name = $"Open file at line {_lineNumber} in `notepad++`", Action = () => ProcessService.Start("notepad++.exe", @$"{_fileName} -n{_lineNumber}") },
                new() { Name = $"Quit", Action = () => Console.WriteLine("Goodbye.") }
            };

            return actions;
        }
    }
}
