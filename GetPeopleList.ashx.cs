using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUserInfo
{
    /// <summary>
    /// GetPeopleList 的摘要说明
    /// </summary>
    public class GetPeopleList : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string xqid = context.Request["xqid"];
            string startdate = context.Request["startdate"];
            string enddate = context.Request["enddate"];
            DBOpration db = new DBOpration();
            List<UserInfoEnity> list_ui = new List<UserInfoEnity>();
            list_ui = db.GetPeopleInfoList(xqid,startdate,enddate);
            context.Response.ContentType = "text/plain";
            context.Response.Write(JsonHelper.GetJson<List<UserInfoEnity>>(list_ui));
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