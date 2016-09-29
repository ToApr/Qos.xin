﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;

namespace Qos.xin.Common
{

    public static class UploadHelper
    {
        public class UploadStatus{

            public bool Status { get; set; }
            public string Result { get; set; }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file">文件对象</param>
        /// <param name="FilePath">文件保存路径(相对网站根目录路径)</param>
        /// <param name="AllowFileType">允许上传的文件类型</param>
        /// <returns>返回上传状态及上传后的新文件路径</returns>
        public static UploadStatus Upload(HttpPostedFile file, string FilePath, string[] AllowFileType)
        {
            var US = new UploadStatus();
            string extend = Path.GetExtension(file.FileName).ToLower();
            string url = string.Empty;
            if (AllowFileType.Contains(extend.ToLower()))
            { 
                string FileName = (DateTime.Now - new DateTime(1970, 1, 1)).Ticks.ToString() + "_" + new Random().Next(10000, 99999).ToString();
                switch (file.ContentType)
                {
                    case "application/pdf":
                    case "application/msword":
                    case "application/vnd.ms-excel":
                        file.SaveAs(HttpContext.Current.Server.MapPath("/").TrimEnd('\\')+"\\"+FilePath.Replace('/','\\').TrimEnd('\\')+"\\"+ FileName+extend);
                        US.Result=FilePath.TrimEnd('\\') + "/" + FileName + extend;
                        US.Status = true;
                        break;
                    case "image/jpeg":
                    case "image/jpg":
                        var image = System.Drawing.Image.FromStream(file.InputStream);
                        CreateThumbnail(file, FilePath, extend, image.Width / 4, image.Height / 4, FileName + "_small", ZoomType.缩略图);
                        US.Result = CreateThumbnail(file, FilePath, extend, image.Width, image.Height, FileName, ZoomType.不变形);
                        US.Status = true;
                        break;
                    default:
                        US.Result = "未知的Mime-Type";
                        US.Status = false;
                        break;
                }
            }
            else
            {
                US.Status = false;
                US.Result = "文件格式不允许!";
            }
            return US;
        }
        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="file"></param>
        /// <param name="_Width"></param>
        /// <param name="_Height"></param>
        /// <returns></returns>
        public static string Upload(HttpPostedFileBase file, int _Width, int _Height)
        {


            string extend = Path.GetExtension(file.FileName).ToLower();
            string FileName = (DateTime.Now - new DateTime(1970, 1, 1)).Ticks.ToString() + "_" + new Random().Next(10000, 99999).ToString();
            string carimg = "";
            if (extend == ".jpg" || extend == ".png" || extend == ".gif" || extend == ".jpeg")
            {
                carimg = CreateThumbnail(file, "upload/images/", extend, _Width, _Height, FileName, ZoomType.不变形);
                CreateThumbnail(file, "upload/images/", extend, _Width / 4, _Height / 4, FileName + "_small", ZoomType.缩略图);
            }
            return carimg;
        }
        public static string Upload(string Image, string[] AllowFileType)
        {
            string expend = Image.Substring(0, 5).Trim();
            if (AllowFileType.Contains(expend.ToLower()))
            {

                byte[] bytes = Convert.FromBase64String(Image.Substring(5));
                MemoryStream ms = new MemoryStream(bytes);
                Bitmap b = new Bitmap(ms);
                string FileName = (DateTime.Now - new DateTime(1970, 1, 1)).Ticks.ToString() + "_" + new Random().Next(10000, 99999).ToString() + expend;
                string path = HttpContext.Current.Server.MapPath("/").Trim('\\') + "\\upload\\images\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                b.Save(path + FileName);
                b.Dispose();
                return "/upload/images/" + FileName;
            }
            return "";
        }
        public static string UploadPortrait(string Image, string[] AllowFileType)
        {
            string expend = Image.Substring(0, 5).Trim();
            if (AllowFileType.Contains(expend.ToLower()))
            {

                byte[] bytes = Convert.FromBase64String(Image.Substring(5));
                MemoryStream ms = new MemoryStream(bytes);
                Bitmap b = new Bitmap(ms);
                string FileName = (DateTime.Now - new DateTime(1970, 1, 1)).Ticks.ToString() + "_" + new Random().Next(10000, 99999).ToString() + expend;
                string path = HttpContext.Current.Server.MapPath("/").Trim('\\') + "\\upload\\Portrait\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                b.Save(path + FileName);
                b.Dispose();
                return "/upload/Portrait/" + FileName;
            }
            return "";
        }
        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="uploadObject">上传的HttpPostedFile或图片物理路径</param>
        /// <param name="uploaddir">上传文件夹相对路径</param>
        /// <param name="ext">后缀（如：.jpg）</param>
        /// <param name="t_width">缩略图宽</param>
        /// <param name="t_height">缩略图高</param>
        /// <param name="filename">文件夹，不含路径和后缀</param>
        /// <param name="str">枚举类-缩略图的样式</param>
        /// <returns>返回生成图片的路径</returns>
        public static string CreateThumbnail(object uploadObject, string uploaddir, string ext, int t_width, int t_height, string filename, ZoomType zoomType)
        {
            System.Drawing.Image thumbnail_image = null;
            System.Drawing.Image original_image = null;
            System.Drawing.Bitmap final_image = null;
            System.Drawing.Graphics graphic = null;
            MemoryStream ms = null;
            string ThumbnailFilename = "";
            try
            {
                if (uploadObject is HttpPostedFileBase)
                {
                    HttpPostedFileBase jpeg_image_upload = uploadObject as HttpPostedFileBase;
                    original_image = System.Drawing.Image.FromStream(jpeg_image_upload.InputStream);
                }
                else if (uploadObject is HttpPostedFile)
                {
                    HttpPostedFile jpeg_image_upload = uploadObject as HttpPostedFile;
                    original_image = System.Drawing.Image.FromStream(jpeg_image_upload.InputStream);
                }
                else
                {
                    string jpeg_image_upload = uploadObject as string;
                    original_image = System.Drawing.Image.FromFile(jpeg_image_upload);
                }
                // Calculate the new width and height
                int original_paste_x = 0;
                int original_paste_y = 0;
                int original_width = original_image.Width;//截取原图宽度
                int original_height = original_image.Height;//截取原图高度
                int target_paste_x = 0;
                int target_paste_y = 0;
                int target_width1 = t_width;
                int target_height1 = t_height;
                switch (zoomType)
                {
                    case ZoomType.缩略图:
                        float target_ratio = (float)t_width / (float)t_height;//缩略图 宽、高的比例
                        float original_ratio = (float)original_width / (float)original_height;//原图 宽、高的比例

                        if (target_ratio > original_ratio)//宽拉长
                        {
                            target_height1 = t_height;
                            target_width1 = (int)Math.Floor(original_ratio * (float)t_height);
                        }
                        else
                        {
                            target_height1 = (int)Math.Floor((float)t_width / original_ratio);
                            target_width1 = t_width;
                        }

                        target_width1 = target_width1 > t_width ? t_width : target_width1;
                        target_height1 = target_height1 > t_height ? t_height : target_height1;
                        target_paste_x = (t_width - target_width1) / 2;
                        target_paste_y = (t_height - target_height1) / 2;
                        break;
                    case ZoomType.不变形:
                        target_ratio = (float)t_width / (float)t_height;//缩略图 宽、高的比例
                        original_ratio = (float)original_width / (float)original_height;//原图 宽、高的比例

                        if (target_ratio > original_ratio)//宽拉长
                        {
                            original_height = (int)Math.Floor((float)original_width / target_ratio);
                        }
                        else
                        {
                            original_width = (int)Math.Floor((float)original_height * target_ratio);
                        }
                        original_paste_x = (original_image.Width - original_width) / 2;
                        original_paste_y = (original_image.Height - original_height) / 2;
                        break;
                    case ZoomType.不变形中心放大:
                        original_paste_x = (original_width - target_width1) / 2;
                        original_paste_y = (original_height - target_height1) / 2;
                        if (original_height > target_height1) original_height = target_height1;
                        if (original_width > target_width1) original_width = target_width1;
                        break;
                    default:

                        break;
                }
                final_image = new System.Drawing.Bitmap(t_width, t_height);
                graphic = System.Drawing.Graphics.FromImage(final_image);
                graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High; /* new way */
                graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphic.Clear(Color.White);//背景
                Rectangle SrcRec = new Rectangle(original_paste_x, original_paste_y, original_width, original_height);
                Rectangle targetRec = new Rectangle(target_paste_x, target_paste_y, target_width1, target_height1);
                graphic.DrawImage(original_image, targetRec, SrcRec, GraphicsUnit.Pixel);
                string saveFileName = uploaddir + filename + ext;
                using (FileStream fs = new FileStream(HttpContext.Current.Server.MapPath("/" + saveFileName), FileMode.Create))
                {

                    final_image.Save(fs, System.Drawing.Imaging.ImageFormat.Jpeg);
                    ThumbnailFilename = "/" + saveFileName;
                }
            }
            catch (Exception ex)
            {
                // If any kind of error occurs return a 500 Internal Server error
                HttpContext.Current.Response.StatusCode = 500;
                HttpContext.Current.Response.Write(ex);
                HttpContext.Current.Response.End();
            }
            finally
            {
                // Clean up
                if (final_image != null) final_image.Dispose();
                if (graphic != null) graphic.Dispose();
                if (original_image != null) original_image.Dispose();
                if (thumbnail_image != null) thumbnail_image.Dispose();
                if (ms != null) ms.Close();
            }
            return ThumbnailFilename;
        }

        public enum ZoomType { 缩略图 = 0, 不变形, 不变形中心放大 }

    }
}
