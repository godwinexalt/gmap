using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Summary description for WebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class WebService : System.Web.Services.WebService {

    public SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Con"].ConnectionString);
    SqlDataAdapter adp;
    DataSet ds;

    public WebService () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public XmlElement GetCurrentPosition()
    {
        DataSet ds = new DataSet();
        ds = FillDataSet("SELECT TOP 1 * FROM Location ORDER BY ID DESC");
        XmlElement xE = (XmlElement)Serialize(ds);
        return xE;
    }

    [WebMethod]
    public XmlElement GetRoute()
    {
        DataSet ds = new DataSet();
        ds = FillDataSet("SELECT TOP 20 * FROM Location ORDER BY ID DESC");
        XmlElement xE = (XmlElement)Serialize(ds);
        string strXml = xE.OuterXml.ToString();
        return xE;
    }

    public XmlElement Serialize(object transformObject)
    {
        XmlElement serializedElement = null;
        try
        {
            MemoryStream memStream = new MemoryStream();
            XmlSerializer serializer = new XmlSerializer(transformObject.GetType());
            serializer.Serialize(memStream, transformObject);
            memStream.Position = 0;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(memStream);
            serializedElement = xmlDoc.DocumentElement;
        }
        catch (Exception SerializeException)
        {

        }
        return serializedElement;
    }

    public DataSet FillDataSet(string str)
    {
        if (con.State == ConnectionState.Open)
        {
            con.Close();
        }
        con.Open();
        adp = new SqlDataAdapter(str, con);
        ds = new DataSet("LocationDate");
        adp.Fill(ds,"Location");
        con.Close();
        return (ds);
    }

}

