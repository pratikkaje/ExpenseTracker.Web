// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace ExpenseTracker.Web.Client.Views.Bases
{
    public partial class DashboardLayoutBase : ComponentBase
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

        public void SetColumns(int columns) =>
            this.Columns = columns;

        public void SetCellAspectRatio(double cellaspectratio) =>
            this.CellAspectRatio = cellaspectratio;

        public void SetCellSpacing(double[] cellspacing) =>
            this.CellSpacing = cellspacing;

        public void SetAllowDragging(bool allowDragging) =>
            this.AllowDragging = allowDragging;

        public void SetAllowResizing(bool allowResizing) =>
            this.AllowResizing = allowResizing;

        public void SetShowGridLines(bool showGridLines) =>
            this.ShowGridLines = showGridLines;
    }
}
