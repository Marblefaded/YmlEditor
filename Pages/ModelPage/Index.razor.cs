using Microsoft.AspNetCore.Components;
using YmlEditor.Data.EditModel;
using YmlEditor.Data.Service;
using YmlEditor.Data.ViewModels;

namespace YmlEditor.Pages.ModelPage
{
    public class IndexView : ComponentBase
    {
        public List<ViewModel> ListModel { get; set; } = new();
        public ViewModel Model;
        public EditViewModel EditModel = new EditViewModel();
        [Inject] ServiceModel Service {  get; set; }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                ListModel = Service.GetAll();
                StateHasChanged();
            }
        }

        public void GettingElements()
        {

        }

        public void InsertElements()
        {
            ListModel.Add(new ViewModel
            {
                Id = "90125",
                Name = "Товар Brand ABC-01",
                Vendor = "Nike",
                Price = "800",
                CurrencyId = "RUR",
                CategoryId = "101",
                Picture = "http://example.com/image01.jpg",
                Description = "Описание первого товара",
                ShortDescription = "Короткое описание первого товара",
                Url = "http://example.com/product01/"
            });
        }

        public void Create()
        {
            Model = new ViewModel();
            EditModel.Model = Model;
            EditModel.DialogIsOpen = true;
        }

        public void Save(ViewModel item)
        {
            /*Переписать методы для работы внутри сервиса*/

            /*if (item.PetsId > 0)
            {
                var newItem = Service.Update(item);
                var index = Model.FindIndex(x => x.PetsId == newItem.PetsId);
                Model[index] = newItem;
            }
            else
            {
                var newItem = Service.Create(item);
                Model.Add(newItem);
            }
            EditModel.DialogIsOpen = false;
            StateHasChanged();*/
        }

        public void Delete(ViewModel item)
        {
            Model = item;
            EditModel.Model = item;
            EditModel.DialogIsOpen = true;
        }
    }
}
