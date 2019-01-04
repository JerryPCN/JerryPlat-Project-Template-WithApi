using System.Collections.Generic;

namespace JerryPlat.Models.Dto
{
    public class NavigationDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string RequestUrl { get; set; }
        public int ParentId { get; set; }
        public int OrderIndex { get; set; }
        public SiteType SiteType { get; set; }
        public NavigationType NavigationType { get; set; }
        public List<NavigationDto> Children { get; set; }
    }
}