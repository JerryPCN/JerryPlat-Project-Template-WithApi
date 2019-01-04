namespace JerryPlat.Models.Dto
{
    public class AdminUserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
    }
}