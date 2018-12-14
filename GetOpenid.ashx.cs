using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace WebUserInfo
{
    /// <summary>
    /// GetOpenid 的摘要说明
    /// </summary>
    public class GetOpenid : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string rescode = context.Request["rescode"];
            Encoding encoding = Encoding.Default;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.weixin.qq.com/sns/jscode2session");
            request.Method = "post";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/x-www-form-urlencoded";
            string strPostdata = "appid=wx081e0a630eead8c0&secret=a153753165f050d5ccce625d1c2c08e0&js_code="+rescode+"&grant_type=authorization_code";
            byte[] buffer = encoding.GetBytes(strPostdata);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string result;
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("UTF-8")))
            {
                result= reader.ReadToEnd();
            }
            context.Response.ContentType = "text/plain";
            context.Response.Write(result);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}