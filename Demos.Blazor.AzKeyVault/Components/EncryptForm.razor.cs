using Demos.Blazor.AzKeyVault.Services;
using Demos.Extensions;
using Microsoft.AspNetCore.Components;

namespace Demos.Blazor.AzKeyVault.Components
{
    public partial class EncryptForm
    {
        [Parameter]
        public string? Key { get; set; }

        [Inject]
        public KeyVaultService? KeyVault { get; set; }

        [Inject]
        public IAsyncHolderService? AsyncHolder { get; set; }

        public string? Plain { get; set; }
        public string? Algorithm { get; set; }
        public string? Cipher { get; set; }

        private List<string> Algorithms { get; set; }

        public EncryptForm()
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
                Algorithms = await KeyVault!.ListEncryptionAlgorithms();
            }
        }

        public async Task Encrypt()
        {
            using (var _ = AsyncHolder!.StartOperation())
            {
                try
                {
                    Cipher = await KeyVault!.Encrypt(Key!, Plain!, Algorithm!);
                }
                catch
                {
                }
            }

        }
    }
}
