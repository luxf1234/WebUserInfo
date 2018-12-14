using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUserInfo
{
    /// <summary>
    /// UpdateInfo 的摘要说明
    /// </summary>
    public class UpdateInfo : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            UserInfoEnity ui = new UserInfoEnity();
            ui.Xq = context.Request["xq"];
            ui.Username = context.Request["username"];
            ui.Tel = context.Request["tel"];
            ui.Idcard = context.Request["idcard"];
            ui.Carnum = context.Request["carnum"];
            ui.Addr = context.Request["addr"];
            ui.Isidentical =context.Request["identical"];
            ui.Isempty = context.Request["isempty"];
            ui.Isoutcoming = context.Request["isoutcoming"];
            ui.Relationship = context.Request["relationship"];
            ui.Imageurl = context.Request["imageurl"];
            ui.Inserttime = DateTime.Now.ToString();
            MSG msg = new MSG();
            DBOpration db = new DBOpration();
            bool checkidcard = db.CheckIDCard(ui.Idcard);
            if (checkidcard)
            {
                int result = db.InsertInfo(ui);
                
                if (result == 1)
                {
                    msg.Code = "200";
                }
                else
                {
                    msg.Code = "300";
                    msg.Msg = "新增失败";
                }
            }
            else
            {
                msg.Code = "301";
                msg.Msg = "新增失败，身份证号重复";
            }
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