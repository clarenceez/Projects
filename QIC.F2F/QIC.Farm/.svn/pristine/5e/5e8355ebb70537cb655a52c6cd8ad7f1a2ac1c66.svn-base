using Newtonsoft.Json;
using QIC.Infrastructure.Common.HttpRequest;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace QIC.Farm.WebApi.Common
{
    public class WechatHelper
    {
        private static string WechatApiUrl = "https://api.weixin.qq.com";
        private static string AppID = ConfigurationManager.AppSettings["appid"];
        private static string Secret = ConfigurationManager.AppSettings["secret"];
        public static string GetWechatOpenID(string code)
        {
            string urlFormatter = WechatApiUrl + "/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code";
            

            var url = string.Format(urlFormatter, AppID, Secret, code);
            var result = UserWebRequest.Request(url, false, RequestMethod.GET, "");

            var dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
            if (dic.ContainsKey("errcode"))
            {
                return "";
            }

            string openID = dic["openid"];
            return openID;
        }

        public static string GetAccessToken()
        {
            string accessToken = string.Empty;
            var url = WechatApiUrl + string.Format("/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", AppID, Secret);
            var result = UserWebRequest.Request(url, false, RequestMethod.GET, "");
            var dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
            if (dic.ContainsKey("access_token"))
            {
                accessToken = dic["access_token"];
            }
            return accessToken;
        }


        public static string GetWechatNickNameByOpenID(string openID)
        {
            string nickname = string.Empty;
            string accessToken = GetAccessToken();
            string url = WechatApiUrl + string.Format("/cgi-bin/user/info?access_token={0}&openid={1}", accessToken, openID);
            var result = UserWebRequest.Request(url, false, RequestMethod.GET, "");
            Dictionary<string, object> userInfo = JsonConvert.DeserializeObject<Dictionary<string, object>>(result);
            
            if(userInfo.ContainsKey("nickname"))
            {
                nickname = userInfo["nickname"].ToString();
            }
            return nickname;
        }
    }
}