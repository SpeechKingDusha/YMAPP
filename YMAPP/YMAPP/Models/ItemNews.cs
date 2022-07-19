using System.ComponentModel;

namespace YMAPP.Models
{
    public class ItemNews: INotifyPropertyChanged
    {
        static public byte CountNews { get; private set; }
        public byte IdItemNews { get; set; }
        public string Author { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public string MinText { get; set; }
        public string FullTextHtml { get; set; }
        public string LinkFullMaterial { get; set; }

        //В конструкторе считается общее кол-во созданных экземпляров, а так же присваивается id экземпляру
        public ItemNews()
        {
            CountNews++;
            IdItemNews = CountNews;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
