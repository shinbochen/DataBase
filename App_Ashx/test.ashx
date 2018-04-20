<%@ WebHandler Language="C#" Class="test" %>

using System;
using System.Web;
using System.Collections.Generic;


public class test : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {

        //ClientSocket s = new ClientSocket();

        //string str = s.Send("192.168.1.112", 8868);


        //List<string> lst = FileList.getAllFile("F:\\apache_www_root\\sourceManager\\upload\\image", "\\upload\\image");
        
        //context.Response.ContentType = "text/plain";
        //context.Response.Write("Hello World");
        //context.Response.Write( JSONHelper.ObjectToJSON(lst) );
            
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}