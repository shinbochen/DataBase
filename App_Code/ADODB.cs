using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


using System.Data.SqlClient;
using System.ComponentModel;
using System.Collections.Generic;

/// <summary>
///ADODB 的摘要说明
/// </summary>
//namespace DataBase
//{
    public class ADODB
    {

        public SqlConnection m_conn = null;
        public ADODB()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        public SqlConnection getConnection()
        {
            return m_conn;
        }

        // using the connect string
        public bool Open()
        {
            bool bResult = false;
            string connection;

            connection = ConfigurationManager.ConnectionStrings["dbconn"].ToString();
            m_conn = new SqlConnection(connection);
            try
            {
                m_conn.Open();
                bResult = true;
            }
            catch (SqlException e)
            {

                bResult = false;
                System.Console.WriteLine(e.Source);
            }
            return bResult;

        }

        public bool Open(string server, string database, string user, string psd)
        {

            bool bResult = false;
            string connection;

            // Security Support Provider Interface
            // 设置Integrated Security为 True 的时候，连接语句前面的 UserID, PW 是不起作用的，即采用windows身份验证模式。
            // 只有设置为 False 或省略该项的时候，才按照 UserID, PW 来连接。
            // Integrated Security 可以设置为: True, false, yes, no ，这四个的意思很明白了，还可以设置为：sspi ，相当于 True，建议用这个代替 True。

            connection = ConfigurationManager.ConnectionStrings["dbconn"].ToString();

            // 少于10个认为不是正确的连接字符
            //if (connection.Length < 10)
            //{
            //    connection = String.Format("Server={0};Database={1};uid={2};pwd={3};Connect Timeout=30;Pooling=true;Max Pool Size=5;Min Pool Size=3;integrated security=false;",
            //                              server, database, user, psd);
            //}


            m_conn = new SqlConnection(connection);
            try
            {
                m_conn.Open();
                bResult = true;
            }
            catch (SqlException e)
            {

                bResult = false;
                System.Console.WriteLine(e.Source);
            }
            return bResult;
        }

        public bool Close()
        {
            if (m_conn != null)
            {
                m_conn.Close();
            }
            return true;
        }

        public List<string> getTableColName(string table)
        {
            string sql = "";
            List<string> lst = new List<string>();

            DataTable dt = null;

            sql = String.Format("select   name   from   syscolumns   where   id=object_id('{0}')", table);

            dt = exec_dataset(sql);

            foreach (DataRow row in dt.Rows)
            {
                lst.Add(row[0].ToString());
            }
            return lst;
        }

        public DataTable exec_dataset(string sql)
        {
            SqlDataAdapter adapter = null;
            DataTable dt = null;

            try
            {
                adapter = new SqlDataAdapter(sql, m_conn);

                if (adapter != null)
                {
                    dt = new DataTable();
                    adapter.Fill(dt);
                }
            }
            catch (SqlException ex)
            {
                System.Console.WriteLine(ex.Source + ex.Server);
            }
            return dt;
        }
        // 用来执行新增，删除，更新指令

        public int execNonQuery(string sql)
        {
            int result = -1;
            try
            {
                SqlCommand cmd = new SqlCommand(sql, m_conn);
                result = cmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                result = -2;
            }
            return result;
        }

        public String exec(string sql)
        {

            SqlDataAdapter adapter = null;
            DataTable dt = null;

            String result = "";

            try
            {
                adapter = new SqlDataAdapter(sql, m_conn);

                if (adapter != null)
                {
                    dt = new DataTable();

                    adapter.Fill(dt);

                    foreach (DataColumn column in dt.Columns)
                    {
                        if (result.Length > 0)
                        {
                            result += ':';
                        }
                        result += String.Format("{0}[{1}]", column.ColumnName, column.Caption);
                    }

                    result += "\r\n";

                    foreach (DataRow row in dt.Rows)
                    {

                        foreach (DataColumn column in dt.Columns)
                        {
                            result += String.Format("{0}:{1},\r\n", column.ColumnName, row[column.ColumnName]);
                        }
                    }

                }
            }
            catch (SqlException ex)
            {
                System.Console.WriteLine(ex.Source + ex.Server);
                result = "error" + ex.Source + ex.Server;
            }
            return result;
        }

        /// <summary> 
        /// 数据表转键值对集合  
        /// 把DataTable转成 List集合, 存每一行 
        /// 集合中放的是键值对字典,存每一列 
        /// </summary> 
        /// <param name="dt">数据表</param> 
        /// <returns>哈希表数组</returns> 
        public static List<Dictionary<string, object>> DataTableToList(DataTable dt)
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

            foreach (DataRow dr in dt.Rows)
            {
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
        public static Dictionary<string, List<Dictionary<string, object>>> DataSetToDic(DataSet ds)
        {
            Dictionary<string, List<Dictionary<string, object>>> result = new Dictionary<string, List<Dictionary<string, object>>>();

            foreach (DataTable dt in ds.Tables)
                result.Add(dt.TableName, DataTableToList(dt));

            return result;
        }


    }
//}