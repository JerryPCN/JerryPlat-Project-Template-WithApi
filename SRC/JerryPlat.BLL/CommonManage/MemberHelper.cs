using JerryPlat.DAL;
using JerryPlat.Models;
using JerryPlat.Models.Dto;
using JerryPlat.Utils.Helpers;
using JerryPlat.Utils.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace JerryPlat.BLL.CommonManage
{
    public class MemberHelper<TSession> : BaseSessionHelper<Member, TSession>
        where TSession : class
    {
        public IQueryable<Member> GetQueryableMemberList(SearchModel searchModel)
        {
            IQueryable<Member> memberQuerable = GetDbSet<Member>().AsNoTracking();

            if (searchModel.Id != 0)
            {
                memberQuerable = memberQuerable.Where(o => o.RefereeId == searchModel.Id);
            }

            if (searchModel.StartTime.HasValue)
            {
                memberQuerable = memberQuerable.Where(o => o.JoinTime >= searchModel.StartTime.Value);
            }

            if (searchModel.EndTime.HasValue)
            {
                memberQuerable = memberQuerable.Where(o => o.JoinTime < searchModel.EndTime.Value);
            }

            if (!string.IsNullOrEmpty(searchModel.SearchText))
            {
                memberQuerable = memberQuerable.Where(o => o.Name.Contains(searchModel.SearchText) | o.NickName.Contains(searchModel.SearchText));
            }

            return memberQuerable;
        }
        
        public async Task<PageData<Member>> GetTeamListAsync<TKey>(SearchModel searchModel, Expression<Func<Member, TKey>> orderByKeySelector, PageParam pageParam, bool bIsAscOrder = true)
        {
            IQueryable<Member> memberQuerable = GetQueryableMemberList(searchModel);
            PageData<Member> pageData = await PageHelper.GetPageDataAsync(memberQuerable, orderByKeySelector, pageParam, bIsAscOrder, searchModel.Sort);
            return pageData;
        }

        public async Task<List<Member>> GetTeamListAsync<TKey>(SearchModel searchModel, Expression<Func<Member, TKey>> orderByKeySelector, bool bIsAscOrder = true)
        {
            IQueryable<Member> memberQuerable = GetQueryableMemberList(searchModel);
            List<Member> memberList = await PageHelper.GetDataAsync(memberQuerable, orderByKeySelector, bIsAscOrder, searchModel.Sort);
            return memberList;
        }

        public async Task<Member> GetByShareCodeAsync(string strShareCode)
        {
            return await _Db.Members.Where(o => o.ShareCode == strShareCode).FirstOrDefaultAsync();
        }
        
        public async Task SetMemberReferee(string strShareCode)
        {
            Member member = _Session as Member;
            await SetMemberReferee(member, strShareCode);
        }

        public async Task SetMemberReferee(Member member, string strShareCode)
        {
            if (member.RefereeId > 0)
            {
                if (member.Id == member.RefereeId)
                {
                    member.RefereeId = 0;
                    await SaveAsync(member);
                }

                return;
            }

            Member referee = await GetByShareCodeAsync(strShareCode);

            if (referee == null || member.Id == referee.Id)
            {
                return;
            }

            member.RefereeId = referee.Id;
            
            await SaveAsync(member);
        }

        public async Task SetLocation(LocationDto location)
        {
            Member member = _Session as Member;
            member.Latitude = location.Latitude;
            member.Longitude = location.Longitude;
            member.Precision = location.Accuracy;
            
            await SaveAsync(member);
        }

        public async Task<bool> BindAsync(BindDto model)
        {
            Member member = _Session as Member;
            
            member.Name = model.Name;

            return await SaveAsync(member);
        }

        public override async Task<bool> SaveAsync(Member member)
        {
            bool bIsSaveSuccessfully = await base.SaveAsync(member);

            if (bIsSaveSuccessfully)
            {
                SetSession(member);
            }

            return bIsSaveSuccessfully;
        }

        public override async Task<bool> DeleteAsync(int intMemberId)
        {
            _Db.Members.Where(o => o.RefereeId == intMemberId)
                .ToList().ForEach(item => item.RefereeId = 0);

            _Db.Members.RemoveRange(_Db.Members.Where(o => o.Id == intMemberId));
            await _Db.SaveChangesAsync();
            return true;
        }
    }

    public class AdminMemberHelper : MemberHelper<AdminUser> { }

    public class MobMemberHelper : MemberHelper<Member> { }
}