using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace YMAPP.Models
{
    public class ItemNews
    {
        public const string URL = @"https://ym-penza.ru";
        const string FILENAMECACHE = "newscache.nc";
        const string FILENAMEHASH = "HashMainPage.hmp";
        static private bool isReadFromFile;
        static private byte[] HashMainPage;
        static public byte CountNews { get; private set; }
        static public List<ItemNews> ListAllNews { get; private set; }
        static public HtmlDocument HtmlDoc { get; private set; }
        public UInt16 IdItemNews { get; set; }
        public string Author { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public string MinText { get; set; }
        public string FullTextHtml { get; private set; }
        public string LinkFullMaterial { get; set; }

        //В конструкторе считается общее кол-во созданных экземпляров, а так же присваивается id экземпляру
        public ItemNews()
        {
            CountNews++;
            IdItemNews = CountNews;
        }

        // Получаем хеш новостей на главной странице
        static private byte[] GetNewHashNL()
        {
            var newsListNodes = HtmlDoc.DocumentNode.SelectSingleNode("//div[contains(@class,'itemListView')]");
            byte[] tmpSource = ASCIIEncoding.ASCII.GetBytes(newsListNodes.InnerHtml.ToString());
            byte[] tmpHash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);
            return tmpHash;
        }

        //Получаем старый хеш новостной ленты
        static private byte[] GetOldHashNL()
        {
            FileInfo fileInf = new FileInfo(FILENAMEHASH);
            if (!fileInf.Exists) return null;
            byte[] tmpHash = new byte[fileInf.Length];
            using (BinaryReader reader = new BinaryReader(File.Open(FILENAMEHASH, FileMode.Open)))
            {
                for (byte i = 0; i < fileInf.Length; i++)
                {
                    tmpHash[i] = reader.ReadByte();
                }
            }
            return tmpHash;
        }

        //Сравниваем новый и старый хеши.
        static private bool ComparatorHash(byte[] newHash, byte[] oldHash)
        {
            if (oldHash == null || newHash.Length != oldHash.Length) return false;
            for (byte i = 0; i < newHash.Length; i++)
            {
                if (newHash[i] != oldHash[i]) return false;
            }
            return true;
        }

        //Сохраняем полученный хеш новостной ленты в файл.
        static private void SaveHash()
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(FILENAMEHASH, FileMode.OpenOrCreate)))
            {
                writer.Write(HashMainPage);
            }
        }

        //Асинхронный метод. Сохраняет спарсенные новости в файл.
        static public async void SaveNewsCacheAsync(List<ItemNews> listNews)
        {
            using (StreamWriter writer = new StreamWriter(FILENAMECACHE, false))
            {
                await writer.WriteLineAsync(CountNews.ToString());
            }
            using (StreamWriter writer = new StreamWriter(FILENAMECACHE, true))
            {
                foreach (ItemNews itemNews in listNews)
                {
                    await writer.WriteLineAsync(itemNews.IdItemNews.ToString());
                    await writer.WriteLineAsync(itemNews.Author);
                    await writer.WriteLineAsync(itemNews.Date);
                    await writer.WriteLineAsync(itemNews.Name);
                    await writer.WriteLineAsync(itemNews.MinText);
                    await writer.WriteLineAsync(itemNews.LinkFullMaterial);
                }
            }
        }

        //Парсит полный текст матераила в виде html для дальнейшей передачи в webview 
        public void GetFullMaterial()
        {
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(LinkFullMaterial);

            var node = htmlDoc.DocumentNode.SelectSingleNode("//div[contains(@class,'itemFullText')]");
            FullTextHtml = ResizeImage(node.InnerHtml);
        }

        //Находит в коде, полученный через GetFullMaterial() фиксированные размеры изображения
        //и меняет их на 100% по ширине. Код размера по высоте удалеят из тега.
        private string ResizeImage(string Code)
        {
            const string NEWSIZE = "100%";
            int DELETECODEHEIGHT = 12 + NEWSIZE.Length;
            for (int i = 0; i < Code.Length - 4; i++)
            {
                if (Code[i] == 'w' && Code[i + 1] == 'i' && Code[i + 2] == 'd' && Code[i + 3] == 't' && Code[i + 4] == 'h')
                {
                    i += 7;
                    Code = Code.Insert(i, NEWSIZE);
                    Code = Code.Remove(i + NEWSIZE.Length, DELETECODEHEIGHT);
                }

            }
            return Code;
        }

        //Удаляет из спарсенного текста символы табуляции, перехода на новую строку, и лишнии пробелы
        static string CleanText(string text)
        {
            text = text.Replace("\t", "");
            text = text.Replace("\n", "");
            text = text.Replace("  ", "");
            return text;
        }
        //Вытягивает только правильную ссылку на полный текст материала
        static string GetLink(string link, string Url)
        {
            link = CleanText(link);
            StringBuilder text = new StringBuilder(Url, 50);
            byte step = 0;
            for (byte i = 0; i < link.Length; i++)
            {
                if (step == 1) text.Append(link[i]);
                if (step == 2) break;
                if (link[i] == '\"') ++step; ;
            }
            text = text.Replace("\"", "");
            return text.ToString();
        }

        //Создает список новостей из полученных материалов. Работает на уровне класса.
        static public List<ItemNews> GetListNewsFromWeb()
        {
            List<ItemNews> listNews = new List<ItemNews>();
            var newsListNodes = HtmlDoc.DocumentNode.SelectNodes("//div[contains(@class,'itemContainer itemContainerLast')]");
            foreach (var node in newsListNodes)
            {
                ItemNews Material = new ItemNews();
                var innerText = new HtmlDocument();
                innerText.LoadHtml(node.InnerHtml);
                var PieceNews = innerText.DocumentNode.SelectSingleNode("//div[contains(@class, 'catItemIntroText')]/p");
                Material.MinText = CleanText(PieceNews.InnerText);
                PieceNews = innerText.DocumentNode.SelectSingleNode("//div[contains(@class, 'catItemHeader')]/h3");
                Material.Name = CleanText(PieceNews.InnerText);
                PieceNews = innerText.DocumentNode.SelectSingleNode("//span[contains(@class, 'catItemDateCreated')]");
                Material.Date = CleanText(PieceNews.InnerText);
                PieceNews = innerText.DocumentNode.SelectSingleNode("//div[contains(@class, 'catItemAuthor')]");
                Material.Author = CleanText(PieceNews.InnerText);
                PieceNews = innerText.DocumentNode.SelectSingleNode("//h3[contains(@class, 'catItemTitle')]");
                Material.LinkFullMaterial = GetLink(PieceNews.InnerHtml.ToString(), URL);
                //Material.GetFullMaterial();
                listNews.Add(Material);
            }
            SaveHash();
            return listNews;
        }
        //Создает список новостей ранеей сохраненный в файл. Работает на уровне класса.
        static public List<ItemNews> GetListNewsFromCeche()
        {
            List<ItemNews> listNews = new List<ItemNews>();
            using (StreamReader reader = new StreamReader(FILENAMECACHE))
            {
                CountNews = Byte.Parse(reader.ReadLine());
                byte count = CountNews;
                for (byte i = 0; i < count; i++)
                {
                    ItemNews itemNews = new ItemNews();
                    itemNews.IdItemNews = UInt16.Parse(reader.ReadLine());
                    itemNews.Author = reader.ReadLine();
                    itemNews.Date = reader.ReadLine();
                    itemNews.Name = reader.ReadLine();
                    itemNews.MinText = reader.ReadLine();
                    itemNews.LinkFullMaterial = reader.ReadLine();

                    listNews.Add(itemNews);
                }
                CountNews = count;
            }
            return listNews;
        }

        //Определяет через какой метод получить список материалов. Работает на уровне класса.
        static public void GetListNews(bool isReadFile)
        {
            FileInfo fileInf = new FileInfo(FILENAMECACHE);
            if (isReadFile && fileInf.Exists) ListAllNews = GetListNewsFromCeche();
            else ListAllNews = GetListNewsFromWeb();
        }

        //Инициализирует класс. Работает на уровне класса.
        static public void Initialization()
        {
            CountNews = 0;
            isReadFromFile = false;
            HtmlWeb web = new HtmlWeb();
            HtmlDoc = web.Load(URL);
            HashMainPage = GetNewHashNL();
            byte[] old = GetOldHashNL();
            FileInfo fileInf = new FileInfo(FILENAMECACHE);
            isReadFromFile = ComparatorHash(HashMainPage, old);
            GetListNews(isReadFromFile);
            if (!isReadFromFile || !fileInf.Exists) SaveNewsCacheAsync(ListAllNews);
            SaveHash();
        }
    }
}
