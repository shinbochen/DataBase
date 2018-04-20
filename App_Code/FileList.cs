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

using System.IO;
using System.Collections.Generic;


/// <summary>
///FileList 的摘要说明
/// </summary>
public class FileList
{
	public FileList()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}


    static public List<Dictionary<string, object>> getAllFile(string path, string parentPath)
    {

        List<Dictionary<string, object>> lst = new List<Dictionary<string, object>>();

        DirectoryInfo dirInfo = new DirectoryInfo(path);
        FileSystemInfo[] dir = dirInfo.GetFileSystemInfos();
        foreach (FileSystemInfo di in dir)
        {
            if (di.Attributes == (System.IO.FileAttributes.Hidden | System.IO.FileAttributes.System | System.IO.FileAttributes.Directory))
            {
                continue;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();

            dic.Add("name", di.Name);
            dic.Add("url", parentPath + "\\" + di.Name);

            dic.Add("lastmod", di.LastAccessTime);

            dic.Add("lastmodstr", di.LastAccessTime.ToLongDateString() );

            lst.Add(dic);
        }
        return lst;
    }
}
