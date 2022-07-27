using Demos.Blazor.AzKeyVault.Services;
using Demos.Extensions;
using Microsoft.AspNetCore.Components;

namespace Demos.Blazor.AzKeyVault.Components
{
    public partial class DecryptForm
    {
        [Parameter]
        public string? Key { get; set; }

        [Inject]
        public KeyVaultService? KeyVault { get; set; }

        [Inject]
        public IAsyncHolderService? AsyncHolder { get; set; }

        public string? Cipher { get; set; }
        public string? Algorithm { get; set; }
        public string? Plain { get; set; }

        private List<string> Algorithms { get; set; }

        public DecryptForm()
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

        private async Task Decrypt()
        {
            using (var _ = AsyncHolder!.StartOperation())
            {

                try
                {
                    Plain = await KeyVault!.Decrypt(Key!, Cipher!, Algorithm!);
                }
                catch
                {
                }
            }

        }
    }
}
