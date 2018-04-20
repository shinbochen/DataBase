<%@ WebHandler Language="C#" class = "Report"%>

using System;
using System.IO;
using System.Data;
using System.Web;
using System.Collections.Generic;
using System.Configuration;

using System.Data.SqlClient;

public class Report : IHttpHandler {

    private string prog;
    private string keycol;

    private string para;
    private string sql;
    private string json;

    private int start;
    private int page;
    private int limit;
    private Result result;
    
    
    // condition = n1:v;n2:v2
    
    public void GetParam( HttpContext context ){
        try
        {
            prog = context.Request.QueryString.Get("prog") != null ? context.Request.QueryString.Get("prog") : "";
            keycol = context.Request.QueryString.Get("keycol") != null ? context.Request.QueryString.Get("keycol") : "";
            para = context.Request.QueryString.Get("para") != null ? context.Request.QueryString.Get("para") : "";

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

    
    public void ProcessGet(HttpContext context, ADODB db)
    {
        if (para.Length > 0)
        {
            sql = String.Format("exec {0} {1}", prog, para);
        }
        else
        {
            sql = String.Format("exec {0}", prog );
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
        else{
            result.SetMsg("no result");
        }        
        return;
    }
    public void ProcessRequest (HttpContext context) {


        context.Response.ContentType = "text/plain";
        result = new Result();
        GetParam(context);

        ADODB db = new ADODB();
        if (db.Open() )
        {
            if (prog.Length > 0)
            {
                ProcessGet(context,db);
            }
            else
            {
                result.SetMsg("NO prog name");
            }
            db.Close();
        }
        else
        {
            result.SetMsg("Open failed!");
        }              
        context.Response.Write( JSONHelper.ObjectToJSON(result) );          
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}