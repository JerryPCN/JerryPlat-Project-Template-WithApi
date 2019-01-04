using JerryPlat.DAL;
using JerryPlat.Models;
using JerryPlat.Models.Dto;
using JerryPlat.Utils.Helpers;
using JerryPlat.Utils.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace JerryPlat.BLL.CommonManage
{
    public class OwinConfigHelper<TSession> : BaseSessionHelper<OwinConfig, TSession>
        where TSession : class
    {
        private static object objLock = new object();
        private static Dictionary<string, OwinConfig> _KeyValues = new Dictionary<string, OwinConfig>();

        public OwinConfig GetByName(string strName)
        {
            if (!_KeyValues.Keys.Contains(strName))
            {
                lock (objLock)
                {
                    if (!_KeyValues.Keys.Contains(strName))
                    {
                        OwinConfig owinConfig = GetDbSet<OwinConfig>().Where(o => o.Name == strName).FirstOrDefault();
                        if (owinConfig == null)
                        {
                            throw new Exception("Not exist OwinConfig with Name = " + strName);
                        }
                        _KeyValues.Add(strName, owinConfig);
                    }
                }
            }
            return _KeyValues[strName];
        }

        public void ReSet(OwinConfig entity)
        {
            if (_KeyValues.Keys.Contains(entity.Name))
            {
                _KeyValues[entity.Name] = entity;
            }
            else
            {
                _KeyValues.Add(entity.Name, entity);
            }
        }

        public override async Task<bool> SaveAsync(OwinConfig entity)
        {
            if (await base.SaveAsync(entity))
            {
                ReSet(entity);
                return true;
            }
            return false;
        }

        public string GetRequestUri(string strName, bool bIsSaveStateSession = true)
        {
            OwinConfig config = GetByName(strName);
            return GetRequestUri(config, bIsSaveStateSession);
        }

        public string GetRequestUri(OwinConfig config, bool bIsSaveStateSession = true)
        {
            string strState = Guid.NewGuid().ToString().Replace("-", "");
            if (bIsSaveStateSession)
            {
                SessionHelper.Owin.SetSession(strState);
            }
            //https://open.weixin.qq.com/connect/oauth2/authorize?appid={{AppId}}&redirect_uri={{RedirectUri}}&response_type=code&scope=snsapi_userinfo&state={{State}}#wechat_redirect
            //scope：应用授权作用域，snsapi_base （不弹出授权页面，直接跳转，只能获取用户openid），snsapi_userinfo （弹出授权页面，可通过openid拿到昵称、性别、所在地。并且，即使在未关注的情况下，只要用户授权，也能获取其信息）
            //redirect_uri/?code=CODE&state=STATE。若用户禁止授权，则重定向后不会带上code参数，仅会带上state参数redirect_uri?state=STATE
            string strUri = config.RequestUri.Replace("{{State}}", strState);
            return TypeHelper.FillContent<OwinConfig>(config, strUri);
        }

        public string GetAccessTokenUri(string strName, string strCode)
        {
            OwinConfig config = GetByName(strName);
            return GetAccessTokenUri(config, strCode);
        }

        public string GetAccessTokenUri(OwinConfig config, string strCode)
        {
            string strUri = config.AccessTokenUri.Replace("{{Code}}", strCode);
            return TypeHelper.FillContent<OwinConfig>(config, strUri);
        }

        public string GetUserUri(string strName, OwinTokenDto owinTokenDto)
        {
            OwinConfig config = GetByName(strName);
            return GetUserUri(config, owinTokenDto);
        }

        public string GetUserUri(OwinConfig config, OwinTokenDto owinTokenDto)
        {
            string strUri = TypeHelper.FillContent<OwinConfig>(config, config.UserInfoUri);
            return TypeHelper.FillContent<OwinTokenDto>(owinTokenDto, strUri);
        }

        public OwinTokenDto GetTokenDto(string strName, string strCode)
        {
            OwinConfig config = GetByName(strName);
            return GetTokenDto(config, strCode);
        }

        public OwinTokenDto GetTokenDto(OwinConfig owinConfig, string strCode)
        {
            //https://api.weixin.qq.com/sns/oauth2/access_token?appid={{AppId}}&secret={{AppSecret}}&code={{Code}}&grant_type=authorization_code
            string strUri = GetAccessTokenUri(owinConfig, strCode);
            return HttpHelper.Get<OwinTokenDto>(strUri);
        }

        public OwinUserDto GetUserDto(string strName, string strCode)
        {
            OwinConfig config = GetByName(strName);
            return GetUserDto(config, strCode);
        }

        public OwinUserDto GetUserDto(OwinConfig owinConfig, string strCode)
        {
            OwinTokenDto owinTokenDto = GetTokenDto(owinConfig, strCode);
            if (owinTokenDto.ErrCode.HasValue)
            {
                return new OwinUserDto
                {
                    ErrCode = owinTokenDto.ErrCode,
                    ErrMsg = owinTokenDto.ErrMsg
                };
            }

            return GetUserDto(owinConfig, owinTokenDto);
        }

        public OwinUserDto GetUserDto(OwinConfig owinConfig, OwinTokenDto owinTokenDto)
        {
            //https://api.weixin.qq.com/sns/userinfo?access_token=ACCESS_TOKEN&openid=OPENID
            string strUri = GetUserUri(owinConfig, owinTokenDto);
            return HttpHelper.Get<OwinUserDto>(strUri);
        }

        public async Task<bool> Login(string strOwinConfigName, string strOwinState, string strOwinCode, string strSessionName = "Mob")
        {
            MobMemberHelper helper = new MobMemberHelper();

            Member member = null;

            if (!SystemConfigModel.Instance.IsUseWechatLogin)
            {
                member = new Member();
                if (!(await helper.SaveAsync(member)))
                {
                    return false;
                }

                return true;
            }

            if (!SessionHelper.Owin.IsValid(strOwinState))
            {
                return false;
            }

            OwinConfig config = GetByName(strOwinConfigName);
            OwinTokenDto owinTokenDto = GetTokenDto(config, strOwinCode);
            
            if (!owinTokenDto.ErrCode.HasValue)
            {
                member = helper.GetDbSet<Member>().Where(o => o.OpenId == owinTokenDto.OpenId).FirstOrDefault();
                if (member != null)
                {
                    member.Name = "";
                    SetSession(member);
                    return true;
                }
            }

            OwinUserDto owinDto = GetUserDto(config, owinTokenDto);

            if (owinDto.ErrCode.HasValue)
            {
                LogHelper.Error(owinDto.ErrCode + ":" + owinDto.ErrMsg);
                return false;
            }

            if (member == null)
            {
                member = helper.GetDbSet<Member>().Where(o => o.OpenId == owinTokenDto.OpenId).FirstOrDefault();
            }

            if (member == null)
            {
                member = new Member
                {
                    OpenId = owinDto.OpenId,
                };
            }

            member.NickName = owinDto.NickName;
            member.Avatar = owinDto.HeadImgUrl;
            member.Sex = owinDto.Sex == "1" ? Sex.Male : Sex.Male;
            member.ShareCode = await GetShareCodeAsync();

            if (!(await helper.SaveAsync(member)))
            {
                return false;
            }

            string strShareCode = SessionHelper.ShareCode.GetSession<string>();
            if (!string.IsNullOrEmpty(strShareCode))
            {
                await helper.SetMemberReferee(member, strShareCode);
            }

            return true;
        }

        public async Task<bool> IsExistShareCodeAsync(string strShareCode)
        {
            return await _Db.Members.Where(o => o.ShareCode == strShareCode).AnyAsync();
        }

        public async Task<string> GetShareCodeAsync()
        {
            DateTime dateTime = DateTime.Now;
            string strShareCode = string.Empty;
            while (string.IsNullOrEmpty(strShareCode) || await IsExistShareCodeAsync(strShareCode))
            {
                strShareCode = RandomHelper.CreateCode(8);
            }
            return strShareCode;
        }

        public bool Login()
        {
            Member member = _Db.Members.FirstOrDefault();
            if (member == null)
            {
                return false;
            }
            member.Name = "";
            SetSession(member);
            return true;
        }

        public async Task<bool> SetLocation(LocationDto model)
        {
            MobMemberHelper helper = new MobMemberHelper();
            Member member = _Db.Members.Where(o => o.OpenId == model.FromUserName).FirstOrDefault();
            if (member == null)
            {
                member = new Member
                {
                    OpenId = model.FromUserName
                };
            }

            member.Latitude = model.Latitude;
            member.Longitude = model.Longitude;
            member.Precision = model.Precision;

            if (!(await helper.SaveAsync(member)))
            {
                return false;
            }
            SetSession(member);
            return true;
        }
    }
    public class AdminOwinConfigHelper : OwinConfigHelper<AdminUser> { }
    public class MobOwinConfigHelper : OwinConfigHelper<Member> { }
}