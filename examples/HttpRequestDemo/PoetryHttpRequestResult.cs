using Core.Abstractions;
using Core.Attributes;
using Core.Models;
using Microsoft.Extensions.Localization;
using TextCopy;

namespace Infrastructure.HttpRequestDemoExample
{
    internal class PoetryHttpRequestResult : ResultItemBase
    {
        private readonly IStringLocalizer<PoetryHttpRequestSource> _customLocalizer;
        private readonly IHandlerHelper _handlerHelper;

        public override string Name { get => Author; set => Author = value; }

        public override string Description { get => Title; set => Title = value; }

        [DetailViewColumn]
        public string Author { get; set; } = string.Empty;

        [DetailViewColumn]
        public string Title { get; set; } = string.Empty;

        [DetailViewColumn<string>]
        public string Lines { get; set; } = string.Empty;

        [DetailViewColumn<int>]
        public int LineCount { get; set; }

        public PoetryHttpRequestResult(IStringLocalizer<PoetryHttpRequestSource> customLocalizer, IHandlerHelper handlerHelper)
        {
            _customLocalizer = customLocalizer ?? throw new ArgumentNullException(nameof(customLocalizer));
            _handlerHelper = handlerHelper ?? throw new ArgumentNullException(nameof(handlerHelper));
        }

        public override IEnumerable<ActionItem> GetActions()
        {
            var actions = new List<ActionItem>
            {
                new() { Name = _customLocalizer["Copy lines to clipboard"], Action = () => { ClipboardService.SetText(string.Join(Environment.NewLine, Lines)); } },
                _handlerHelper.Back(),
                _handlerHelper.Exit()
            };

            return actions;
        }

        public override string ToString()
        {
            return $"{Name,-30}{Description,-35}";
        }
    }
}
