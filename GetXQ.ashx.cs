using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUserInfo
{
    /// <summary>
    /// GetXQ 的摘要说明
    /// </summary>
    public class GetXQ : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            MSG msg = new MSG();
            msg.Code = "200";
            DBOpration db = new DBOpration();
            List<XQEnity> list = new List<XQEnity>();
            list = db.GetXQ();
            msg.Data =list;
            context.Response.ContentType = "text/plain";
            context.Response.Write(JsonHelper.GetJson<MSG>(msg));
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