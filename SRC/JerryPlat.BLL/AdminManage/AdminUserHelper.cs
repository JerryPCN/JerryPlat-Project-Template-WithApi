using AutoMapper;
using JerryPlat.DAL;
using JerryPlat.Models;
using JerryPlat.Models.Dto;
using JerryPlat.Utils.Helpers;
using JerryPlat.Utils.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace JerryPlat.BLL.AdminManage
{
    public class AdminUserHelper : AdminBaseSessionHelper<AdminUser, AdminUserDto>
    {
        public AdminUser Get(LoginModel loginModel)
        {
            return Get(loginModel.UserName, loginModel.Password);
        }

        public AdminUser Get(string strUserName, string strPassword)
        {
            return _Db.AdminUsers.Where(o => o.UserName == strUserName & o.Password == strPassword).FirstOrDefault();
        }

        public async Task<AdminUser> GetAsync(LoginModel loginModel)
        {
            return await GetAsync(loginModel.UserName, loginModel.Password);
        }

        public Task<AdminUser> GetAsync(string strUserName, string strPassword)
        {
            return _Db.AdminUsers.Where(o => o.UserName == strUserName & o.Password == strPassword).FirstOrDefaultAsync();
        }

        public override async Task<bool> SaveAsync(AdminUserDto modelDto)
        {
            AdminUser model = Mapper.Map<AdminUser>(modelDto);
            if (!PageHelper.IsAdd(model))
            {
                AdminUser adminUser = await GetByIdAsync<AdminUser>(model.Id);
                if (adminUser == null)
                {
                    return true;
                }

                if (adminUser.Password != model.Password)
                {
                    model.Password = EncryptHelper.Encrypt(model.Password);
                }
            }
            else
            {
                model.Password = EncryptHelper.Encrypt(model.Password);
            }

            return await SaveAsync(model);
        }
        
        public async Task<bool> ChangePassword(PasswordDto model)
        {
            if (model.Password != model.Confirm)
            {
                return false;
            }

            if (IsNullSession())
            {
                return false;
            }

            AdminUser adminUser = _Session;
            if (adminUser.Password != EncryptHelper.Encrypt(model.Original))
            {
                return false;
            }

            adminUser.Password = EncryptHelper.Encrypt(model.Password);

            Attach<AdminUser>(adminUser, EntityState.Modified);

            bool bIsSave = await SaveChangesAsync();
            if (bIsSave)
            {
                SetSession(adminUser);
            }
            return bIsSave;
        }
    }
}