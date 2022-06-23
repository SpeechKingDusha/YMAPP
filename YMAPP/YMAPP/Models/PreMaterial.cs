using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace YMAPP.Models
{
    public class PreMaterial : IMaterial
    {
        public int IdMaterial { get; set; }
        public int MaterialsCount { get; set; }
        public string Author { get; set; }
        public string Text { get; set; }
        public string PhotoLink { get; set; }
        public string MaterialLink { get; set; }

        public void GetMaterail(string materialLink)
        {
            var html = @"https://www.ym-penza.ru/";

            HtmlWeb web = new HtmlWeb();

            var htmlDoc = web.Load(html);

            var node = htmlDoc.DocumentNode.SelectSingleNode("//head/title");
        }
    }
}
