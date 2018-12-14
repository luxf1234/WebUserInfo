using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace WebUserInfo
{
    /// <summary>
    /// UploadImage 的摘要说明
    /// </summary>
    public class UploadImage : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            MSG msg = new MSG();
            try
            {
                HttpPostedFile file = HttpContext.Current.Request.Files["file"];
                
                if (file != null)
                {
                    string key = context.Request["user"];
                    string serialnumber = DateTime.Now.ToString("yyyyMMddHHmmss") + Guid.NewGuid().ToString("N");
                    string filename = serialnumber + ".jpg";
                    string smallname = "S_" + filename;
                    string currentpath = HttpContext.Current.Server.MapPath("~");
                    string path = "/tmp/" + DateTime.Now.ToString("yyyyMMdd")+"/";
                    ImageEnity image = new ImageEnity();
                    image.Serialnum = serialnumber;
                    image.Imagename = filename;
                    image.Imagepath = path + filename;
                    image.Simagepath = path + smallname;
                    image.Isrelated = "0";
                    image.Openid = key;
                    DBOpration db = new DBOpration();
                    int imageid = db.InsertImage(image);
                    if (imageid > 0)
                    {
                        Stream sr = file.InputStream;        //文件流
                        if (!Directory.Exists(currentpath+path))
                        {
                            DirectoryInfo dir = new DirectoryInfo(currentpath + path);
                            dir.Create();
                        }
                        Image.FromStream(sr).Save(currentpath+path + filename);
                        if (sr != null) sr.Close();
                        MakeThumbnail(currentpath + path + filename, currentpath + path + smallname, 60, 80, "HW");
                        //bitmap.Save(currentpath + path);
                        msg.Code = "200";
                        msg.Msg = serialnumber;
                    }
                    else
                    {
                        msg.Code = "300";
                        msg.Msg = "图片提交失败";
                    }
                    
                    
                }
                else
                {
                    msg.Code = "300";
                    msg.Msg = "图片未提交";
                }
            }
            catch (Exception ex)
            {
                msg.Code = "300";
                msg.Msg = "图片提交失败";
            }
            context.Response.ContentType = "text/plain";
            context.Response.Write(JsonHelper.GetJson<MSG>(msg));

        }

        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode)
        {
            Image originalImage = Image.FromFile(originalImagePath);

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形）                 
                    break;
                case "W"://指定宽，高按比例                     
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例 
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形）                 
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片 
            Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板 
            Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法 
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度 
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充 
            g.Clear(Color.White);

            //在指定位置并且按指定大小绘制原图片的指定部分 
            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
                new Rectangle(x, y, ow, oh),
                GraphicsUnit.Pixel);

            try
            {
                //以jpg格式保存缩略图 
                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
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