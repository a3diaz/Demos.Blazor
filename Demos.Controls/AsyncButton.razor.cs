using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Demos.Controls
{
    public partial class AsyncButton
    {
        [Parameter]
        public string? Text { get; set; }

        [Parameter]
        public string? LoadingText { get; set; }
    }
}
