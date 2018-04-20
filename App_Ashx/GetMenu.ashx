<%@ WebHandler Language="C#" Class="GetMenu" %>

using System;
using System.IO;
using System.Data;
using System.Web;
using System.Collections.Generic;
using System.Configuration;

using System.Data.SqlClient;
/// <summary>
///  取菜单数据
/// </summary>

public class GetMenu : IHttpHandler
{     
    public void ProcessRequest (HttpContext context) {

        context.Response.ContentType = "text/plain" ;

        int permission = context.Request.QueryString.Get("permission") != null ? Int32.Parse(context.Request.QueryString.Get("permission")) : 0;
        
        Menu menu =  new Menu();
        menu.setPermission(permission);
        menu.LoadMenu();
        context.Response.Write(JSONHelper.ObjectToJSON(menu));        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }
}