using Demos.Extensions;
using Microsoft.AspNetCore.Components;

namespace Demos.Controls
{
    public abstract class AsyncControl : ComponentBase
    {
        [Parameter]
        public string? OperationsFilter { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> InputAttributes { get; set; }

        [Inject]
        public IAsyncHolderService? AsyncHolder { get; set; }

        private bool _isLoading;
        protected bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    StateHasChanged();
                }
            }
        }

        public AsyncControl()
        {
            InputAttributes = new();
        }

        protected override void OnInitialized()
        {
            AsyncHolder!.OnChange += AsyncHolderOnChange;
        }

        private void AsyncHolderOnChange()
        {
            IsLoading = AsyncHolder!.IsRunning(OperationsFilter!);
        }
    }
}
