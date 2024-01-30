using Microsoft.AspNetCore.Components;
using YmlEditor.Data.EditModel;
using YmlEditor.Data.ViewModels;

namespace YmlEditor.Pages.ModelPage.EditDialog
{
    public class EditIndexView : ComponentBase
    {
        [Parameter]
        public EditViewModel ViewModel { get; set; }
        [Parameter]
        public EventCallback<ViewModel> Save { get; set; }
    }
}
