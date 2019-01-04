using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JerryPlat.Utils.Models
{
    public class PageSearchModel
    {
        public SearchModel SearchModel { get; set; }
        public PageParam PageParam { get; set; }

        public PageSearchModel()
        {
            SearchModel = new SearchModel();
            PageParam = new PageParam();
        }
    }
}
