using Demos.Extensions;
using Microsoft.AspNetCore.Components;

namespace Demos.Controls
{
    public partial class AsyncSelect<TItem>
    {
        [Parameter]
        public IEnumerable<TItem> Items { get; set; }

        [Parameter]
        public string? Placeholder { get; set; }

        [Parameter]
        public string? LoadingPlaceholder { get; set; }

        [Parameter]
        public Func<TItem, string?> ValueSelector { get; set; }

        [Parameter]
        public Func<TItem, string?> NameSelector { get; set; }

        [Parameter]
        public string? Value { get; set; }

        [Parameter]
        public EventCallback<string?> ValueChanged { get; set; }

        public AsyncSelect()
        {
            Items = Enumerable.Empty<TItem>();
            ValueSelector = (item) => item?.ToString();
            NameSelector = (name) => name?.ToString();
        }

        private async void OnSelectionChanged(ChangeEventArgs args)
        {
            await ValueChanged.InvokeAsync((string?)args.Value);
        }
    }
}
