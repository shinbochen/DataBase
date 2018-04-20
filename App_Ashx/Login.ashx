<%@ WebHandler Language="C#" Class="Login" %>

using System;
using System.IO;
using System.Data;
using System.Web;
using System.Collections.Generic;
using System.Configuration;

using System.Data.SqlClient;
/// <summary>
/// 处理LOGIN
/// </summary>

public class Login : IHttpHandler {

    private Result result;
    
    public void ProcessRequest (HttpContext context) {
        
        context.Response.ContentType = "text/plain";
        result = new Result();

        
        string user = context.Request.QueryString.Get("username") != null ? context.Request.QueryString.Get("username") : "";
        string psd = context.Request.QueryString.Get("password") != null ? context.Request.QueryString.Get("password") : "";

        if (user.Length == 0)
        {
            user = context.Request.Form.Get("username") != null ? context.Request.Form.Get("username") : "";
        }
        if (psd.Length == 0)
        {
            psd = context.Request.Form.Get("password") != null ? context.Request.Form.Get("password") : "";
        }
        if (user.Length == 0 || psd.Length == 0)
        {
            result.SetMsg("用户名与密码不能为空");
        }
        else
        {
            
            ADODB db = new ADODB();
            if (db.Open())
            {

                String sql = String.Format("select * from sm_user where _user='{0}' and _psd='{1}'", user, psd);


                DataTable dt = db.exec_dataset(sql);

                List<Dictionary<string, object>> lst;
                List<Dictionary<string, object>> lst2;

                if (dt != null)
                {
                    int _len = dt.Rows.Count;
                    if (_len > 0)
                    {
                        result.SetFlag(true);
                        lst = JSONHelper.DataTableToList(dt);

                        lst2 = this.getSystemData( db );
                        
                        if ( (lst2 != null) && (lst2.Count > 0) )
                        {
                            //遍历字典  
                            foreach (KeyValuePair<string, object> kvp in lst2[0] )
                            {
                                if (kvp.Key != "id")
                                {
                                    lst[0].Add(kvp.Key, kvp.Value);
                                }
                            } 

                        }                       
                       
                        
                        result.SetData( lst );
                        result.SetTotal(_len);
                    }
                    else
                    {
                        result.SetMsg(" 用户或密码不对！");
                    }
                    
                }
                db.Close();
            }
            else
            {
                result.SetMsg("Open failed!");
            }
        }
        context.Response.Write(JSONHelper.ObjectToJSON(result));
        return;
    }

    public List<Dictionary<string, object>> getSystemData(ADODB db)
    {

        List<Dictionary<string, object>> lst= null;
        
        string sql = String.Format("select * from sm_system");

        DataTable dt = db.exec_dataset(sql);
        if ( (dt != null) && (dt.Rows.Count > 0) )
        {
           lst = JSONHelper.DataTableToList(dt);
        }
        return lst;

    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}