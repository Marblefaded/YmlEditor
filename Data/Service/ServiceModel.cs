using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YmlEditor.Data.ViewModels;

namespace YmlEditor.Data.Service
{
    public class ServiceModel
    {
        public List<ViewModel> listModel = new List<ViewModel>();
        public List<ViewModel> GetAll()
        {
            string line = "";
            StreamReader sr = new StreamReader("..\\data.json");

            line = sr.ReadToEnd();

            sr.Close();

            listModel = JsonConvert.DeserializeObject<List<ViewModel>>(line);

            /*JObject file = JObject.Parse(line);
            int n = 0;

            while (true)
            {
                ViewModel model = new ViewModel();
                if (file["products"].AsJEnumerable<JToken> != null)
                {
                    try
                    {
                        if (file["products"][n].AsJEnumerable<JToken> != null)
                        {
                            if (file["products"][n]["Id"].AsJEnumerable<JToken> != null)
                            {
                                model.Id = (string)file["products"][n]["Id"];
                            }
                            else
                            {
                                break;
                            }
                            if (file["products"][n]["Name"].AsJEnumerable<JToken> != null)
                            {
                                model.Name = (string)file["products"][n]["Name"];
                            }
                            else
                            {
                                break;
                            }
                            if (file["products"][n]["Vendor"].AsJEnumerable<JToken> != null)
                            {
                                model.Vendor = (string)file["products"][n]["Vendor"];
                            }
                            else
                            {
                                break;
                            }
                            if (file["products"][n]["Price"].AsJEnumerable<JToken> != null)
                            {
                                model.Price = (string)file["products"][n]["Price"];
                            }
                            else
                            {
                                break;
                            }
                            if (file["products"][n]["CurrencyId"].AsJEnumerable<JToken> != null)
                            {
                                model.CurrencyId = (string)file["products"][n]["CurrencyId"];
                            }
                            else
                            {
                                break;
                            }
                            if (file["products"][n]["CategoryId"].AsJEnumerable<JToken> != null)
                            {
                                model.CategoryId = (string)file["products"][n]["CategoryId"];
                            }
                            else
                            {
                                break;
                            }
                            if (file["products"][n]["Picture"].AsJEnumerable<JToken> != null)
                            {
                                model.Picture = (string)file["products"][n]["Picture"];
                            }
                            else
                            {
                                break;
                            }
                            if (file["products"][n]["Description"].AsJEnumerable<JToken> != null)
                            {
                                model.Description = (string)file["products"][n]["Description"];
                            }
                            else
                            {
                                break;
                            }
                            if (file["products"][n]["ShortDescription"].AsJEnumerable<JToken> != null)
                            {
                                model.ShortDescription = (string)file["products"][n]["ShortDescription"];
                            }
                            else
                            {
                                break;
                            }
                            if (file["products"][n]["Url"].AsJEnumerable<JToken> != null)
                            {
                                model.Url = (string)file["products"][n]["Url"];
                            }
                            else
                            {
                                break;
                            }


                            listModel.Add(model);
                            n++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        break;
                    }

                }
                else
                {
                    break;
                }
            }
            foreach (var item in listModel)
            {
                Console.WriteLine(item.Vendor);
            }*/

            return listModel;

        }

        public ViewModel Remove(ViewModel model)
        {
            listModel.Remove(model);

            string json = JsonConvert.SerializeObject(listModel, Formatting.Indented);

            StreamWriter sw = new StreamWriter("..\\data.json");

            sw.WriteLine(json);
            sw.Close();

            return null;
        }

        public ViewModel Create(ViewModel item)// Создание
        {
            //var newItem = mRepoProject.Create(item.Item);
            listModel.Add(item);

            string json = JsonConvert.SerializeObject(listModel, Formatting.Indented);

            StreamWriter sw = new StreamWriter("..\\data.json");

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
            x.CurrencyId = model.CurrencyId;
            x.Vendor = model.Vendor;
            x.CategoryId = model.CategoryId;
            x.Description = model.Description;
            x.ShortDescription = model.ShortDescription;
            x.Picture = model.Picture;

            listModel.Add(x);

            string json = JsonConvert.SerializeObject(listModel, Formatting.Indented);

            StreamWriter sw = new StreamWriter("..\\data.json");

            sw.WriteLine(json);
            sw.Close();

            return x;
        }
    }
}
