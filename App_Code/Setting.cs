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
///生成设置文件
/// </summary>
public class Setting
{
    public List<Summary> root;       // 来自于class summary

	public Setting()
	{
        root = null;
	}

    public void SetData(List<Summary> _data)
    {
        root = _data;
    }

    public void LoadData( )
    {
        ADODB db = new ADODB();
        if (db.Open())
        {
            DataTable dt = db.exec_dataset(Summary.getquerysql());
            root = new List<Summary>();

            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    Summary obj = new Summary(row, db);
                    root.Add(obj);
                }
            }
            db.Close();
        }
        return;
    }
}



public class Summary
{
    public String tf_moduleName;
    public String tf_title;
    public int tf_glyph;
    public String tf_shortname;
    public String tf_englishName;
    public String tf_englishShortName;
    public String tf_description;
    public String tf_primaryKey;
    public String tf_nameFields;




    public List<Fields>     tf_fields; 
    public List<GridScheme> tf_gridSchemes;
    public List<FormScheme> tf_formSchemes;


    static public string getquerysql()
    {
        return "select * from sm_setting_sum";
    }


    public Summary(DataRow row, ADODB db)
    {
        tf_moduleName = row.IsNull("tf_moduleName") ? "" : (string)row["tf_moduleName"];
        tf_title = row.IsNull("tf_title") ? "" : (string)row["tf_title"];


        tf_glyph = row.IsNull("tf_glyph") ? 0 : Convert.ToInt32((string)row["tf_glyph"], 16);

        tf_shortname = row.IsNull("tf_shortname") ? "" : (string)row["tf_shortname"];
        tf_englishName = row.IsNull("tf_englishName") ? "" : (string)row["tf_englishName"];
        tf_englishShortName = row.IsNull("tf_englishShortName") ? "" : (string)row["tf_englishShortName"];
        tf_description = row.IsNull("tf_description") ? "" : (string)row["tf_description"];
        tf_primaryKey = row.IsNull("tf_primaryKey") ? "" : (string)row["tf_primaryKey"];
        tf_nameFields = row.IsNull("tf_nameFields") ? "" : (string)row["tf_nameFields"];

        tf_fields = Fields.getlstdata( tf_moduleName, db );
        tf_gridSchemes = GridScheme.getlstdata( tf_moduleName, db );
        tf_formSchemes = FormScheme.getlstdata( tf_moduleName, db );
    }
}

public class Fields
{
	public String 		tf_fieldName;	// 字段名
	public String 		tf_title;		// 字段描述
	public String 		tf_fieldType;   // 字段类型
    public int          tf_l;

	public Boolean 		tf_isHidden;	// 是否是隐藏字段
	public Boolean 		tf_isRequired;	// 是否是隐藏字段
    
    public int          tf_width;

	public String 		tf_unitText;
	public String 		tf_editXType;  // 字段分组
	public String 		tf_editExtra;  // 字段分组
   

    static public string getquerysql(string id)
    {
        return String.Format("select * from sm_setting_field where tf_moduleName = '{0}' order by id asc", id);
    }

    static public List<Fields> getlstdata(string id, ADODB db)
    {

        List<Fields> lst = null;
        DataTable dt = db.exec_dataset(Fields.getquerysql(id));

        if (dt != null)
        {
            lst = new List<Fields>();

            foreach (DataRow row in dt.Rows)
            {
                Fields obj = new Fields(row);
                lst.Add(obj);
            }

        }
        return lst;

    }
    public Fields(DataRow row)
    {
        tf_fieldName = row.IsNull("tf_fieldName") ? "" : (string)row["tf_fieldName"];
        tf_title = row.IsNull("tf_title") ? "" : (string)row["tf_title"];
        tf_fieldType = row.IsNull("tf_fieldType") ? "" : (string)row["tf_fieldType"];
        
        tf_l = row.IsNull("tf_l") ? 0 : (int)row["tf_l"];

        tf_isHidden = row.IsNull("tf_isHidden") ? false : (Boolean)row["tf_isHidden"];
        tf_isRequired = row.IsNull("tf_isRequired") ? false : (Boolean)row["tf_isRequired"];
        
        tf_width = row.IsNull("tf_width") ? 0 : (int)row["tf_width"];

        tf_unitText = row.IsNull("tf_unitText") ? "" : (string)row["tf_unitText"];
        tf_editXType = row.IsNull("tf_editXType") ? "" : (string)row["tf_editXType"];
        tf_editExtra = row.IsNull("tf_editExtra") ? "" : (string)row["tf_editExtra"];
    }
}


