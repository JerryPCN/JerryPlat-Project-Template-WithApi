using System.Collections.Generic;

namespace JerryPlat.Models.Dto
{
    public class GroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OrderIndex { get; set; }

        public List<int> NavigationIdList { get; set; }
        
        public GroupDto()
        {
            NavigationIdList = new List<int>();
        }
    }
}