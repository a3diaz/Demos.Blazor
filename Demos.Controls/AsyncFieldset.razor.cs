using Microsoft.AspNetCore.Components;

namespace Demos.Controls
{
    public partial class AsyncFieldset
    {

        [Parameter]
        public RenderFragment? ChildContent { get; set; }
    }
}
