using JerryPlat.DAL;
using JerryPlat.Models;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace JerryPlat.BLL.CommonManage
{
    public class WxTicketHelper : AdminBaseSessionHelper
    {
        public WxTicket GetWxTicket(WxTicketType wxTicketType)
        {
            return _Db.WxTickets.Where(o => o.Key == wxTicketType).FirstOrDefault();
        }

        public bool Save(WxTicketType wxTicketType, WxTicket ticket)
        {
            WxTicket wxTicket = GetWxTicket(wxTicketType);
            if (wxTicket == null)
            {
                wxTicket = new WxTicket();
                wxTicket.Key = wxTicketType;
            }
            wxTicket.Ticket = ticket.Ticket;
            wxTicket.ExpireTime = ticket.ExpireTime;
            return base.Save(wxTicket);
        }
    }
}