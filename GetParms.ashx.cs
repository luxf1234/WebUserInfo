using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUserInfo
{
    /// <summary>
    /// GetParms 的摘要说明
    /// </summary>
    public class GetParms : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            List<Parms> parms = new List<Parms>();
            DBOpration db = new DBOpration();
            parms = db.GetParms();
            context.Response.ContentType = "text/plain";
            context.Response.Write(JsonHelper.GetJson<List<Parms>>(parms));
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