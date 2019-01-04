using AutoMapper;
using JerryPlat.DAL;
using JerryPlat.Models;
using JerryPlat.Models.Dto;
using JerryPlat.Utils.Helpers;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace JerryPlat.BLL.AdminManage
{
    public class GroupHelper : AdminBaseSessionHelper<Group, GroupDto>
    {
        public override async Task<bool> SaveAsync(GroupDto model)
        {
            Group group = Mapper.Map<Group>(model);
            if (model.Id == 0)
            {
                GetDbSet<Group>().Add(group);
                await SaveChangesAsync();
            }
            else
            {
                Attach(group, EntityState.Modified);
            }

            var navRoleDb = GetDbSet<Role>();
            List<Role> navRoleList = navRoleDb.Where(o => o.GroupId == group.Id).ToList();
            navRoleDb.RemoveRange(navRoleList);

            model.NavigationIdList.ForEach(o =>
            {
                navRoleDb.Add(new Role
                {
                    GroupId = group.Id,
                    NavigationId = o
                });
            });

            //CacheHelper.RemoveCache(key => key.StartsWith("Navigation"));

            return await SaveChangesAsync();
        }
    }
}