using Demos.Blazor.AzKeyVault.Constants;
using Demos.Blazor.AzKeyVault.Services;
using Demos.Extensions;
using Microsoft.AspNetCore.Components;

namespace Demos.Blazor.AzKeyVault.Components
{
    public partial class SignForm
    {
        [Parameter]
        public string? Key { get; set; }

        [Inject]
        public KeyVaultService? KeyVault { get; set; }

        [Inject]
        public IAsyncHolderService? AsyncHolder { get; set; }

        public string? Data { get; set; }
        public KvSignMode Mode { get; set; } = KvSignMode.Hash;
        public string? Algorithm { get; set; }
        public string? Signature { get; set; }

        private List<string> Algorithms { get; set; }

        public SignForm()
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

        private async Task Sign()
        {
            using (var _ = AsyncHolder!.StartOperation())
            {
                try
                {
                    Signature = Mode switch
                    {
                        KvSignMode.Hash => await KeyVault!.Sign(Key!, Data!, Algorithm!),
                        KvSignMode.Data => await KeyVault!.SignData(Key!, Data!, Algorithm!),
                        _ => string.Empty
                    };
                }
                catch
                {
                }
            }

        }
    }
}
