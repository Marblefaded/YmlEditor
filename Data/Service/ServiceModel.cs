using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using YmlEditor.Data.ViewModels;
using Converter.Model;

namespace YmlEditor.Data.Service
{
    public class ServiceModel
    {
        public List<ViewModel> listModel = new List<ViewModel>();
        public List<ViewModel> GetAll()
        {
            string line = "";
            StreamReader sr = new StreamReader("..\\YmlEditor\\data.json");

            line = sr.ReadToEnd();

            sr.Close();

            listModel = JsonConvert.DeserializeObject<List<ViewModel>>(line);

            return listModel;

        }

        public ViewModel Remove(ViewModel model)
        {
            listModel.Remove(model);

            string json = JsonConvert.SerializeObject(listModel, Formatting.Indented);

            StreamWriter sw = new StreamWriter("..\\YmlEditor\\data.json");

            sw.WriteLine(json);
            sw.Close();

            return null;
        }

        public ViewModel Create(ViewModel item)// Создание
        {
            //var newItem = mRepoProject.Create(item.Item);
            /*foreach (var product in listModel)
            {
                product.Id
            }*/
            /*listModel.Max<ViewModel, Id>;*/
            int max = 0;

            if(listModel.Count != 0)
            {
                max = Convert.ToInt32(listModel.MaxBy(x => Convert.ToInt32(x.Id)).Id) + 1;
            }

            item.Id = max.ToString();
            listModel.Add(item);

            string json = JsonConvert.SerializeObject(listModel, Formatting.Indented);

            StreamWriter sw = new StreamWriter("..\\YmlEditor\\data.json");

            sw.WriteLine(json);
            sw.Close();

            return item;
        }

        public ViewModel CreateFromExcel(ViewModel item)// Создание
        {
            //var newItem = mRepoProject.Create(item.Item);
            /*foreach (var product in listModel)
            {
                product.Id
            }*/
            /*listModel.Max<ViewModel, Id>;*/
            
            listModel.Add(item);

            string json = JsonConvert.SerializeObject(listModel, Formatting.Indented);

            StreamWriter sw = new StreamWriter("..\\YmlEditor\\data.json");

            sw.WriteLine(json);
            sw.Close();

            return item;
        }

        public ViewModel Update(ViewModel model)
        {
            var x = listModel.FirstOrDefault(x => x.Id == model.Id);
            listModel.Remove(x);
            x.Name = model.Name;
            x.Price = model.Price;
            x.Url = model.Url;
            x.PopularProduct = model.PopularProduct;
            x.InStock = model.InStock;
            x.Count = model.Count;
            x.Description = model.Description;
            x.ShortDescription = model.ShortDescription;
            x.Picture = model.Picture;
            x.Units = model.Units;
            x.Category = model.Category;
            

            listModel.Add(x);

            string json = JsonConvert.SerializeObject(listModel, Formatting.Indented);

            StreamWriter sw = new StreamWriter("..\\YmlEditor\\data.json");

            sw.WriteLine(json);
            sw.Close();

            return x;
        }
    }
}