public class GridScheme
{
    public String tf_schemeId;
    public String tf_schemeName;
    public List<GridSchemeGroup> tf_schemeGroups;    

    
    static public string getquerysql(string id)
    {
        return String.Format("select * from sm_setting_scheme where tf_moduleName = '{0}' order by tf_schemeId, id", id);
    }

    static public List<GridScheme> getlstdata(string id, ADODB db)
    {

        List<GridScheme> lst = null;
        DataTable dt = db.exec_dataset(GridScheme.getquerysql(id));

        if (dt != null)
        {
            lst = new List<GridScheme>();

            foreach (DataRow row in dt.Rows)
            {
                GridScheme obj = new GridScheme(row, db);
                lst.Add(obj);
            }

        }
        return lst;
    }

    public GridScheme(DataRow row, ADODB db)
    {
        tf_schemeId = row.IsNull("tf_schemeId") ? "" : (string)row["tf_schemeId"];
        tf_schemeName = row.IsNull("tf_schemeName") ? "" : (string)row["tf_schemeName"];
        tf_schemeGroups = GridSchemeGroup.getlstdata(tf_schemeId, db);
    }

}

public class GridSchemeGroup
{
    public String tf_gridGroupId;
    public String tf_gridGroupName;
    public Boolean tf_isShowHeaderSpans;
    public Boolean tf_isLocked;
    public List<GridGroupFields> tf_groupFields;         //来自于类gridgroupfileds


    static public string getquerysql(string id)
    {
        return String.Format("select * from sm_setting_scheme_group where tf_schemeId = '{0}' ORDER BY tf_GroupId, id", id);
    }

    static public List<GridSchemeGroup> getlstdata(string id, ADODB db)
    {

        List<GridSchemeGroup> lst = null;
        DataTable dt = db.exec_dataset( GridSchemeGroup.getquerysql(id ) );

        if (dt != null)
        {
            lst = new List<GridSchemeGroup>();

            foreach (DataRow row in dt.Rows)
            {
                GridSchemeGroup obj = new GridSchemeGroup(row, db);
                lst.Add(obj);
            }

        }
        return lst;
    }

    public GridSchemeGroup(DataRow row, ADODB db )
    {
        tf_gridGroupId = row.IsNull("tf_GroupId") ? "" : (string)row["tf_GroupId"];
        tf_gridGroupName = row.IsNull("tf_GroupName") ? "" : (string)row["tf_GroupName"];
        tf_isShowHeaderSpans = row.IsNull("tf_isShowHeaderSpans") ? false : (Boolean)row["tf_isShowHeaderSpans"];
        tf_isLocked = row.IsNull("tf_isLocked") ? false : (Boolean)row["tf_isLocked"];
        tf_groupFields = GridGroupFields.getlstdata(tf_gridGroupId, db);
    }
}
public class GridGroupFields
{    
    public String   tf_fieldName;
    public int      tf_columnWidth;

    static public string getquerysql(string id)
    {
        return String.Format("select * from sm_setting_scheme_group_field where tf_GroupId = '{0}' order by id asc", id);
    }

    static public List<GridGroupFields> getlstdata(string id, ADODB db)
    {

        List<GridGroupFields> lst = null;
        DataTable dt = db.exec_dataset(GridGroupFields.getquerysql(id));

        if (dt != null)
        {
            lst = new List<GridGroupFields>();

            foreach (DataRow row in dt.Rows)
            {
                GridGroupFields obj = new GridGroupFields(row);
                lst.Add(obj);
            }

        }
        return lst;
    }

    public GridGroupFields(DataRow row)
    {
        tf_fieldName = row.IsNull("tf_fieldName") ? "" : (string)row["tf_fieldName"];
        tf_columnWidth = row.IsNull("tf_columnWidth") ? -1 : (int)row["tf_columnWidth"];
    }
}

