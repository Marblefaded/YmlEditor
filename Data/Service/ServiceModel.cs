﻿using Newtonsoft.Json.Linq;
using YmlEditor.Data.ViewModels;

namespace YmlEditor.Data.Service
{
    public class ServiceModel
    {
        public List<ViewModel> GetAll() // Запарсить json
        {
            string line = "";
            StreamReader sr = new StreamReader("..\\data.json");

            line = sr.ReadToEnd();

            sr.Close();

            ViewModel model = new ViewModel();
            List<ViewModel> listModel = new List<ViewModel>();

            JObject file = JObject.Parse(line);
            int n = 0;

            while (true)
            {
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


                        }
                        else
                        {
                            break;
                        }
                        listModel.Add(model);
                        n++;
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
            return listModel;
        }

        /*private static ViewModel Convert(Pets Model) // Конверт (хз надо или нет)
        {
            var item = new ZooViewModel(Model);
            return item;
        }*/

        /*public ZooViewModel Update(ZooViewModel model)// Редактирование
        {
            var x = mRepoProject.FindById(model.PetsId);
            x.Name = model.Name;
            x.TypeOfAnimal = model.TypeOfAnimal;

            return Convert(mRepoProject.Update(x, model.Item.RowVersion));
        }

        public ZooViewModel Create(ZooViewModel item)// Создание
        {
            var newItem = mRepoProject.Create(item.Item);

            return Convert(newItem);
        }

        public DutyViewModel Remove(DutyViewModel model)// Удаление
        {
            repoDuty.Remove(model.Item);
            return null;
        }*/
    }
}
