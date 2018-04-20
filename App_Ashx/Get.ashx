<%@ WebHandler Language="C#" Class="Get" %>

using System;
using System.IO;
using System.Data;
using System.Web;
using System.Collections.Generic;
using System.Configuration;

using System.Data.SqlClient;
//using DataBase;

/// <summary>
/// 可以修改，删除，新增数据到数据库中
/// </summary>
public class Get : IHttpHandler {

    private string table;
    private string action;
    private string keycol;

    private string condition;
    private string sql;
    private string json;
    private string query;

    private int start;
    private int page;
    private int limit;
    private Result result;
    
    
    // condition = n1:v;n2:v2
    
    public void GetParam( HttpContext context ){
        try
        {
            table = context.Request.QueryString.Get("table") != null ? context.Request.QueryString.Get("table") : "";
            action = context.Request.QueryString.Get("action") != null ? context.Request.QueryString.Get("action").ToLower() : "";
            keycol = context.Request.QueryString.Get("keycol") != null ? context.Request.QueryString.Get("keycol") : "";
            condition = context.Request.QueryString.Get("condition") != null ? context.Request.QueryString.Get("condition") : "";
            query = context.Request.QueryString.Get("query") != null ? context.Request.QueryString.Get("query") : "";

            start = context.Request.QueryString.Get("start") != null ? Int32.Parse(context.Request.QueryString.Get("start")): 0;
            page = context.Request.QueryString.Get("page") != null ? Int32.Parse(context.Request.QueryString.Get("page")): 1;
            limit = context.Request.QueryString.Get("limit") != null ? Int32.Parse(context.Request.QueryString.Get("limit")): 20;

            if (context.Request.HttpMethod.ToLower() == "post")
            {
                StreamReader reader = new StreamReader(context.Request.InputStream);
                json = HttpUtility.UrlDecode(reader.ReadToEnd());
            }
            
            
        }
        catch (Exception ex)
        {
            System.Console.Write(ex.Message);
            return;
        }
    }

    public string getcondition(ADODB db)
    {
        string result="";
        string querystr = "";

        if (condition.Length > 0)
        {
            String [] arr = condition.Split(';');

            foreach (string val in arr)
            {
                if (val.Length > 0)
                {
                    string[] temp = val.Split(':');
                    if (result.Length > 0)
                    {
                        result += " AND ";
                    }
                    result += String.Format("{0}={1}", temp[0], temp[1]);
                }                
            }
        }
        querystr = getquerypara(db);
        if (querystr.Length > 0)
        {
            if( result.Length > 0 ){
                
                result +=  " AND ";
            }
            result += String.Format("( {0} )", querystr );
        }
        
        return result;
    }

    public string getquerypara( ADODB db )
    {
        string result = "";
        if (query.Length > 0)
        {
            List<string> lst = db.getTableColName(table);
            if (lst.Count > 0)
            {
                foreach (string colname in lst)
                {
                    if (colname == "id")
                    {
                        continue;
                    }
                    if (result.Length > 0)
                    {

                        result += " OR ";
                    }
                    result += String.Format("{0} like '%{1}%'", colname, query);
                }
            }
        }
        return result;
    }
    /// <summary>
    /// 返回假，不需要外面再处理数据
    /// </summary>
    /// <param name="context"></param>
    /// <param name="db"></param>
    /// <returns></returns>
    public Boolean ProcessExcel(HttpContext context, ADODB db)
    {
        sql = String.Format("select * from {0}", table);
        if (condition.Length > 0 || query.Length > 0)
        {
            sql += String.Format(" where {0}", getcondition(db));
        }
        DataTable dt = db.exec_dataset(sql);


        sql = String.Format("select * from sm_setting_field where tf_moduleName ='{0}' ORDER BY id", table);
        DataTable dtcol = db.exec_dataset(sql);



        if (dt != null && dtcol != null)
        {
           Excel.CreateExcel(context.Response, dtcol, dt, table+".xls");
           return false;
        }
        return true;
    }
    
    public void ProcessGet(HttpContext context, ADODB db)
    {
        sql = String.Format("select * from {0}", table);
        if (condition.Length > 0 || query.Length > 0)
        {
            sql += String.Format(" where {0}", getcondition( db) );
        }

        DataTable dt = db.exec_dataset(sql);   
        
        if (dt != null)
        {
            int _len = dt.Rows.Count;
            int _start;
            _start = (page - 1) * limit;
            result.SetFlag(true);
            result.SetData(JSONHelper.DataTableToList(dt, _start, limit));
            result.SetTotal(_len);
        }
        else
        {
            result.SetMsg("no result");
        }
        
        return;
    }

    public void ProcessAdd(HttpContext context, ADODB db )
    {
        if (json.Length > 0)
        {
            Bean bean = new Bean();

            bean.SetTable(table);
            bean.SetKeyCol("id");
            bean.ParseJson(json);
            if (bean.execSQL(db, "add") > 0)
            {
                result.SetFlag(true);
                
                int res = bean.GetLastID(db);                
                List<Dictionary<string, object>>  _lst = new List<Dictionary<string,object>>();                
                Dictionary<string, object> dic = new Dictionary<string,object>();
                
                dic.Add( "id", res );                
                _lst.Add( dic );
                
                result.SetData(_lst);
                  
            }
            result.SetMsg(bean.msql);       
        }
        else
        {
            result.SetMsg("json length equal zero");
        }
        return;

    }
    public void ProcessEdit(HttpContext context, ADODB db)
    {
        if (json.Length > 0)
        {
            Bean bean = new Bean();

            bean.SetTable(table);
            bean.SetKeyCol("id");
            bean.ParseJson(json);
            if (bean.execSQL(db, "edit") > 0)
            {
                result.SetFlag(true);
            }
        }
        else
        {
            result.SetMsg("json length equal zero");
        }
        return;
    }
    public void ProcessDelete(HttpContext context, ADODB db)
    {
        if (json.Length > 0)
        {
            Bean bean = new Bean();

            bean.SetTable(table);
            bean.SetKeyCol("id");
            bean.ParseJson(json);
            if (bean.execSQL(db, "delete") > 0)
            {
                result.SetFlag(true);
            }
        }
        else
        {
            result.SetMsg("json length equal zero");
        }
        return;
    }
    
    public void ProcessRequest (HttpContext context) {

        Boolean flag = true;

        context.Response.ContentType = "text/plain";
        result = new Result();
        GetParam(context);

        ADODB db = new ADODB();
        if (db.Open() )
        {
            if (table.Length > 0)
            {
                switch (action)
                {
                    case "new":
                        ProcessAdd(context, db);
                        break;

                    case "update":
                        ProcessEdit(context, db);
                        break;

                    case "delete":
                        ProcessDelete(context,db);
                        break;

                    case "excel":
                        flag = ProcessExcel(context, db);
                        break;

                    case "read":
                    default:
                        ProcessGet(context,db);
                        break;
                }
            }
            else
            {
                result.SetMsg("NO Table");
            }
            db.Close();
              
        }
        else
        {
            result.SetMsg("Open failed!");
        }
        if (flag)
        {
            context.Response.Write(JSONHelper.ObjectToJSON(result));
        }       
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}