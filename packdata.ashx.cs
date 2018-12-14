using GemBox.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUserInfo
{
    /// <summary>
    /// packdata 的摘要说明
    /// </summary>
    public class packdata : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            
            string xqid = context.Request["xqid"];
            string startdate = context.Request["startdate"];
            string enddate = context.Request["enddate"];
            DBOpration db = new DBOpration();
            List<UserInfoEnity> list_ui = new List<UserInfoEnity>();
            list_ui = db.GetPeopleInfoList(xqid, startdate, enddate);
            List<string> imagepaths = new List<string>();
            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            ExcelFile excel = new ExcelFile();
            ExcelWorksheet sheet = excel.Worksheets.Add("sheet1");
            sheet.Rows[0].Cells[0].Value = "序号";
            sheet.Rows[0].Cells[1].Value = "小区";
            sheet.Rows[0].Cells[2].Value = "姓名";
            sheet.Rows[0].Cells[3].Value = "身份证号";
            sheet.Rows[0].Cells[4].Value = "车辆信息";
            sheet.Rows[0].Cells[5].Value = "地址信息";
            sheet.Rows[0].Cells[6].Value = "户主关系";
            sheet.Rows[0].Cells[7].Value = "是否流动人口";
            sheet.Rows[0].Cells[8].Value = "是否人户一致";
            sheet.Rows[0].Cells[9].Value = "是否有户无人";
            sheet.Rows[0].Cells[10].Value = "图片信息";
            sheet.Rows[0].Cells[11].Value = "新增时间";
            string currentpath = HttpContext.Current.Server.MapPath("~");
            for (int i=0;i<list_ui.Count;i++)
            {
                sheet.Rows[i+1].Cells[0].Value = i;
                sheet.Rows[i + 1].Cells[1].Value = list_ui[i].Xq;
                sheet.Rows[i + 1].Cells[2].Value =list_ui[i].Username;
                sheet.Rows[i + 1].Cells[3].Value = list_ui[i].Idcard;
                sheet.Rows[i + 1].Cells[4].Value =list_ui[i].Carnum;
                sheet.Rows[i + 1].Cells[5].Value = list_ui[i].Addr;
                sheet.Rows[i + 1].Cells[6].Value = list_ui[i].Relationship;
                sheet.Rows[i + 1].Cells[7].Value = list_ui[i].Isoutcoming;
                sheet.Rows[i + 1].Cells[8].Value =list_ui[i].Isidentical;
                sheet.Rows[i + 1].Cells[9].Value = list_ui[i].Isempty;
                sheet.Rows[i + 1].Cells[10].Value = list_ui[i].Imagename;
                if (list_ui[i].Imageurl != "")
                {
                    imagepaths.Add(currentpath + list_ui[i].Imageurl);
                }
                sheet.Rows[i + 1].Cells[11].Value = list_ui[i].Inserttime;
            }
            
            string path =  "/tmp/file/" + DateTime.Now.ToString("yyyyMMddHHmmss")+".xls";
            string filepath="/tmp/file/" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".zip";
            excel.Save(currentpath+path);
            imagepaths.Add(currentpath + path);
            ZipHelper zh = new ZipHelper();
            bool result= zh.ZipManyFilesOrDictorys(imagepaths, currentpath + filepath, "");
            context.Response.ContentType = "text/plain";
            context.Response.Write(filepath);
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