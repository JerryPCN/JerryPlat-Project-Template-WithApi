using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JerryPlat.Utils.Models
{
    public class SearchModel : BaseSearchModel
    {
        public static SearchModel Instance = new SearchModel();

        public int Id1 { get; set; }
        public string SearchText { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
