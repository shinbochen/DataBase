<%@ WebHandler Language="C#" Class="GetPicList" %>

using System;
using System.Web;
using System.Collections.Generic;
using System.Configuration;

/// <summary>
/// 取得服务器的图像列表
/// </summary>
public class GetPicList : IHttpHandler
{
    
    public void ProcessRequest (HttpContext context) {

        Result result = new Result();
        String strPath = "upload\\image";
        String parentPath = ConfigurationManager.ConnectionStrings["parentDic"].ToString();

        String dicPath = parentPath + "\\" + strPath;;


        context.Response.ContentType = "text/plain";
        
        
        List<Dictionary<string,object>> lst = FileList.getAllFile(dicPath, strPath);

        result.SetFlag(true);

        result.SetData(lst);
        result.SetTotal(lst.Count);        
        context.Response.Write( JSONHelper.ObjectToJSON(result) );
            
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}