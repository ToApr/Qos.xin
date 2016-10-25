using System;
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
        public class UploadStatus
        {
            public bool Status { get; set; }
            public List<string> Url { get; set; }
            public string Msg { get; set; }

            public UploadStatus()
            {
                this.Url = new List<string>();
            }
        }
        /// <summary>
        /// 批量上传文件
        /// </summary>
        /// <param name="fileCollection">要上传的文件对象</param>
        /// <param name="FilePath">文件保存路径(相对网站根目录路径)</param>
        /// <param name="AllowFileType">允许上传的文件类型</param>
        /// <returns>返回上传状态及上传后的新文件路径</returns>
        public static UploadStatus Upload(HttpFileCollection fileCollection, string FilePath, string[] AllowFileType)
        {
            var US = new UploadStatus();
            for (int i = 0; i < fileCollection.Count; i++)
			{
		        US=Upload(fileCollection.Get(i), FilePath, AllowFileType);
               if (!US.Status)
               {
                   US.Msg = "图片[" + fileCollection.Get(i).FileName + "]上传失败!";
                   US.Status = false;
                   break;
               }
			} 
            return US;
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
                string _filePath = HttpContext.Current.Server.MapPath("/").TrimEnd('\\') + "\\" + FilePath.Replace('/', '\\').TrimEnd('\\') + "\\";
                if (!Directory.Exists(_filePath))
                    Directory.CreateDirectory(_filePath);
                switch (extend)
                {
                    case ".pdf":
                    case ".doc":
                    case ".docx":
                    case ".xls":
                    case ".xlsx":
                    case ".mp4":
                    case ".mov":
                    case ".mp3":
                        file.SaveAs(_filePath+ FileName+extend);
                        US.Url.Add(FilePath.TrimEnd('\\') + "/" + FileName + extend);
                        US.Status = true;
                        break;
                    case ".jpeg":
                    case ".jpg":
                    case ".png":
                    case ".gif":
                    case ".bmp":
                        var image = System.Drawing.Image.FromStream(file.InputStream);
                        CreateThumbnail(file, FilePath, extend, image.Width / 4, image.Height / 4, FileName + "_small", ZoomType.缩略图);
                        US.Url.Add(CreateThumbnail(file, FilePath, extend, image.Width, image.Height, FileName, ZoomType.不变形));
                        US.Status = true;
                        break;
                    default:
                        US.Msg = "未知的文件类型!";
                        US.Status = false;
                        break;
                }
            }
            else
            {
                US.Status = false;
                US.Msg = "文件格式不允许!";
            }
            return US;
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file">文件对象</param>
        /// <param name="FilePath">文件保存路径(相对网站根目录路径)</param>
        /// <param name="AllowFileType">允许上传的文件类型</param>
        /// <returns>返回上传状态及上传后的新文件路径</returns>
        public static UploadStatus Upload(HttpPostedFileBase file, string FilePath, string[] AllowFileType)
        {
            var US = new UploadStatus();
            string extend = Path.GetExtension(file.FileName).ToLower();
            string url = string.Empty;
            if (AllowFileType.Contains(extend.ToLower()))
            {
                string FileName = (DateTime.Now - new DateTime(1970, 1, 1)).Ticks.ToString() + "_" + new Random().Next(10000, 99999).ToString();
                string _filePath = HttpContext.Current.Server.MapPath("/").TrimEnd('\\') + "\\" + FilePath.Replace('/', '\\').TrimEnd('\\') + "\\";
                if (!Directory.Exists(_filePath))
                    Directory.CreateDirectory(_filePath);
                switch (extend)
                {
                    case ".pdf":
                    case ".doc":
                    case ".docx":
                    case ".xls":
                    case ".xlsx":
                    case ".mp4":
                    case ".mov":
                    case ".mp3":
                        file.SaveAs(_filePath + FileName + extend);
                        US.Url.Add(FilePath.TrimEnd('\\') + "/" + FileName + extend);
                        US.Status = true;
                        break;
                    case ".jpeg":
                    case ".jpg":
                    case ".png":
                    case ".gif":
                    case ".bmp":
                        var image = System.Drawing.Image.FromStream(file.InputStream);
                        CreateThumbnail(file, FilePath, extend, image.Width / 4, image.Height / 4, FileName + "_small", ZoomType.缩略图);
                        US.Url.Add(CreateThumbnail(file, FilePath, extend, image.Width, image.Height, FileName, ZoomType.不变形));
                        US.Status = true;
                        break;
                    default:
                        US.Msg = "未知的文件类型!";
                        US.Status = false;
                        break;
                }
            }
            else
            {
                US.Status = false;
                US.Msg = "文件格式不允许!";
            }
            return US;
        }
        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="file">文件对象</param>
        /// ><param name="FilePath">文件相对网站根目录的路径</param>
        /// <param name="AllowFileType">允许上传的文件类型</param>
        /// <param name="_Width">图像宽度</param>
        /// <param name="_Height">图像高度</param>
        /// <returns>返回上传状态及上传后的新文件路径</returns>
        public static UploadStatus Upload(HttpPostedFileBase file, string FilePath, string[] AllowFileType, int _Width, int _Height)
        {
            var US = new UploadStatus();
            string extend = Path.GetExtension(file.FileName).ToLower();
            string FileName = (DateTime.Now - new DateTime(1970, 1, 1)).Ticks.ToString() + "_" + new Random().Next(10000, 99999).ToString();
            if (AllowFileType.Contains(extend.ToLower()))
            { 
                US.Url.Add(CreateThumbnail(file,FilePath, extend, _Width, _Height, FileName, ZoomType.不变形));
                US.Status = true;
                CreateThumbnail(file, "FilePath", extend, _Width / 4, _Height / 4, FileName + "_small", ZoomType.缩略图);
            }
            else
            {
                US.Status = false;
                US.Msg = "文件格式不允许!";
            }
            return US;
        }
        /// <summary>
        /// base64上传文件方法,前5位为文件扩展名
        /// </summary>
        /// <param name="Image">base64编码的图片</param>
        /// <param name="FilePath">文件相对网站根目录的路径 eg:/upload/images</param>
        /// <param name="AllowFileType">允许上传的文件类型</param>
        /// <returns>返回上传状态及上传后的新文件路径</returns>
        public static UploadStatus Upload(string Image, string FilePath, string[] AllowFileType)
        {
            var US = new UploadStatus();
            string expend = Image.Substring(0, 5).Trim();
            if (AllowFileType.Contains(expend.ToLower()))
            {
                byte[] bytes = Convert.FromBase64String(Image.Substring(5));
                MemoryStream ms = new MemoryStream(bytes);
                Bitmap b = new Bitmap(ms);
                string FileName = (DateTime.Now - new DateTime(1970, 1, 1)).Ticks.ToString() + "_" + new Random().Next(10000, 99999).ToString() + expend;
                string path = HttpContext.Current.Server.MapPath("/" + FilePath);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                b.Save(path.Trim('/')+"/"+ FileName);
                b.Dispose();
                US.Url.Add(FilePath.Trim('/')+"/"+ FileName);
                US.Status = true;
            }
            else
            {
                US.Status = false;
                US.Msg = "文件格式不允许!";
            }
            return US;
        }
        /// <summary>
        /// base64上传头像方法
        /// </summary>
        /// <param name="Image">base64上传文件法,前5位为文件扩展名</param>
        /// <param name="ImagePath">文件相对网站根目录的路径</param>
        /// <param name="AllowFileType">允许上传的文件类型</param>
        /// <returns>返回上传状态及上传后的新文件路径</returns>
        public static UploadStatus UploadPortrait(string Image, string ImagePath, string[] AllowFileType)
        {
            var US = new UploadStatus();
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
                US.Status = true;
                US.Url.Add(ImagePath.Trim('/') + FileName);
            }
            else
            {
                US.Status = false;
                US.Msg = "文件格式不允许!";
            }
            return US;
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
                string saveFileName = filename + ext;
                string path = HttpContext.Current.Server.MapPath("/" +  uploaddir);
                if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
                using (FileStream fs = new FileStream(path.TrimEnd('\\')+"\\"+saveFileName, FileMode.Create))
                {

                    final_image.Save(fs, System.Drawing.Imaging.ImageFormat.Jpeg);
                    ThumbnailFilename =uploaddir.TrimEnd('/')+ "/" + saveFileName;
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
