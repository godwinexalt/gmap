using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Xml;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class GetRoute : System.Web.UI.Page
{
    public SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Con"].ConnectionString);
    DataBase dbObj = new DataBase();

    protected void Page_Load(object sender, EventArgs e)
    {
        //string origin = "Thiruvananthapuram, Kerala, India";
        //string destination = "Kazhakkoottam, Kerala, India";

        DataTable dtSrc = new DataTable();
        dtSrc = dbObj.FillDataSet("SELECT TOP 1 * FROM Destination WHERE IsReached=1 ORDER BY ID DESC").Tables[0];

        DataTable dtDest = new DataTable();
        dtDest = dbObj.FillDataSet("SELECT TOP 1 * FROM Destination WHERE IsReached IS NULL ORDER BY ID ASC").Tables[0];

        if (dtSrc.Rows.Count > 0 && dtDest.Rows.Count > 0)
        {
            string origin = dtSrc.Rows[0]["Name"].ToString();
            string destination = dtDest.Rows[0]["Name"].ToString();

            var requestUrl = string.Format("http://maps.google.com/maps/api/directions/xml?origin={0}&destination={1}&sensor=false", origin, destination);

            var client = new WebClient();
            var result = client.DownloadString(requestUrl);
            //List<DirectionSteps> dSteps = ParseDirectionResults(result);

            DataTable dtDir = ParseDirectionResults2(result);

            foreach (DataRow dr in dtDir.Rows)
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                con.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Location VALUES('" + dr["Latitude"] + "','" + dr["Longitude"] + "')", con);
                cmd.ExecuteNonQuery();
                con.Close();

                int duration = Convert.ToInt32(dr["Duration"].ToString().Replace("mins", "").Replace("min", "").Trim());
               // System.Threading.Thread.Sleep(duration * 60 * 1000);
            }

            dbObj.AUDOP("UPDATE Destination SET IsReached=1 WHERE ID=" + dtDest.Rows[0]["ID"].ToString());
        }
    }

    private static DataTable ParseDirectionResults2(string result)
    {
        DataTable dtDir = new DataTable();
        dtDir.Columns.Add("Latitude");
        dtDir.Columns.Add("Longitude");
        dtDir.Columns.Add("Duration");

        var xmlDoc = new XmlDocument { InnerXml = result };
        if (xmlDoc.HasChildNodes)
        {
            var directionsResponseNode = xmlDoc.SelectSingleNode("DirectionsResponse");
            if (directionsResponseNode != null)
            {
                var statusNode = directionsResponseNode.SelectSingleNode("status");
                if (statusNode != null && statusNode.InnerText.Equals("OK"))
                {
                    var legs = directionsResponseNode.SelectNodes("route/leg");
                    foreach (XmlNode leg in legs)
                    {
                        var stepNodes = leg.SelectNodes("step");
                        foreach (XmlNode stepNode in stepNodes)
                        {

                            //string distance = stepNode.SelectSingleNode("distance/text").InnerText;
                            string duration = stepNode.SelectSingleNode("duration/text").InnerText;
                            string latitude = stepNode.SelectSingleNode("end_location/lat").InnerText;
                            string longitude = stepNode.SelectSingleNode("end_location/lng").InnerText;

                            dtDir.Rows.Add(new object[] { latitude, longitude, duration });
                        }
                    }
                }
            }
        }
        return dtDir;
    }

    //private static List<DirectionSteps> ParseDirectionResults(string result)
    //    {
    //        var directionStepsList = new List<DirectionSteps>();
    //        var xmlDoc = new XmlDocument {InnerXml = result};
    //        if (xmlDoc.HasChildNodes)
    //        {
    //            var directionsResponseNode = xmlDoc.SelectSingleNode("DirectionsResponse");
    //            if (directionsResponseNode != null)
    //            {
    //                var statusNode = directionsResponseNode.SelectSingleNode("status");
    //                if (statusNode != null && statusNode.InnerText.Equals("OK"))
    //                {
    //                    var legs = directionsResponseNode.SelectNodes("route/leg");
    //                    foreach (XmlNode leg in legs)
    //                    {
    //                        int stepCount = 1;
    //                        var stepNodes = leg.SelectNodes("step");
    //                        var steps = new List<DirectionStep>();
    //                        foreach (XmlNode stepNode in stepNodes)
    //                        {
    //                            var directionStep = new DirectionStep();
    //                            directionStep.Index = stepCount++;
    //                            directionStep.Distance = stepNode.SelectSingleNode("distance/text").InnerText;
    //                            directionStep.Duration = stepNode.SelectSingleNode("duration/text").InnerText;
    //                            directionStep.Latitude = stepNode.SelectSingleNode("end_location/lat").InnerText;
    //                            directionStep.Longitude = stepNode.SelectSingleNode("end_location/lng").InnerText;

    //                            directionStep.Description = Regex.Replace(stepNode.SelectSingleNode("html_instructions").InnerText, "<[^<]+?>", "");
    //                            steps.Add(directionStep);
    //                        }

    //                        var directionSteps = new DirectionSteps();
    //                        directionSteps.OriginAddress = leg.SelectSingleNode("start_address").InnerText;
    //                        directionSteps.DestinationAddress = leg.SelectSingleNode("end_address").InnerText;
    //                        directionSteps.TotalDistance = leg.SelectSingleNode("distance/text").InnerText;
    //                        directionSteps.TotalDuration = leg.SelectSingleNode("duration/text").InnerText;
    //                        directionSteps.Steps = steps;

    //                        directionStepsList.Add(directionSteps);
    //                    }
    //                }
    //            }
    //        }
    //        return directionStepsList;
    //    }
}

//public class DirectionStep
//{
//    public int Index { get; set; }
//    public string Description { get; set; }
//    public string Distance { get; set; }
//    public string Duration { get; set; }
//    public string Latitude { get; set; }
//    public string Longitude { get; set; }
//}

//public class DirectionSteps
//{
//    public string TotalDuration { get; set; }
//    public string TotalDistance { get; set; }
//    public string OriginAddress { get; set; }
//    public string DestinationAddress { get; set; }
//    public List<DirectionStep> Steps { get; set; }
//}
