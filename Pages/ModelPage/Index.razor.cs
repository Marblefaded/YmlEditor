using Microsoft.AspNetCore.Components;
using System.Reflection;
using System.Xml;
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
        public bool isDel;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                ListModel = Service.GetAll();
                await InvokeAsync(StateHasChanged);
            }
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

        public void CreateYml(List<ViewModel> listModel)
        {
            XmlDocument xmlDoc = new XmlDocument();

            XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlDoc.AppendChild(xmlDeclaration);

            XmlElement ymlCatalogElement = xmlDoc.CreateElement("yml_catalog");
            xmlDoc.AppendChild(ymlCatalogElement);

            XmlElement shopElement = xmlDoc.CreateElement("shop");
            ymlCatalogElement.AppendChild(shopElement);

            XmlElement categoriesElement = xmlDoc.CreateElement("categories");
            shopElement.AppendChild(categoriesElement);
            XmlElement categoryElement = xmlDoc.CreateElement("category");
            categoryElement.SetAttribute("id", "101");
            categoryElement.InnerText = "Категория товаров";

            categoriesElement.AppendChild(categoryElement);
            /*if (categories.Count != 0)
            {
                try
                {
                    foreach (var item in categories)
                    {
                        XmlElement categoryElement = xmlDoc.CreateElement("category");
                        categoryElement.SetAttribute("id", $"{item.id}");
                        categoryElement.InnerText = $"{item.InnerText}";

                        categoriesElement.AppendChild(categoryElement);
                    }
                }
                catch
                {
                    Console.WriteLine("Send correct model categories");
                }
            }
            else
            {
                Console.WriteLine("List categories is null");
            }*/

            XmlElement offersElement = xmlDoc.CreateElement("offers");
            shopElement.AppendChild(offersElement);

            if (listModel.Count != 0)
            {
                try
                {
                    foreach (var item in listModel)
                    {
                        AddOffer(xmlDoc, offersElement, item.Id, item.Name, item.Vendor, item.Price, item.CurrencyId, item.CategoryId, item.Picture, item.Description, item.ShortDescription, item.Url);
                    }
                }
                catch
                {
                    Console.WriteLine("Send correct model offers");
                }
            }
            else
            {
                Console.WriteLine("List models is null");
            }

            xmlDoc.Save("output.xml");
            Console.WriteLine("XML-file created!");
        }

        static void AddOffer(XmlDocument xmlDoc, XmlElement parentElement, string id, string name, string vendor, string price, string currencyId, string categoryId, string picture, string description, string shortDescription, string url)
        {

            XmlElement offerElement = xmlDoc.CreateElement("offer");
            offerElement.SetAttribute("id", id);

            if (name != null)
            {
                AddElement(xmlDoc, offerElement, "name", name);
            }
            if (vendor != null)
            {
                AddElement(xmlDoc, offerElement, "vendor", vendor);
            }
            if (price != null)
            {
                AddElement(xmlDoc, offerElement, "price", price);
            }
            if (currencyId != null)
            {
                AddElement(xmlDoc, offerElement, "currencyId", currencyId);
            }
            if (categoryId != null)
            {
                AddElement(xmlDoc, offerElement, "categoryId", categoryId);
            }
            if (picture != null)
            {
                AddElement(xmlDoc, offerElement, "picture", picture);
            }
            if (description != null)
            {
                AddElement(xmlDoc, offerElement, "description", description);
            }
            if (shortDescription != null)
            {
                AddElement(xmlDoc, offerElement, "shortDescription", shortDescription);
            }
            if (url != null)
            {
                AddElement(xmlDoc, offerElement, "url", url);
            }

            parentElement.AppendChild(offerElement);
        }

        static void AddElement(XmlDocument xmlDoc, XmlElement parentElement, string elementName, string elementValue)
        {
            XmlElement childElement = xmlDoc.CreateElement(elementName);
            childElement.InnerText = elementValue;
            parentElement.AppendChild(childElement);
        }

        public void Save(ViewModel item)
        {
            /*Переписать методы для работы внутри сервиса*/

            if (Convert.ToInt32(item.Id) > 0)
            {
                var newItem = Service.Update(item);
                var index = ListModel.FindIndex(x => Convert.ToInt32(x.Id) == Convert.ToInt32(newItem.Id));
                ListModel[index] = newItem;
            }
            else
            {
                var newItem = Service.Create(item);
                ListModel.Add(newItem);
            }
            EditModel.DialogIsOpen = false;
            StateHasChanged();
        }

        public void Delete(ViewModel item)
        {
            Model = item;
            EditModel.Model = item;
            /*EditModel.DialogIsOpen = true;*/
        }
        public void DeleteItem(ViewModel item)
        {
            Model = item;
            isDel = true;
            StateHasChanged();
        }

        public void Delete(bool value)
        {
            if (value == true)
            {
                Service.Remove(Model);
                ListModel = Service.GetAll();
            }

            isDel = false;
            StateHasChanged();
        }

        public void Edit(ViewModel item)
        {
            Model = item;
            EditModel.Model = item;
            EditModel.DialogIsOpen = true;
        }
    }
}