/// <summary>
/// 
/// </summary>
public class FormScheme
{
    public String tf_schemeId;                  // tf_schemeOrder
    public int tf_windowWidth;
    public int tf_windowHeight;
    public String tf_schemeLayout;
    public List<FormGroups> tf_schemeGroups;         // 来自于类formgroups



    static public string getquerysql(string id)
    {
        return String.Format("select * from sm_setting_scheme where tf_moduleName = '{0}' order by id", id);
    }

    static public List<FormScheme> getlstdata(string id, ADODB db)
    {

        List<FormScheme> lst = null;
        DataTable dt = db.exec_dataset(FormScheme.getquerysql(id));

        if (dt != null)
        {
            lst = new List<FormScheme>();

            foreach (DataRow row in dt.Rows)
            {
                FormScheme obj = new FormScheme(row, db);
                lst.Add(obj);
            }

        }
        return lst;
    }

    public FormScheme(DataRow row, ADODB db)
    {        
        tf_schemeId = row.IsNull("tf_schemeId") ? "" : (string)row["tf_schemeId"];
        tf_windowWidth = row.IsNull("tf_windowWidth") ? -1 : (int)row["tf_windowWidth"];
        tf_windowHeight = row.IsNull("tf_windowHeight") ? -1 : (int)row["tf_windowHeight"];
        tf_schemeLayout = row.IsNull("tf_schemeLayout") ? "" : (string)row["tf_schemeLayout"];
        tf_schemeGroups = FormGroups.getlstdata(tf_schemeId, db);
    }
}

public class FormGroups
{
    public String tf_formGroupId;
    public String tf_formGroupName;
    public int    tf_numCols;
    public List<FormgroupFields> tf_groupFields;         // 来自类formgroupfileds

    
    static public string getquerysql(string id)
    {
        return String.Format("select * from sm_setting_scheme_group where tf_schemeId = '{0}' ORDER BY tf_GroupId, id", id);
    }

    static public List<FormGroups> getlstdata(string id, ADODB db)
    {

        List<FormGroups> lst = null;
        DataTable dt = db.exec_dataset(FormGroups.getquerysql(id));

        if (dt != null)
        {
            lst = new List<FormGroups>();

            foreach (DataRow row in dt.Rows)
            {
                FormGroups obj = new FormGroups(row, db);
                lst.Add(obj);
            }

        }
        return lst;
    }

    public FormGroups(DataRow row, ADODB db)
    {        
        tf_formGroupId = row.IsNull("tf_GroupId") ? "" : (string)row["tf_GroupId"];
        tf_formGroupName = row.IsNull("tf_GroupName") ? "" : (string)row["tf_GroupName"];
        tf_numCols = row.IsNull("tf_numCols") ? 1 : (int)row["tf_numCols"];

        tf_groupFields = FormgroupFields.getlstdata(tf_formGroupId, db);

    }

}

public class FormgroupFields
{
    public String   tf_fieldName;
    public int      tf_colspan;
    public Boolean  tf_isEndRow;

    
    static public string getquerysql(string id)
    {
        return String.Format("select * from sm_setting_scheme_group_field where tf_GroupId = '{0}' order by id asc", id);
    }

    static public List<FormgroupFields> getlstdata(string id, ADODB db)
    {

        List<FormgroupFields> lst = null;
        DataTable dt = db.exec_dataset(FormgroupFields.getquerysql(id));

        if (dt != null)
        {
            lst = new List<FormgroupFields>();

            foreach (DataRow row in dt.Rows)
            {
                FormgroupFields obj = new FormgroupFields(row, db);
                lst.Add(obj);
            }
        }
        return lst;
    }

    public FormgroupFields(DataRow row, ADODB db)
    {        
        tf_fieldName = row.IsNull("tf_fieldName") ? "" : (string)row["tf_fieldName"];
        tf_colspan = row.IsNull("tf_colspan") ? 1 : (int)row["tf_colspan"];
        tf_isEndRow = row.IsNull("tf_isEndRow") ? false : (Boolean)row["tf_isEndRow"];

    }
}

//}