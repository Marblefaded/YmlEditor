using Microsoft.AspNetCore.Components;

namespace YmlEditor.Pages.ModelPage.DeleteDialog
{
    public class DeleteViewModel : ComponentBase
    {
        [Parameter]
        public bool isOpenDelete { get; set; }
        [Parameter]
        public EventCallback<bool> Delete { get; set; }
    }
}
