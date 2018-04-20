using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Data;

/// <summary> 
/// JSON帮助类 
/// </summary> 
/// 
//namespace DataBase{
public class JSONHelper
{
    /// <summary> 
    /// 对象转JSON 
    /// </summary> 
    /// <param name="obj">对象</param> 
    /// <returns>JSON格式的字符串</returns> 
    public static string ObjectToJSON(object obj)
    {
        JavaScriptSerializer jss =new JavaScriptSerializer();
        try
        {
            return jss.Serialize(obj);
        }
        catch(Exception ex)
        {

            throw new Exception("JSONHelper.ObjectToJSON(): "+ ex.Message);
        }
    }

    /// <summary> 
    /// 数据表转JSON 
    /// </summary> 
    /// <param name="dataTable">数据表</param> 
    /// <returns>JSON字符串</returns> 
    public static string DataTableToJSON(DataTable dt)
    {
        return ObjectToJSON(DataTableToList(dt));
    }

    /// <summary> 
    /// 把DataTable转成 List集合, 存每一行 
    /// 集合中放的是键值对字典,存每一列 
    /// </summary> 
    /// <param name="dt">数据表</param> 
    /// <returns>哈希表数组</returns> 
    /// 

    
    public static List<Dictionary<string,object>>DataTableToList( DataTable dt )
    {
        List<Dictionary<string,object>> list = new List<Dictionary<string,object>>();

        

        foreach(DataRow dr in dt.Rows)
        {
            Dictionary<string,object> dic =new Dictionary<string,object>();
            foreach(DataColumn dc in dt.Columns)
            {
                dic.Add(dc.ColumnName, dr[dc.ColumnName]);
            }
            list.Add(dic);
        }
        return list;
    }



    /// <summary>
    /// 从第start条取，最多取limit条
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="start"></param>
    /// <param name="limit"></param>
    /// <returns></returns>

    public static List<Dictionary<string, object>> DataTableToList(DataTable dt, int start, int limit)
    {
        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

        int i = 0;


        foreach (DataRow dr in dt.Rows)
        {
            if( i++ < start ){
                continue;
            }

            if( i >= start+limit ){
                break;
            }

            Dictionary<string, object> dic = new Dictionary<string, object>();
            foreach (DataColumn dc in dt.Columns)
            {
                dic.Add(dc.ColumnName, dr[dc.ColumnName]);
            }
            list.Add(dic);
        }
        return list;
    }

    /// <summary> 
    /// 数据集转键值对数组字典 
    /// </summary> 
    /// <param name="dataSet">数据集</param> 
    /// <returns>键值对数组字典</returns> 
    public static Dictionary<string,List<Dictionary<string,object>>> DataSetToDic(DataSet ds)
    {
        Dictionary<string,List<Dictionary<string,object>>> result =new Dictionary<string,List<Dictionary<string,object>>>();

        foreach(DataTable dt in ds.Tables)
            result.Add(dt.TableName,DataTableToList(dt));

        return result;
    }


    /// <summary> 
    /// JSON文本转对象,泛型方法 
    /// </summary> 
    /// <typeparam name="T">类型</typeparam> 
    /// <param name="jsonText">JSON文本</param> 
    /// <returns>指定类型的对象</returns> 
    public static T JSONToObject<T>(string jsonText)
    {
        JavaScriptSerializer jss =new JavaScriptSerializer();
        try
        {
            return jss.Deserialize<T>(jsonText);
        }
        catch(Exception ex)
        { 
            throw new Exception("JSONHelper.JSONToObject(): "+ ex.Message);
        }
    }

    /// <summary> 
    /// 将JSON文本转换为数据表数据 
    /// </summary> 
    /// <param name="jsonText">JSON文本</param> 
    /// <returns>数据表字典</returns> 
    public static Dictionary<string,List<Dictionary<string,object>>> TablesDataFromJSON(string jsonText)
    {
        return JSONToObject<Dictionary<string,List<Dictionary<string,object>>>>(jsonText);
    }

    /// <summary> 
    /// 将JSON文本转换成数据行 
    /// </summary> 
    /// <param name="jsonText">JSON文本</param> 
    /// <returns>数据行的字典</returns> 
    public static Dictionary<string,object> DataRowFromJSON(string jsonText)
    {
        return JSONToObject<Dictionary<string,object>>(jsonText);
    }

    /// <summary>
    /// Json 字符串 转换为 DataTable数据集合
    /// </summary>
    /// <param name="json"></param>
    /// <returns></returns>
    public static DataTable ToDataTable( string json)
    {
        DataTable dataTable = new DataTable();  //实例化
        DataTable result;
        try
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值
            ArrayList arrayList = javaScriptSerializer.Deserialize<ArrayList>(json);
            if (arrayList.Count > 0)
            {
                foreach (Dictionary<string, object> dictionary in arrayList)
                {
                    if (dictionary.Keys.Count<string>() == 0)
                    {
                        result = dataTable;
                        return result;
                    }
                    if (dataTable.Columns.Count == 0)
                    {
                        foreach (string current in dictionary.Keys)
                        {
                            dataTable.Columns.Add(current, dictionary[current].GetType());
                        }
                    }
                    DataRow dataRow = dataTable.NewRow();
                    foreach (string current in dictionary.Keys)
                    {
                        dataRow[current] = dictionary[current];
                    }

                    dataTable.Rows.Add(dataRow);
                }
            }
        }
        catch
        {
        }
        result = dataTable;
        return result;
    }
}

//}