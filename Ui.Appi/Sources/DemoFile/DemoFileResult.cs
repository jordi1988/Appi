using Domain.Entities;
using Infrastructure.Sources.File;
using System.Diagnostics;

namespace Ui.Appi.Sources.DemoFile
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
                new() { Name = $"Copy {Name} to clipboard", Action = () => { Console.WriteLine($"Item {Name} copied to clipboard (not really)."); } },
                new() { Name = $"Open file at line {_lineNumber} in `notepad++`", Action = () => Process.Start(new ProcessStartInfo(
                    "notepad++.exe", 
                    @$"{_fileName} -n{_lineNumber}") 
                    { UseShellExecute = true }) 
                },
                new() { Name = $"Add new item", Action = () => { Console.WriteLine($"Please enter new item"); var result = Console.ReadLine(); Console.WriteLine($"In a real-world app the string `{result}` would have been added ;)"); } },
                new() { Name = $"Quit", Action = () => Console.WriteLine("Goodbye :)") }
            };

            return actions;
        }
    }
}
