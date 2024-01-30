using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.JSInterop;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.Util;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using YmlEditor.Data.EditModel;
using YmlEditor.Data.Service;
using YmlEditor.Data.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace YmlEditor.Pages.ModelPage
{
    public class IndexView : ComponentBase
    {
        public List<ViewModel> ListModel { get; set; } = new();
        public ViewModel Model;
        public EditViewModel EditModel = new EditViewModel();
        [Inject] ServiceModel Service { get; set; }
        public bool isDel;

        [Inject] IWebHostEnvironment WebHostEnvironment { get; set; }
        [Inject] protected IJSRuntime js { get; set; }
        [Inject] protected IWebHostEnvironment HostingEnv { get; set; }


        protected List<ViewModel> ExcelModel = new List<ViewModel>();
        protected List<ViewModel> ListNewUser = new List<ViewModel>();


        public bool flag;
        public bool flagForProcessing;
        private List<IBrowserFile> loadedFiles = new();
        private long maxFileSize = 1024 * 1024 * 15;
        private int maxAllowedFiles = 1;
        private bool isLoading;
        private List<string> newFiles = new List<string>();
        private string fileNameExcel;
        public int numberOfInputFiles = 1;
        public Dictionary<string, double> fileUploadProgress = new();

        public string Error { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                ListModel = Service.GetAll();
                await InvokeAsync(StateHasChanged);
            }
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
            /*XmlElement categoryElement = xmlDoc.CreateElement("category");
            categoryElement.InnerText = "Категория товаров";

            categoriesElement.AppendChild(categoryElement);*/



            if (listModel.Count != 0)
            {
                try
                {
                    List<ViewModel> listCatalog = new List<ViewModel>();
                    listCatalog = listModel;
                    listCatalog.GroupBy(v => v.Category).Where(g => g.Count() > 1).Select(g => g.Key);
                    foreach (var item in listCatalog)
                    {
                        XmlElement categoryElement = xmlDoc.CreateElement("category");
                        //categoryElement.SetAttribute("id", $"{item.id}");
                        categoryElement.InnerText = $"{item.Category}";

                        categoriesElement.AppendChild(categoryElement);
                    }
                }
                catch
                {
                    //Console.WriteLine("Send correct model categories");
                }
            }
            else
            {
                //Console.WriteLine("List categories is null");
            }

            XmlElement offersElement = xmlDoc.CreateElement("offers");
            shopElement.AppendChild(offersElement);

            if (listModel.Count != 0)
            {
                try
                {
                    foreach (var item in listModel)
                    {
                        AddOffer(xmlDoc, offersElement, item.Id, item.Name, item.PopularProduct, item.Price, item.InStock, item.Count, item.Picture, item.Description, item.ShortDescription, item.Url, item.Units);
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

            MemoryStream ms = new MemoryStream();
            xmlDoc.Save(ms);
            byte[] bytes = ms.ToArray();
            ms.Close();

            SaveAsFileAsync(js, "Output.xml", bytes, "application/vnd.ms-excel");

            Console.WriteLine("XML-file created!");
        }

        public async Task SaveAsFileAsync(IJSRuntime jsrun, string filename, byte[] data, string type = "application/octet-stream")
        {
            await jsrun.InvokeAsync<object>("saveAsFile", filename, Convert.ToBase64String(data));
        }

        static void AddOffer(XmlDocument xmlDoc, XmlElement parentElement, string id, string name, string popularProduct, string price, string inStock, string count, string picture, string description, string shortDescription, string url, string units)
        {

            XmlElement offerElement = xmlDoc.CreateElement("offer");
            offerElement.SetAttribute("id", id);

            if (name != null)
            {
                AddElement(xmlDoc, offerElement, "name", name);
            }
            if (popularProduct != null)
            {
                AddElement(xmlDoc, offerElement, "popularProduct", popularProduct);
            }
            if (price != null)
            {
                AddElement(xmlDoc, offerElement, "price", price);
            }
            if (inStock != null)
            {
                AddElement(xmlDoc, offerElement, "inStock", inStock);
            }
            if (count != null)
            {
                AddElement(xmlDoc, offerElement, "count", count);
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
            if (units != null)
            {
                AddElement(xmlDoc, offerElement, "units", units);
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
            if (Convert.ToInt32(item.Id) > 0)
            {
                var newItem = Service.Update(item);
                var index = ListModel.FindIndex(x => Convert.ToInt32(x.Id) == Convert.ToInt32(newItem.Id));
                ListModel[index] = newItem;
                StateHasChanged();
            }
            else
            {
                var newItem = Service.Create(item);
                //ListModel.Add(newItem);
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

        public async Task OnFileChanged(InputFileChangeEventArgs e, int index)
        {
            numberOfInputFiles++;

            foreach (var file in e.GetMultipleFiles())
            {
                var fileSize = file.Size;
                var buffer = new byte[fileSize];
                long totalBytesRead = 0;

                using var stream = file.OpenReadStream(maxAllowedSize: long.MaxValue);
                while (true)
                {
                    var read = await stream.ReadAsync(buffer);
                    if (read == 0)
                        break;

                    totalBytesRead += read;

                    await SaveFile(buffer, file.Name);
                }
            }
        }

        async Task SaveFile(byte[] buffer, string fileName)
        {
            var wwwRootPath = Path.Combine(WebHostEnvironment.WebRootPath, fileName);
            await File.WriteAllBytesAsync(wwwRootPath, buffer);
        }

        public async Task SaveFiles(InputFileChangeEventArgs e)
        {
            Error = "";
            isLoading = true;
            loadedFiles.Clear();
            flag = false;
            flagForProcessing = false;
            var pathFolder = Path.Combine(WebHostEnvironment.WebRootPath, "Excel");

            if (Directory.Exists(pathFolder))
            {
                //    Console.WriteLine("\nПапка 'Excel' уже существует или была успешно создана.\n");
            }
            else
            {
                Directory.CreateDirectory(pathFolder);
            }

            foreach (var file in e.GetMultipleFiles(maxAllowedFiles))
            {
                loadedFiles.Add(file);
                var trustedFileNameForFileStorage = Guid.NewGuid().ToString();
                var fileExtension = Path.GetExtension(file.Name);
                var fileName = trustedFileNameForFileStorage + fileExtension;
                var path = Path.Combine(pathFolder, fileName);

                fileNameExcel = path;

                using (var fs = new FileStream(path, FileMode.Create))
                {
                    await file.OpenReadStream(maxFileSize).CopyToAsync(fs);
                }
                newFiles.Add(path);
            }
            isLoading = false;

            if (fileNameExcel.Substring(fileNameExcel.Length - 4) != "xlsx")
            {
                File.Delete(fileNameExcel);
                return;
            }

            ListNewUser.Clear();

            ExcelModel = Service.GetAll();

            using (FileStream file = new FileStream(fileNameExcel, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = new XSSFWorkbook(file);

                for (var sheetiter = 0; sheetiter < workbook.NumberOfSheets; sheetiter++)
                {
                    ISheet sheet = workbook.GetSheetAt(sheetiter);
                    IRow firstRow = sheet.GetRow(sheet.FirstRowNum);

                    var firstCellCheck = firstRow.GetCell(CellReference.ConvertColStringToIndex("A"));
                    var secondCellCheck = firstRow.GetCell(CellReference.ConvertColStringToIndex("B"));
                    var thirdCellCheck = firstRow.GetCell(CellReference.ConvertColStringToIndex("C"));
                    var fourCellCheck = firstRow.GetCell(CellReference.ConvertColStringToIndex("D"));
                    var fiveCellCheck = firstRow.GetCell(CellReference.ConvertColStringToIndex("E"));
                    var sixCellCheck = firstRow.GetCell(CellReference.ConvertColStringToIndex("F"));
                    var sevenCellCheck = firstRow.GetCell(CellReference.ConvertColStringToIndex("G"));
                    var eightCellCheck = firstRow.GetCell(CellReference.ConvertColStringToIndex("H"));
                    var nineCellCheck = firstRow.GetCell(CellReference.ConvertColStringToIndex("I"));
                    var tenCellCheck = firstRow.GetCell(CellReference.ConvertColStringToIndex("J"));
                    var elevenCellCheck = firstRow.GetCell(CellReference.ConvertColStringToIndex("K"));
                    var twelveCellCheck = firstRow.GetCell(CellReference.ConvertColStringToIndex("L"));
                    string firstCell = null;
                    string secondCell = null;
                    string thirdCell = null;
                    string fourCell = null;
                    string fiveCell = null;
                    string sixCell = null;
                    string sevenCell = null;
                    string eightCell = null;
                    string nineCell = null;
                    string tenCell = null;
                    string elevenCell = null;
                    string twelveCell = null;
                    if (firstCellCheck != null && secondCellCheck != null && thirdCellCheck != null && fourCellCheck != null && fiveCellCheck != null)
                    {
                        firstCell = firstCellCheck.ToString();
                        secondCell = secondCellCheck.ToString();
                        thirdCell = thirdCellCheck.ToString();
                        fourCell = fourCellCheck.ToString();
                        fiveCell = fiveCellCheck.ToString();
                        sixCell = sixCellCheck.ToString();
                        sevenCell = sevenCellCheck.ToString();
                        eightCell = eightCellCheck.ToString();
                        nineCell = nineCellCheck.ToString();
                        tenCell = tenCellCheck.ToString();
                        elevenCell = elevenCellCheck.ToString();
                        twelveCell= twelveCellCheck.ToString();
                    }
                    if (firstCell != "Категория" || secondCell != "Название" || thirdCell != "Идентификатор" || fourCell != "Описание" || fiveCell != "Короткое описание" || sixCell != "Цена" || sevenCell != "Фото" || eightCell != "Популярный товар" || nineCell != "В наличии" || tenCell != "Количество" || elevenCell != "Единицы измерения" || twelveCell != "Ссылка")
                    {
                        File.Delete(fileNameExcel);
                        ListNewUser.Clear();
                        return;
                    }
                    
                    for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
                    {
                        ViewModel userViewModel = new ViewModel();
                        IRow row = sheet.GetRow(i);
                        if (row == null) { continue; }
                        string Id = null;
                        string Category = null;
                        string Price = null;
                        string InStock = null;
                        string Count = null;
                        string Url = null;
                        string Unit = null;
                        string PopularProduct = null;
                        string Picture = null;
                        string Description = null;
                        string ShortDescription = null;
                        string Name = null;

                        string columnLetter = "A";
                        int columnIndex = CellReference.ConvertColStringToIndex(columnLetter);
                        if (row.GetCell(columnIndex) != null)
                        {
                            try
                            {
                                Category = row.GetCell(columnIndex).StringCellValue;
                                userViewModel.Category = Category;
                            }
                            catch (Exception)
                            {
                                Error += "Столбец Категория содержит неправильный тип данных. ";
                            }
                        }
                        else
                        {
                            Error += "Столбец Категория пуст. \n";
                        }


                        columnLetter = "B";
                        columnIndex = CellReference.ConvertColStringToIndex(columnLetter);
                        if (row.GetCell(columnIndex) != null)
                        {
                            try
                            {
                                Name = row.GetCell(columnIndex).StringCellValue;
                                userViewModel.Name = Name;
                            }
                            catch (Exception)
                            {
                                Error += "Столбец Name содержит неправильный тип данных. \n";
                            }
                        }
                        else
                        {
                            Error += "Столбец Name пуст. \n";
                        }


                        columnLetter = "C";
                        columnIndex = CellReference.ConvertColStringToIndex(columnLetter);
                        if (row.GetCell(columnIndex) != null)
                        {
                            try
                            {

                                var x = (int)row.GetCell(columnIndex).NumericCellValue;
                                Id = x.ToString();
                                userViewModel.Id = Id;
                            }
                             catch (Exception)
                            {
                                Error += "Столбец Идентификатор содержит неправильный тип данных. \n";
                            }
                        }
                        else
                        {
                            Error += "Столбец Идентификатор пуст. \n";
                        }


                        columnLetter = "D";
                        columnIndex = CellReference.ConvertColStringToIndex(columnLetter);
                        if (row.GetCell(columnIndex) != null)
                        {
                            try
                            {
                                Description = row.GetCell(columnIndex).StringCellValue;
                                userViewModel.Description = Description;
                            }
                            catch (Exception)
                            {
                                Error += "Столбец Описание содержит неправильный тип данных. \n";
                            }
                        }
                        else
                        {
                            Error += "Столбец Описание пуст. ";
                        }


                        columnLetter = "E";
                        columnIndex = CellReference.ConvertColStringToIndex(columnLetter);
                        if (row.GetCell(columnIndex) != null)
                        {
                            try
                            {
                                ShortDescription = row.GetCell(columnIndex).StringCellValue;
                                userViewModel.ShortDescription = ShortDescription;
                            }
                            catch (Exception)
                            {
                                Error += "Столбец Короткое описание содержит неправильный тип данных. \n";
                            }
                        }
                        else
                        {
                            Error += "Столбец Короткое описание пуст. \n";
                        }

                        columnLetter = "F";
                        columnIndex = CellReference.ConvertColStringToIndex(columnLetter);
                        if (row.GetCell(columnIndex) != null)
                        {
                            try
                            {
                                var x = (int)row.GetCell(columnIndex).NumericCellValue;
                                Price = x.ToString();
                                userViewModel.Price = Price;
                            }
                            catch (Exception)
                            {
                                Error += "Столбец Цена содержит неправильный тип данных. \n";
                            }
                        }
                        else
                        {
                            Error += "Столбец Цена пуст. \n";
                        }

                        columnLetter = "G";
                        columnIndex = CellReference.ConvertColStringToIndex(columnLetter);
                        if (row.GetCell(columnIndex) != null)
                        {
                            try
                            {
                                Picture = row.GetCell(columnIndex).StringCellValue;
                                userViewModel.Picture = Picture;
                            }
                            catch (Exception)
                            {
                                Error += "Столбец Фото содержит неправильный тип данных. \n";
                            }
                        }
                        else
                        {
                            Error += "Столбец Фото пуст. \n";
                        }

                        columnLetter = "H";
                        columnIndex = CellReference.ConvertColStringToIndex(columnLetter);
                        if (row.GetCell(columnIndex) != null)
                        {
                            try
                            {
                                PopularProduct = row.GetCell(columnIndex).StringCellValue;
                                userViewModel.PopularProduct = PopularProduct;
                            }
                            catch (Exception)
                            {
                                Error += "Столбец Популярный товар содержит неправильный тип данных. \n";
                            }
                        }
                        else
                        {
                            Error += "Столбец Популярный товар пуст. \n";
                        }

                        columnLetter = "I";
                        columnIndex = CellReference.ConvertColStringToIndex(columnLetter);
                        if (row.GetCell(columnIndex) != null)
                        {
                            try
                            {
                                InStock = row.GetCell(columnIndex).StringCellValue;
                                userViewModel.InStock = InStock;
                            }
                            catch (Exception)
                            {
                                Error += "Столбец В наличии содержит неправильный тип данных. \n";
                            }
                        }
                        else
                        {
                            Error += "Столбец В наличии пуст. \n";
                        }

                        columnLetter = "J";
                        columnIndex = CellReference.ConvertColStringToIndex(columnLetter);
                        if (row.GetCell(columnIndex) != null)
                        {
                            try
                            {
                                var x = (int)row.GetCell(columnIndex).NumericCellValue;
                                Count = x.ToString();
                                userViewModel.Count = Count;
                            }
                            catch (Exception)
                            {
                                Error += "Столбец Количество содержит неправильный тип данных. \n";
                            }
                        }
                        else
                        {
                            Error += "Столбец Количество пуст. \n";
                        }

                        columnLetter = "K";
                        columnIndex = CellReference.ConvertColStringToIndex(columnLetter);
                        if (row.GetCell(columnIndex) != null)
                        {
                            try
                            {
                                Unit = row.GetCell(columnIndex).StringCellValue;
                                userViewModel.Units = Unit;
                            }
                            catch (Exception)
                            {
                                Error += "Столбец Единицы измерения содержит неправильный тип данных. \n";
                            }
                        }
                        else
                        {
                            Error += "Столбец Единицы измерения пуст. \n";
                        }

                        columnLetter = "L";
                        columnIndex = CellReference.ConvertColStringToIndex(columnLetter);
                        if (row.GetCell(columnIndex) != null)
                        {
                            try
                            {
                                Url = row.GetCell(columnIndex).StringCellValue;
                                userViewModel.Url = Url;
                            }
                            catch (Exception)
                            {
                                Error += "Столбец Ссылка содержит неправильный тип данных. \n";
                            }
                        }
                        else
                        {
                            Error += "Столбец Ссылка пуст. \n";
                        }

                        Service.CreateFromExcel(userViewModel);

                        /*foreach (var item in ListNewUser)
                        {
                            var rezult = Service.Create(item);
                        }*/
                        ListModel = Service.GetAll();

                        StateHasChanged();
                    }
                }


            }

        }
        public void ProcessExcelFile()
        {
            foreach (var item in ListNewUser)
            {
                var rezult = Service.Create(item);
            }
            ListModel = Service.GetAll();
            StateHasChanged();
        }
    }
}
