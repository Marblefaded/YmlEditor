namespace YmlEditor.Data.Service
{
    public class ServiceModel
    {
        /*public List<ZooViewModel> GetAll() // Запарсить json
        {
            var list = mRepoProject.Get().ToList();
            var result = list.Select(Convert).ToList();

            return result;
        }

        private static ZooViewModel Convert(Pets Model) // Конверт (хз надо или нет)
        {
            var item = new ZooViewModel(Model);
            return item;
        }

        public ZooViewModel Update(ZooViewModel model)// Редактирование
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
