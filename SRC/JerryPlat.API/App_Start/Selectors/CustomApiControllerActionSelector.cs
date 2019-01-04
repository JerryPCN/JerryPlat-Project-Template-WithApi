using JerryPlat.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace JerryPlat.API.App_Start.Selectors
{
    //Not Used
    public class CustomApiControllerActionSelector: ApiControllerActionSelector
    {
        private string GetRountePrefix(Type type)
        {
            object objValue = type.GetCustomAttributeData(typeof(RoutePrefixAttribute).Name);

            if (objValue == null)
            {
                return string.Empty;
            }
            return "/" + objValue.ToString();
        }

        private HttpActionDescriptor GetHttpActionDescriptor(HttpControllerContext controllerContext, 
            ILookup<string, HttpActionDescriptor> actionMapping)
        {
            Type type = controllerContext.Controller.GetType();
            string strRoutePrefix = GetRountePrefix(type);
            object objRoute = null;
            string strRouteTemplate = string.Empty;
            foreach (var mapping in actionMapping)
            {
                foreach (var actionDescriptor in mapping)
                {
                    objRoute = type.GetCustomAttributeData(actionDescriptor.ActionName, typeof(RouteAttribute).Name);
                    if (objRoute == null)
                    {
                        continue;
                    }

                    strRouteTemplate = $"{strRoutePrefix}/{objRoute.ToString()}";
                    if (IsMatchedAction(controllerContext.Request.RequestUri.AbsolutePath, strRouteTemplate))
                    {
                        return actionDescriptor;
                    }
                }
            }
            
            return base.SelectAction(controllerContext);
        }

        private bool IsMatchedAction(string strUrl, string strRouteTemplate)
        {
            strUrl = strUrl.ToLower();
            strRouteTemplate = strRouteTemplate.ToLower();

            string strRegex = GetRouteTemplateRegex(strRouteTemplate);
            
            return Regex.IsMatch(strUrl, strRegex);
        }

        private string GetRouteTemplateRegex(string strRouteTemplate)
        {
            MatchCollection matches = RegexHelper.RegexRouteParam.Matches(strRouteTemplate);
            string strRegex = string.Empty;
            string[] aryValue = null;
            foreach(Match match in matches)
            {
                aryValue = match.Value.Substring(1, match.Value.Length - 2).Split(':');
                if (aryValue.Length == 2 && aryValue[1].ToLower() == "int")
                {
                    strRegex = @"[1-9]\d*";
                }
                else
                {
                    strRegex = @"\S*";
                }
                strRouteTemplate = strRouteTemplate.Replace(match.Value, strRegex);
            }
            return strRouteTemplate;
        }
        

        public override HttpActionDescriptor SelectAction(HttpControllerContext controllerContext)
        {
            ILookup<string, HttpActionDescriptor> actionMapping = 
                GetActionMapping(controllerContext.ControllerDescriptor);
            
            HttpActionDescriptor decriptor = GetHttpActionDescriptor(controllerContext, actionMapping);
          
            return decriptor;
        }
    }
}