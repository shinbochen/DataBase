<%@ WebHandler Language="C#" Class="Module" %>

using System;
using System.IO;
using System.Data;
using System.Web;
using System.Collections.Generic;
using System.Configuration;

using System.Data.SqlClient;
//using DataBase;

public class Module : IHttpHandler
{
     
    public void ProcessRequest (HttpContext context) {

        context.Response.ContentType = "text/plain";        

        Setting setting = new Setting();
        setting.LoadData();
        context.Response.Write(JSONHelper.ObjectToJSON(setting));        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }
}