using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using MyKanban;

public partial class GetUserData : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string callback = Request.QueryString["callback"];
            string userName = Request.QueryString["userName"];
            string pwd = Request.QueryString["pwd"];

            string dbType = Request.QueryString["dbType"];
            string connectionString = Request.QueryString["connectionString"];

            MyKanbanWeb.SetDbConnection(dbType, connectionString);

            Credential credential = new Credential(userName, pwd);
            string json = MyKanban.Data.GetJson(credential.Boards);
            Response.Write(callback + "(" + json + ");");
        }
        catch (Exception ex)
        {
            Response.Write("Error: " + ex.Message);
        }

    }
}