// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using ExpenseTracker.Web.Models.Basics;
using Microsoft.AspNetCore.Components;

namespace ExpenseTracker.Web.Views.Components.Basics
{
    public partial class BasicComponent : ComponentBase
    {
        [Parameter]
        public ComponentState State { get; set; }

        [Parameter]
        public RenderFragment Loading { get; set; }

        [Parameter]
        public RenderFragment Content { get; set; }

        [Parameter]
        public RenderFragment Error { get; set;}

        public RenderFragment GetFragment()
        {
            return State switch
            {
                ComponentState.Loading => Loading,
                ComponentState.Content => Content,
                _ => Error
            };
        }

    }
}
