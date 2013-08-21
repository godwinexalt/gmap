using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for DataBase
/// </summary>
/// 

public class DataBase
{
    public SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Con"].ConnectionString);
    //public SqlConnection cona = new SqlConnection(ConfigurationManager.ConnectionStrings["Appserv"].ConnectionString);
    SqlCommand cmd;
    SqlDataAdapter adp;
    DataSet ds;

    public DataBase()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public void AUDOP(string str)
    {
        if (con.State == ConnectionState.Open)
        {
            con.Close();
        }
        con.Open();
        cmd = new SqlCommand(str, con);
        cmd.ExecuteNonQuery();
        con.Close();
    }

    public int AUDOPID(string str)
    {
        if (con.State == ConnectionState.Open)
        {
            con.Close();
        }
        con.Open();
        cmd = new SqlCommand(str, con);
        int id = Int32.Parse(cmd.ExecuteScalar().ToString());
        con.Close();

        return id;
    }

    public DataSet FillDataSet(string str)
    {
        if (con.State == ConnectionState.Open)
        {
            con.Close();
        }
        con.Open();
        adp = new SqlDataAdapter(str, con);
        ds = new DataSet();
        adp.Fill(ds);
        con.Close();
        return (ds);
    }

    public int IsRowExist(string str)
    {
        int i = 0;
        if (con.State == ConnectionState.Open)
        {
            con.Close();
        }
        con.Open();
        ds = new DataSet();
        ds = FillDataSet(str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            i = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows.Count));
        }
        con.Close();
        return (i);
    }

    public void SelectAllCheckBox(string header, string item, GridView gvtemp)
    {
        CheckBox chk = new CheckBox();
        chk = (CheckBox)gvtemp.HeaderRow.FindControl(header);
        CheckBox chk1 = new CheckBox();
        if (chk.Checked == true)
        {
            for (int i = 0; i < gvtemp.Rows.Count; i++)
            {
                chk1 = (CheckBox)gvtemp.Rows[i].FindControl(item);
                chk1.Checked = true;
            }
        }
        else
        {
            for (int i = 0; i < gvtemp.Rows.Count; i++)
            {
                chk1 = (CheckBox)gvtemp.Rows[i].FindControl(item);
                chk1.Checked = false;
            }
        }
    }

    public string ReplaceString(string str)
    {
        string abc;
        abc = str.Replace("'", "''");
        return abc;
    }

    public void filldropdown(string parameter1, string parameter2, string tablename, string parameter3, string parameter4, DropDownList drp)
    {
        try
        {
            string str = String.Format("select {0},{1} from {2} where {3} ={4}", parameter1, parameter2, tablename, parameter3, parameter4);
            ds = new DataSet();
            ds = FillDataSet(str);
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        drp.Items.Insert(i, new ListItem(ds.Tables[0].Rows[i][1].ToString(), ds.Tables[0].Rows[i][0].ToString()));
                    }
                }
            }
        }
        catch
        { }
    }

    public Int32 GetScalar(string str)
    {
        int result = 0;
        try
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            cmd = new SqlCommand(str, con);
            result = Convert.ToInt32(cmd.ExecuteScalar().ToString());
        }
        catch (Exception ex)
        {

        }
        finally
        {
            con.Close();
        }
        return result;
    }

    //public void filltabstrip(string parameter1, string parameter2, string tablename, string parameter3, string parameter4, ComponentArt.Web.UI.TabStrip tab)
    //{
    //    try
    //    {
    //        string str = String.Format("select {0},{1} from {2} where {3} ={4}", parameter1, parameter2, tablename, parameter3, parameter4);
    //        ds = new DataSet();
    //        ds = FillDataSet(str);
    //        if (ds != null)
    //        {
    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
    //                {
    //                    ComponentArt.Web.UI.TabStripTab tab1 = new ComponentArt.Web.UI.TabStripTab();
    //                    tab1.Text = ds.Tables[0].Rows[i][1].ToString();
    //                    tab1.Value = ds.Tables[0].Rows[i][0].ToString();
    //                    tab.Tabs.Add(tab1);
    //                }
    //            }
    //        }
    //    }
    //    catch
    //    { }
    //}
}