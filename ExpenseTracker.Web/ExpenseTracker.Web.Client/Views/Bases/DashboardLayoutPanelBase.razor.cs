// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace ExpenseTracker.Web.Client.Views.Bases
{
    public partial class DashboardLayoutPanelBase : ComponentBase
    {
        [Parameter]
        public int Column { get; set; }

        [Parameter]
        public int Row { get; set; }

        [Parameter]
        public int SizeX { get; set; }

        [Parameter]
        public int SizeY { get; set; }

        [Parameter]
        public RenderFragment HeaderContent { get; set; }

        [Parameter]
        public RenderFragment Content { get; set; }
    }
}
