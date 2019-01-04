using JerryPlat.Models;
using JerryPlat.Utils.Helpers;
using System;
using System.Data.Entity.Migrations;

namespace JerryPlat.DAL.Context
{
    public class CreateAndMigrateDatabaseInitializer<TConfiguration> : CreateAndMigrateDatabaseInitializer<JerryPlatDbContext, TConfiguration>
         where TConfiguration : DbMigrationsConfiguration<JerryPlatDbContext>, new()
    {
        protected override void Seed(JerryPlatDbContext context)
        {
            int intId = 1, intSecondParentId = 0, intThirdParentId = 0, intFirstOrderIndex = 1, intSecondOrderIndex = 1, intThirdOrderIndex = 1;
            #region Navigations
            #region 基础设置
            //First
            context.Navigations.AddOrUpdate(new Navigation[] {
                new Navigation { Id = intId++, ParentId = 0, OrderIndex = intFirstOrderIndex++, SiteType = SiteType.Admin,
                    NavigationType =NavigationType.Page, Name = "基础设置", RequestUrl = ""}
            });
            //Second
            intSecondParentId = intId - 1; intSecondOrderIndex = 1;
            context.Navigations.AddOrUpdate(new Navigation[] {
                new Navigation { Id = intId++, ParentId = intSecondParentId, OrderIndex = intSecondOrderIndex++, SiteType = SiteType.Admin,
                    NavigationType =NavigationType.Page, Name = "Banner图管理", RequestUrl = "/Admin/Banner" },
            });
            //Third
            intThirdParentId = intId - 1; intThirdOrderIndex = 1;
            context.Navigations.AddOrUpdate(new Navigation[] {
                new Navigation { Id = intId++, ParentId = intThirdParentId, OrderIndex = intThirdOrderIndex++, SiteType = SiteType.Admin, NavigationType=NavigationType.Button,
                    Name = "查看", Code="View", RequestUrl = "/Admin/Banner" },
                new Navigation { Id = intId++, ParentId = intThirdParentId, OrderIndex = intThirdOrderIndex++, SiteType = SiteType.Admin, NavigationType=NavigationType.Button,
                    Name = "添加", Code="Add", RequestUrl = "/Api/Banner/Add" },
                new Navigation { Id = intId++, ParentId = intThirdParentId, OrderIndex = intThirdOrderIndex++, SiteType = SiteType.Admin, NavigationType=NavigationType.Button,
                    Name = "编辑", Code="Edit", RequestUrl = "/Api/Banner/Edit" },
                new Navigation { Id = intId++, ParentId = intThirdParentId, OrderIndex = intThirdOrderIndex++, SiteType = SiteType.Admin, NavigationType=NavigationType.Button,
                    Name = "删除", Code="Delete", RequestUrl = "/Api/Banner/Delete" },
                new Navigation { Id = intId++, ParentId = intThirdParentId, OrderIndex = intThirdOrderIndex++, SiteType = SiteType.Admin, NavigationType=NavigationType.Button,
                    Name = "批量删除", Code="DeleteList", RequestUrl = "/Api/Banner/DeleteList" },
            });
            #endregion

            #region 权限管理
            //First
            context.Navigations.AddOrUpdate(new Navigation[] {
                new Navigation { Id = intId++, ParentId = 0, OrderIndex = intFirstOrderIndex++, SiteType = SiteType.Admin,
                    NavigationType =NavigationType.Page, Name = "权限管理", RequestUrl = ""}
            });
            //Second
            intSecondParentId = intId - 1; intSecondOrderIndex = 1;
            context.Navigations.AddOrUpdate(new Navigation[] {
                new Navigation { Id = intId++, ParentId = intSecondParentId, OrderIndex = intSecondOrderIndex++, SiteType = SiteType.Admin,
                    NavigationType =NavigationType.Page, Name = "角色管理", RequestUrl = "/Admin/Group" },
            });
            //Third
            intThirdParentId = intId - 1; intThirdOrderIndex = 1;
            context.Navigations.AddOrUpdate(new Navigation[] {
                new Navigation { Id = intId++, ParentId = intThirdParentId, OrderIndex = intThirdOrderIndex++, SiteType = SiteType.Admin, NavigationType=NavigationType.Button,
                    Name = "查看", Code="View", RequestUrl = "/Admin/Group" },
                new Navigation { Id = intId++, ParentId = intThirdParentId, OrderIndex = intThirdOrderIndex++, SiteType = SiteType.Admin, NavigationType=NavigationType.Button,
                    Name = "添加", Code="Add", RequestUrl = "/Api/Group/Add" },
                new Navigation { Id = intId++, ParentId = intThirdParentId, OrderIndex = intThirdOrderIndex++, SiteType = SiteType.Admin, NavigationType=NavigationType.Button,
                    Name = "编辑", Code="Edit", RequestUrl = "/Api/Group/Edit" },
                new Navigation { Id = intId++, ParentId = intThirdParentId, OrderIndex = intThirdOrderIndex++, SiteType = SiteType.Admin, NavigationType=NavigationType.Button,
                    Name = "删除", Code="Delete", RequestUrl = "/Api/Group/Delete" },
                new Navigation { Id = intId++, ParentId = intThirdParentId, OrderIndex = intThirdOrderIndex++, SiteType = SiteType.Admin, NavigationType=NavigationType.Button,
                    Name = "批量删除", Code="DeleteList", RequestUrl = "/Api/Group/DeleteList" },
            });

            //Second
            context.Navigations.AddOrUpdate(new Navigation[] {
                new Navigation { Id = intId++, ParentId = intSecondParentId, OrderIndex = intSecondOrderIndex++, SiteType = SiteType.Admin,
                    NavigationType =NavigationType.Page, Name = "用户管理", RequestUrl = "/Admin/User" },
            });
            //Third
            intThirdParentId = intId - 1; intThirdOrderIndex = 1;
            context.Navigations.AddOrUpdate(new Navigation[] {
                new Navigation { Id = intId++, ParentId = intThirdParentId, OrderIndex = intThirdOrderIndex++, SiteType = SiteType.Admin, NavigationType=NavigationType.Button,
                    Name = "查看", Code="View", RequestUrl = "/Admin/User" },
                new Navigation { Id = intId++, ParentId = intThirdParentId, OrderIndex = intThirdOrderIndex++, SiteType = SiteType.Admin, NavigationType=NavigationType.Button,
                    Name = "添加", Code="Add", RequestUrl = "/Api/User/Add" },
                new Navigation { Id = intId++, ParentId = intThirdParentId, OrderIndex = intThirdOrderIndex++, SiteType = SiteType.Admin, NavigationType=NavigationType.Button,
                    Name = "编辑", Code="Edit", RequestUrl = "/Api/User/Edit" },
                new Navigation { Id = intId++, ParentId = intThirdParentId, OrderIndex = intThirdOrderIndex++, SiteType = SiteType.Admin, NavigationType=NavigationType.Button,
                    Name = "删除", Code="Delete", RequestUrl = "/Api/User/Delete" },
                new Navigation { Id = intId++, ParentId = intThirdParentId, OrderIndex = intThirdOrderIndex++, SiteType = SiteType.Admin, NavigationType=NavigationType.Button,
                    Name = "批量删除", Code="DeleteList", RequestUrl = "/Api/User/DeleteList" },
            });
            #endregion

            #region 系统设置
            //First
            context.Navigations.AddOrUpdate(new Navigation[] {
                new Navigation { Id = intId++, ParentId = 0, OrderIndex = intFirstOrderIndex++, SiteType = SiteType.Admin,
                    NavigationType =NavigationType.Page, Name = "系统设置", RequestUrl = ""}
            });
            //Second
            intSecondParentId = intId - 1; intSecondOrderIndex = 1;
            context.Navigations.AddOrUpdate(new Navigation[] {
                new Navigation { Id = intId++, ParentId = intSecondParentId, OrderIndex = intSecondOrderIndex++, SiteType = SiteType.Admin,
                    NavigationType =NavigationType.Page, Name = "系统设置", RequestUrl = "/Admin/SystemConfig" },
            });
            //Third
            intThirdParentId = intId - 1; intThirdOrderIndex = 1;
            context.Navigations.AddOrUpdate(new Navigation[] {
                new Navigation { Id = intId++, ParentId = intThirdParentId, OrderIndex = intThirdOrderIndex++, SiteType = SiteType.Admin, NavigationType=NavigationType.Button,
                    Name = "查看", Code="View", RequestUrl = "/Admin/SystemConfig" },
                new Navigation { Id = intId++, ParentId = intThirdParentId, OrderIndex = intThirdOrderIndex++, SiteType = SiteType.Admin, NavigationType=NavigationType.Button,
                    Name = "保存", Code="Edit", RequestUrl = "/Api/SystemConfig/Edit" },
                new Navigation { Id = intId++, ParentId = intThirdParentId, OrderIndex = intThirdOrderIndex++, SiteType = SiteType.Admin, NavigationType=NavigationType.Button,
                    Name = "清除数据", Code="Clear", RequestUrl = "/Api/System/ClearData" },
                new Navigation { Id = intId++, ParentId = intThirdParentId, OrderIndex = intThirdOrderIndex++, SiteType = SiteType.Admin, NavigationType=NavigationType.Button,
                    Name = "重启站点", Code="Restart", RequestUrl = "/Api/System/Restart" },
            });
            //Second
            context.Navigations.AddOrUpdate(new Navigation[] {
                new Navigation { Id = intId++, ParentId = intSecondParentId, OrderIndex = intSecondOrderIndex++, SiteType = SiteType.Admin,
                    NavigationType =NavigationType.Page, Name = "开放授权", RequestUrl = "/Admin/OwinConfig" },
            });
            //Third
            intThirdParentId = intId - 1; intThirdOrderIndex = 1;
            context.Navigations.AddOrUpdate(new Navigation[] {
                new Navigation { Id = intId++, ParentId = intThirdParentId, OrderIndex = intThirdOrderIndex++, SiteType = SiteType.Admin, NavigationType=NavigationType.Button,
                    Name = "查看", Code="View", RequestUrl = "/Admin/OwinConfig" },
                new Navigation { Id = intId++, ParentId = intThirdParentId, OrderIndex = intThirdOrderIndex++, SiteType = SiteType.Admin, NavigationType=NavigationType.Button,
                    Name = "添加", Code="Add", RequestUrl = "/Api/OwinConfig/Add" },
                new Navigation { Id = intId++, ParentId = intThirdParentId, OrderIndex = intThirdOrderIndex++, SiteType = SiteType.Admin, NavigationType=NavigationType.Button,
                    Name = "编辑", Code="Edit", RequestUrl = "/Api/OwinConfig/Edit" },
                new Navigation { Id = intId++, ParentId = intThirdParentId, OrderIndex = intThirdOrderIndex++, SiteType = SiteType.Admin, NavigationType=NavigationType.Button,
                    Name = "删除", Code="Delete", RequestUrl = "/Api/OwinConfig/Delete" },
                new Navigation { Id = intId++, ParentId = intThirdParentId, OrderIndex = intThirdOrderIndex++, SiteType = SiteType.Admin, NavigationType=NavigationType.Button,
                    Name = "批量删除", Code="DeleteList", RequestUrl = "/Api/OwinConfig/DeleteList" },
            });
            //Second
            context.Navigations.AddOrUpdate(new Navigation[] {
                new Navigation { Id = intId++, ParentId = intSecondParentId, OrderIndex = intSecondOrderIndex++, SiteType = SiteType.Admin,
                    NavigationType =NavigationType.Page, Name = "数据库管理", RequestUrl = "/Admin/Db" },
            });
            //Third
            intThirdParentId = intId - 1; intThirdOrderIndex = 1;
            context.Navigations.AddOrUpdate(new Navigation[] {
                new Navigation { Id = intId++, ParentId = intThirdParentId, OrderIndex = intThirdOrderIndex++, SiteType = SiteType.Admin, NavigationType=NavigationType.Button,
                    Name = "查看", Code="View", RequestUrl = "/Admin/Db" },
                new Navigation { Id = intId++, ParentId = intThirdParentId, OrderIndex = intThirdOrderIndex++, SiteType = SiteType.Admin, NavigationType=NavigationType.Button,
                    Name = "备份数据库", Code="Backup", RequestUrl = "/Api/Db/Backup" },
                new Navigation { Id = intId++, ParentId = intThirdParentId, OrderIndex = intThirdOrderIndex++, SiteType = SiteType.Admin, NavigationType=NavigationType.Button,
                    Name = "还原数据库", Code="Restore", RequestUrl = "/Api/Db/Restore" }
            });
            #endregion
            #endregion

            intId = 1;
            context.Groups.AddOrUpdate(new Group[] {
                new Group { Id = intId, OrderIndex = intId++, Name = "系统超级管理员" }
            });

            intId = 1;
            context.AdminUsers.AddOrUpdate(new AdminUser[] {
                new AdminUser { Id = intId++, UserName = "admin", Password = EncryptHelper.Encrypt("admin"), GroupId = 1 }
            });
            
            intId = 1;
            context.OwinTokens.AddOrUpdate(new OwinToken[] {
                new OwinToken { Id = intId++, ClientId = "Jerry", ClientSecret = EncryptHelper.Encrypt("Jerry") }
            });
            
            intId = 1;
            context.SystemConfigs.AddOrUpdate(new SystemConfig[] {
                new SystemConfig {Id=intId++, Name="IsUseWechatLogin", Config="False" },
                
                new SystemConfig {Id=intId++, Name="AllowOriginSites", Config="" },

                new SystemConfig {Id=intId++, Name="ShareTitle", Config="JerryPlat分享，联系电话15802775429" },
                new SystemConfig {Id=intId++, Name="ShareContent", Config="JerryPlat分享，联系电话15802775429" },

                new SystemConfig {Id=intId++, Name="MasterDbConnStr", Config="server=.;database=master;uid=sa;pwd=pxj5201314;Persist Security Info=True;Application Name=EntityFramework;" },
                new SystemConfig {Id=intId++, Name="DbName", Config="JerryPlatDB_Match" },
                new SystemConfig {Id=intId++, Name="BackUpDbName", Config="JerryPlatDB_Match_DbBackup" },
            });

            intId = 1;
            context.OwinConfigs.AddOrUpdate(new OwinConfig[]
            {
                new OwinConfig {
                    Id = intId++,
                    Name ="Wechat",
                    AppId ="wx147c289c1e518a3b",
                    AppSecret ="5eaf84c8aa5f51095dfa0e3257eae4a9",
                    RequestUri ="https://open.weixin.qq.com/connect/oauth2/authorize?appid={{AppId}}&redirect_uri={{RedirectUri}}&response_type=code&scope=snsapi_userinfo&state={{State}}#wechat_redirect",
                    AccessTokenUri="https://api.weixin.qq.com/sns/oauth2/access_token?appid={{AppId}}&secret={{AppSecret}}&code={{Code}}&grant_type=authorization_code",
                    UserInfoUri="https://api.weixin.qq.com/sns/userinfo?access_token={{Access_Token}}&openid={{OpenId}}",
                    RedirectUri="http://huawei.logogo.cn/Mob/Owin/Wechat"
                }
            });

#if DEBUG
            intId = 1;
            context.Members.AddOrUpdate(new Member[]
            {
                new Member
                {
                    Id = intId++,
                    OpenId = "11212",
                    NickName = "Jerry",
                    Sex = Sex.Male,
                    Avatar = "/File/Banner/avatar.jpg",
                    ShareCode = "ACDE8D03",
                    Latitude= 28.23f,
                    Longitude = 112.93f,
                    Phone = "15802775429",
                    Name = "Jerry"
                }
            });
#endif
        }
    }
}