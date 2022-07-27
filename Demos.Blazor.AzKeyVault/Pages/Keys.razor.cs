using Demos.Blazor.AzKeyVault.Constants;
using Demos.Blazor.AzKeyVault.Services;
using Demos.Extensions;
using Microsoft.AspNetCore.Components;

namespace Demos.Blazor.AzKeyVault.Pages
{
    public partial class Keys
    {
        private KvOperationStatus _status;
        public KvOperationStatus Status
        {
            get => _status;
            set
            {
                if (value != _status)
                {
                    _status = value;
                    StateHasChanged();
                }
            }
        }

        private KvOperation? _operation;
        public KvOperation? Operation 
        { 
            get => _operation; 
            set
            {
                if(value != _operation)
                {
                    _operation = value;
                    StateHasChanged();
                }
            }
        }

        public string? Input { get; set; }
        public string? Output { get; set; }
        public string? Key { get; set; }
        public List<string> KeyList { get; set; }

        [Inject]
        public KeyVaultService? KeyVault { get; set; }

        [Inject]
        public IAsyncHolderService? AsyncHolder { get; set; }

        public Keys()
        {
            KeyList = new List<string>();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
                await RefreshKeys();
        }

        public async Task RefreshKeys()
        {
            using (var _ = AsyncHolder!.StartOperation())
            {
                try
                {
                    KeyList = await KeyVault!.ListKeys();
                }
                catch
                {
                }
            }
        }
    }
}
