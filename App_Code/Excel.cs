using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;

/// <summary>
///Excel 的摘要说明
/// </summary>
public class Excel
{
    public Excel()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }
    public static void CreateExcel(HttpResponse resp, DataTable dtcol, DataTable dt, string FileName)  
    {
        string          headers= "", 
                        items=""; 
        List<string>    lstkey = new List<string>();

        
        resp.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312"); 
        resp.AppendHeader("Content-Disposition", "attachment;filename=" + FileName);    

        

        // 写表头
        foreach(DataRow row in dtcol.Rows)
        {
            string key, title;

            key = row.IsNull( "tf_fieldName") ? "" : (string) row["tf_fieldName"];
            title = row.IsNull( "tf_title") ? "" : (string) row["tf_title"];
            if( key.Length ==0 || key == "id"){
                continue;
            }
            lstkey.Add( key);
            headers += title + '\t';
        }
        headers += "\n";
        resp.Write(headers);  

        // 写内容
        foreach(DataRow row in dt.Rows){
            string item="";

            items = "";
            foreach( string key in lstkey ){
                item = row.IsNull(key) ? "" : row[key].ToString();
                items += item + "\t";
            }
            items += "\n";
            resp.Write( items ); 
        }
       resp.End(); 
    }
}
