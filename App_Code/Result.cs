using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

/// <summary>
///result 的摘要说明
/// </summary>
/// 
//namespace DataBase{
public class Result
{
    public Boolean success;
    public int total;
    public List<Dictionary<string, object>> root;
    public String msg;

	public Result( )
	{
        success = false;
        msg = "";
        total = 0;
	}

    public void SetData( List<Dictionary<string, object>>  _data ){

        root = _data;
    }

    public void SetFlag(Boolean _flag)
    {
        success = _flag;
    }

    public void SetMsg(String _msg)
    {
        msg = _msg;
    }
    public void SetTotal(int _len)
    {
        total = _len;
    }
}

//}