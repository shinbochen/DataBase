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
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;

/// <summary>
///Bean 的摘要说明
/// </summary>
/// 
public class Bean
{
    private List<Dictionary<string, object>> lst; //保存数据
    private String table;
    private String keycol;                      //默认为id
    public String msql;

    /// <summary>
    /// 
    /// </summary>
    public Bean()
    {
        lst = null;
        keycol = "id";
        table = "sm_test";
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="_table"></param>
    public void SetTable(string _table)
    {
        table = _table;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="keycol"></param>
    public void SetKeyCol(string _keycol)
    {
        keycol = _keycol;
    }
    /// <summary>
    ///  返回受影响的条数
    /// </summary>
    /// <param name="db"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public int execSQL(ADODB db, string action/*, HttpContext context*/)
    {

        int result = -1;
        string sql = "";

        if (lst != null)
        {
            action = action.ToLower();

            switch (action)
            {
                case "add":  //增加 应该一次只加一条数据
                    foreach (Dictionary<string, object> dic in lst)
                    {
                        sql = getAddSQL(dic);
                    }
                    break;
                case "delete":
                    sql = getDeleteSQL();
                    break;

                case "edit": //修改 应该一次只修改一条数据
                    foreach (Dictionary<string, object> dic in lst)
                    {
                        sql = getUpdateSQL(dic);
                    }
                    break;
                default:
                    break;
            }
            if (sql.Length > 0)
            {
                msql = sql;

                //context.Response.Write(sql);
                result = db.execNonQuery(sql);
            }

        }
        return result;

    }


    public int GetLastID(ADODB db)
    {
        int result = -1;
        string sql = String.Format("SELECT IDENT_CURRENT('{0}')", table);
        DataTable dt = db.exec_dataset(sql);

        if( dt.Rows.Count > 0 ){
            DataRow row = dt.Rows[0];
            decimal temp = row.IsNull(0) ? -1 : (decimal)row[0];
            result = (int)temp;
        }
        return result;
    }
    /* insert into table_bank_information( 
                    tbi_no,
                    tbi_account_name,			
                    tbi_account_no,		
                    tbi_detail,		
                    tbi_remark)	
                values(
                    @tbi_no,
                    @tbi_account_name,			
                    @tbi_account_no,		
                    @tbi_detail,		
                    @tbi_remark)
     * 
     * 
     * 
    
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "insert into sm_vendor(tf_id,tf_name) values(@tf_id, @tf_name)";

        cmd.Parameters.Add(new SqlParameter("@tf_name", tf_name));

        SqlParameter[] sqlParas = new SqlParameter[]{
            new SqlParameter("@sql",SqlDbType.VarChar,50),
            new SqlParameter("@password",tf_name),
            new SqlParameter("@username",tf_name)
        };

        foreach (SqlParameter sp in sqlParas)
        {
            cmd.Parameters.Add(sp);
        }
     */



    public string getAddSQL(Dictionary<string, object> dic)
    {
        SqlCommandBuilder build = new SqlCommandBuilder();

        string column = "", values = "", result = "";

        if (dic != null)
        {
            foreach (string key in dic.Keys)
            {
                try
                {
                    if (key == keycol)
                    {
                        continue;
                    }
                    if (column.Length > 0)
                    {
                        column += ",";
                    }
                    if (values.Length > 0)
                    {
                        values += ",";
                    }

                    column += key;

                    if (dic[key] != null)
                    {

                        switch (dic[key].GetType().ToString())
                        {
                            case "System.String":
                                values += "'" + dic[key].ToString() + "'";
                                break;

                            case "System.Boolean":
                                if ((Boolean)dic[key])
                                {
                                    values += "1";
                                }
                                else
                                {
                                    values += "0";
                                }
                                break;

                            case "System.Int32":
                            default:
                                values += dic[key].ToString();
                                break;
                        }
                    }
                    else
                    {
                        values += "''";
                    }
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                }
            }

            result = String.Format("insert into {0}({1}) values({2})", table, column, values);
        }
        return result;
    }
    /// <summary>
    /// update table set tf_id = '' where tf_id=''
    /// </summary>
    /// <returns></returns>
    public string getUpdateSQL(Dictionary<string, object> dic)
    {
        SqlCommandBuilder build = new SqlCommandBuilder();

        string condition = "", values = "", value = "", result = "";

        if (dic != null)
        {
            foreach (string key in dic.Keys)
            {
                try
                {
                    if (dic[key] != null)
                    {
                        switch (dic[key].GetType().ToString())
                        {
                            case "System.String":
                                value = String.Format("{0}='{1}'", key, dic[key].ToString());
                                break;


                            case "System.Boolean":
                                if ((Boolean)dic[key])
                                {
                                    value = String.Format("{0}=1", key);
                                }
                                else
                                {
                                    value = String.Format("{0}=0", key);
                                }
                                break;

                            case "System.Int32":
                            default:
                                value = String.Format("{0}={1}", key, dic[key].ToString());
                                break;
                        }
                    }
                    else
                    {
                        value = String.Format("{0}=''", key);
                    }



                    if (keycol == key)
                    {
                        condition = value;
                    }
                    else
                    {
                        if (values.Length > 0)
                        {
                            values += ",";
                        }
                        values += value;
                    }

                }
                catch (Exception e)
                {

                    System.Console.WriteLine(e.Message);
                }
            }
            result = String.Format("update {0} set {1} where {2}", table, values, condition);
        }
        return result;
    }


    /// <summary>
    /// delete table where tf_id='' and  tf_id=''
    /// </summary>
    /// <returns></returns>
    public string getDeleteSQL()
    {
        SqlCommandBuilder build = new SqlCommandBuilder();

        string conditions = "", condition = "", result = "";

        if (lst != null)
        {

            foreach (Dictionary<string, object> dic in lst)
            {
                condition = "";
                foreach (string key in dic.Keys)
                {
                    try
                    {
                        if (key != keycol)
                        {
                            continue;
                        }

                        if (dic[key] != null)
                        {
                            switch (dic[key].GetType().ToString())
                            {
                                case "System.String":
                                    condition = String.Format("{0}='{1}'", key, dic[key].ToString());
                                    break;

                                case "System.Int32":
                                case "System.Boolean":
                                default:
                                    condition = String.Format("{0}={1}", key, dic[key].ToString());
                                    break;
                            }
                        }
                        break;
                    }
                    catch (Exception e)
                    {
                        System.Console.WriteLine(e.Message);
                    }
                }
                if (condition.Length > 0)
                {
                    if (conditions.Length > 0)
                    {
                        conditions += " or ";
                    }
                    conditions += condition;
                }

            }


            result = String.Format("delete {0} where {1}", table, conditions);
        }
        return result;
    }

    /// <summary>
    /// 如果以[开头说明是个list,如果以{号开头说明是个object
    /// </summary>
    /// <param name="str"></param>
    public void ParseJson(String str)
    {
        str = str.Trim();
        if (str.Substring(0, 1) == "[")
        {
            ParseJsonLst(str);
        }
        else if (str.Substring(0, 1) == "{")
        {
            if (lst != null)
            {
                if (lst.Count > 0)
                {
                    lst.RemoveRange(0, lst.Count);
                }

            }
            else
            {
                lst = new List<Dictionary<string, object>>();
                lst.Add(ParseJsonDic(str));
            }


        }

    }

    public void ParseJsonLst(String str)
    {
        lst = JSONHelper.JSONToObject<List<Dictionary<string, object>>>(str);

    }

    public Dictionary<string, object> ParseJsonDic(String str)
    {
        Dictionary<string, object> _dic = null;
        _dic = JSONHelper.JSONToObject<Dictionary<string, object>>(str);
        return _dic;
    }

    //public void ParseJsonDic(String str)
    //{
    //    dic = JSONHelper.JSONToObject<Dictionary<string, object>>(str);
    //}



    public void output(HttpContext context)
    {
        if (lst != null)
        {
            foreach (Dictionary<string, object> dic in lst)
            {

                context.Response.Write(":=========\r\n");
                foreach (string key in dic.Keys)
                {
                    try
                    {
                        if (dic[key] != null)
                        {
                            //context.Response.Write(key + ':' + dic[key].ToString() + "\r\n");

                            context.Response.Write(key + ':' + dic[key].GetType().ToString() + "\r\n");
                        }
                        else
                        {
                            context.Response.Write(key + ':' + "null" + "\r\n");

                        }
                    }
                    catch (Exception e)
                    {

                        context.Response.Write(key + ':' + e.Message + "\r\n");
                    }
                }
            }

        }
    }
}

