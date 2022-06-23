using System;
using System.Collections.Generic;
using System.Text;

namespace YMAPP.Models
{
    interface IMaterial
    {
        int IdMaterial { get; set; }
        int MaterialsCount { get; set; }
        string Author { get; set; }
        string Text { get; set; }
        string PhotoLink { get; set; }
        string MaterialLink { get; set; }
        void GetMaterail(string materialLink);
        
    }
}
