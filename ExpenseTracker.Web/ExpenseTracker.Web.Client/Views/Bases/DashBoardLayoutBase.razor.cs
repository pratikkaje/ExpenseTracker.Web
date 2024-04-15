// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace ExpenseTracker.Web.Client.Views.Bases
{
    public partial class DashBoardLayoutBase : ComponentBase
    {
        [Parameter]
        public int Columns { get; set; }

        [Parameter]
        public double CellAspectRatio { get; set; }

        [Parameter]
        public double[] CellSpacing { get; set; }

        [Parameter]
        public bool AllowDragging { get; set; }

        [Parameter]
        public bool AllowResizing { get; set; }

        [Parameter]
        public bool ShowGridLines { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }
    }
}
