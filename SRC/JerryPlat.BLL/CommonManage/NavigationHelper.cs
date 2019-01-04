using AutoMapper;
using AutoMapper.QueryableExtensions;
using JerryPlat.DAL;
using JerryPlat.Models;
using JerryPlat.Models.Dto;
using JerryPlat.Utils.Helpers;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace JerryPlat.BLL.CommonManage
{
    public class NavigationHelper<TSession> : BaseSessionHelper<TSession>
        where TSession : class
    {
        #region Cache
        protected List<Navigation> GetButtonCacheList(SiteType siteType)
        {
            string strCacheKey = CacheKey.Navigation.ToString() + siteType.ToString();
            List<Navigation> navigationList = CacheHelper.GetCache<List<Navigation>>(strCacheKey);
            if (navigationList == null)
            {
                navigationList = _Db.Navigations
                    .Where(o => o.SiteType == siteType)
                    .OrderBy(o => o.OrderIndex)
                    .ToList();
                CacheHelper.SetCache(navigationList, strCacheKey);
            }
            return navigationList;
        }

        protected List<NavigationDto> GetNavigationDtoCacheList(SiteType siteType)
        {
            string strCacheKey = CacheKey.NavigationDto.ToString() + siteType.ToString();
            List<NavigationDto> navigationDtoList = CacheHelper.GetCache<List<NavigationDto>>(strCacheKey);
            if (navigationDtoList == null)
            {
                navigationDtoList = _Db.Navigations
                    .Where(o => o.SiteType == siteType)
                    .ProjectTo<NavigationDto>()
                    .OrderBy(o => o.OrderIndex)
                    .ToList();
                CacheHelper.SetCache(navigationDtoList, strCacheKey);
            }
            return navigationDtoList;
        }
        #endregion

        public string GetFirstPageUrl(SiteType siteType, int intGroupId)
        {
            List<Navigation> navigationList = GetList(siteType, intGroupId, NavigationType.Page);
            return GetFirstPageUrl(navigationList);
        }

        public string GetFirstPageUrl(List<Navigation> navigationList)
        {
            return navigationList.Where(o => o.ParentId > 0)
                .OrderBy(o => o.ParentId).ThenBy(o => o.OrderIndex)
                .Select(o => o.RequestUrl).FirstOrDefault();
        }

        public List<ButtonDto> GetButtonList(SiteType siteType, int intGroupId, int intNavigationId)
        {
            List<Navigation> navigationList = GetList(siteType, intGroupId, NavigationType.Button);
            return Mapper.Map<List<ButtonDto>>(navigationList
                .Where(o => o.ParentId == intNavigationId)
                .ToList());
        }

        public List<Navigation> GetList(SiteType siteType, int intGroupId = 1, NavigationType navigationType = NavigationType.All)
        {
            //string strCacheKey = $"{CacheKey.Navigation.ToString()}_{siteType.ToString()}_{navigationType.ToString()}_{intGroupId}";
            List<Navigation> navigationList = null; //CacheHelper.GetCache<List<Navigation>>(strCacheKey);
            if (navigationList == null)
            {
                IQueryable<Navigation> navigationQuerable = _Db.Navigations.AsNoTracking().Where(o => o.SiteType == siteType);
                if (navigationType != NavigationType.All)
                {
                    navigationQuerable = navigationQuerable.Where(o => o.NavigationType == navigationType);
                }

                if (intGroupId == 1)
                {
                    navigationList = navigationQuerable.ToList();
                }
                else
                {
                    navigationList = (from a in _Db.NavigationRoles.Where(o => o.GroupId == intGroupId)
                                      join b in navigationQuerable on a.NavigationId equals b.Id
                                      select b).ToList();
                }
                //CacheHelper.SetCache(navigationList, strCacheKey);
            }
            return navigationList;
        }

        public List<NavigationDto> GetTreeList(SiteType siteType)
        {
            List<NavigationDto> navigationDtoList = GetNavigationDtoCacheList(siteType);

            List<NavigationDto> firstLayerList = navigationDtoList.Where(o => o.ParentId == 0).ToList();

            firstLayerList.ForEach(second => {
                second.Children = navigationDtoList.Where(o => o.ParentId == second.Id & o.NavigationType == NavigationType.Page)
                                .OrderBy(o => o.OrderIndex).ToList();

                second.Children.ForEach(third =>
                {
                    third.Children = navigationDtoList.Where(o => o.ParentId == third.Id & o.NavigationType == NavigationType.Button)
                         .OrderBy(o => o.OrderIndex).ToList();
                });
            });
            return firstLayerList;
        }

        public virtual bool IsValidAjaxRequest(string strRequestUrl)
        {
            return true;
        }
        
        protected bool IsValidAjaxRequest(SiteType siteType, int intGroupId, string strRequestUrl)
        {
            List<Navigation> allButtonList = GetButtonCacheList(siteType);
            if (!allButtonList.Any(o => o.RequestUrl == strRequestUrl))
            {
                return true;
            }
            List<Navigation> roleButtonList = GetList(siteType, intGroupId, NavigationType.Button);
            return roleButtonList.Any(o => o.RequestUrl == strRequestUrl);
        }
    }

    public class AdminNavigationHelper : NavigationHelper<AdminUser>
    {
        public override bool IsValidAjaxRequest(string strRequestUrl)
        {
            return IsValidAjaxRequest(SiteType.Admin, _Session.GroupId, strRequestUrl);
        }
    }
}