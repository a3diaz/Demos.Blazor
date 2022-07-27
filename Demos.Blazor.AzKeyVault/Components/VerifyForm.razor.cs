using Demos.Blazor.AzKeyVault.Services;
using Demos.Extensions;
using Microsoft.AspNetCore.Components;

namespace Demos.Blazor.AzKeyVault.Components
{
    public partial class VerifyForm
    {
        private bool? _isValid;
        public bool? IsValid
        {
            get => _isValid;
            set
            {
                if (value != _isValid)
                {
                    _isValid = value;
                    StateHasChanged();
                }
            }
        }

        [Parameter]
        public string? Key { get; set; }

        [Inject]
        public KeyVaultService? KeyVault { get; set; }

        [Inject]
        public IAsyncHolderService? AsyncHolder { get; set; }

        public string? Data { get; set; }
        public string? Algorithm { get; set; }
        public string? Signature { get; set; }

        private List<string> Algorithms { get; set; }

        public VerifyForm()
        {
            Algorithms = new();
        }

        protected override async Task OnInitializedAsync()
        {
            await RefreshAlgorithms();
        }

        private async Task RefreshAlgorithms()
        {
            using (var _ = AsyncHolder!.StartOperation())
            {
                Algorithms = await KeyVault!.ListSignatureAlgorithms();
            }
        }

        private async Task Verify()
        {
            using (var _ = AsyncHolder!.StartOperation())
            {
                try
                {
                    IsValid = null;
                    IsValid = await KeyVault!.VerifyData(Key!, Data!, Signature!, Algorithm!);
                }
                catch
                {
                }
            }
        }
    }
}
