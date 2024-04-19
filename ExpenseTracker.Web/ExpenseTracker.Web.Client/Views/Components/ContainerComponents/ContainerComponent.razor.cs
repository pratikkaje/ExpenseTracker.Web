using ExpenseTracker.Web.Client.Models.ContainerComponents;
using Microsoft.AspNetCore.Components;

namespace ExpenseTracker.Web.Client.Views.Components.ContainerComponents
{
    public partial class ContainerComponent : ComponentBase
    {
        [Parameter]
        public ComponentState ComponentState { get; set; }

        [Parameter]
        public RenderFragment Content { get; set; }

        [Parameter]
        public RenderFragment Error { get; set; }
    }
}
