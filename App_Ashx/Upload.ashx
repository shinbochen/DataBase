<%@ WebHandler Language="C#" Class="Upload" Debug="true" %>

using System;
using System.Web;
using System.IO;
using System.Globalization;
using System.Configuration;
using System.Reflection;

using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

/// <summary>
/// 上传图像文件到服务器
/// </summary>

public class Upload : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {

        //虚拟目录，建议写在配置文件中
        String          strPath = "upload\\image\\";
        String          parentPath = ConfigurationManager.ConnectionStrings["parentDic"].ToString();
        //HttpPostedFile  imgFile = context.Request.Files["imgfile"];
        HttpPostedFile  imgFile = context.Request.Files[0];
        String          fileExt;

        //fileExt = "{"+String.Format("success:true,path:\"{0}\"", strPath.Replace("\\", "\\\\") + "shinbo.jpg")+"}";
        
        //context.Response.Write( fileExt );
        //return;
        
        
        //取出文件扩展名
        if (imgFile != null)
        {
            fileExt = Path.GetExtension(imgFile.FileName).ToLower();
            
            //String newFileName = imgFile.FileName + DateTime.Now.ToString("_yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + fileExt;
            String newFileName = Path.GetFileNameWithoutExtension(imgFile.FileName).ToLower() + DateTime.Now.ToString("_MMdd_ffff", DateTimeFormatInfo.InvariantInfo) + fileExt;
            String filePath = parentPath +"\\"+ strPath + newFileName;

            imgFile.SaveAs(filePath);

            fileExt = "{" + String.Format("success:true,path:\"{0}\"", strPath.Replace("\\", "\\\\") + newFileName) + "}";
            context.Response.Write(fileExt);
        }
        else
        {
            context.Response.Write("{success:false, msg:'file is null'}");        
        }
        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}