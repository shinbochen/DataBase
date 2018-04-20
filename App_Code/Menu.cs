using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

//namespace DataBase{
/// <summary>
///setting 的摘要说明
/// </summary>
public class Menu
{
    public List<Dictionary<string, object>> root;
    private int permission;

	public Menu()
	{
        //lst = new List<Dictionary<string, object>>();
        permission = 0;
	}

    public void setPermission(int n)
    {
        this.permission = n;
    }

    public void LoadMenu(  ){
        
        ADODB db = new ADODB();
        if (db.Open())
        {
            // 顶级菜单
            root = this.LoadMenu(db, "0");
            db.Close();
        }
    }
    /// <summary>
    /// condition
    /// </summary>
    /// <param name="db"></param>
    /// <param name="condition"></param>
    /// <returns></returns>

    public List<Dictionary<string, object>> LoadMenu( ADODB db, string condition)
    {

        List<Dictionary<string, object>>  lst = new List<Dictionary<string, object>>();
        string sql = String.Format("select * from sm_menu where _parent='{0}' and _permission <= {1} order by id asc", condition, this.permission);
        DataTable dt = db.exec_dataset( sql );

        if (dt != null)
        {
             foreach(DataRow dr in dt.Rows)
            {
                Dictionary<string,object> dic =new Dictionary<string,object>();
                foreach(DataColumn dc in dt.Columns)
                {
                    string key, value;
                    int    ivalue;

                    key = dc.ColumnName;

                    switch (key)
                    {
                        case "_text":
                        case "_moduleName":
                        case "_extraParams":
                        case "_xtype":
                            value = dr.IsNull(key) ? "" : (string)dr[key];                                
                            dic.Add( key, value );
                            break;
                        // 字符换为十六进制
                        case "_glyph":
                            ivalue = dr.IsNull(key) ? 0 : Convert.ToInt32((string)dr[key], 16);
                            dic.Add(key, ivalue);
                            break;

                        // 不为0表示有子项
                        case "_child":

                            value = dr.IsNull(key) ? "" : (string)dr[key];
                            if (value.Length > 0 && value != "0")
                            {
                                dic.Add("items", this.LoadMenu(db, value) );
                            }
                            break;
                        default:
                            break;

                    }
                }
                lst.Add( dic );
            }

        }
        return lst;
    }
}