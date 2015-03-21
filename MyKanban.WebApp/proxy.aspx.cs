using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Net;
using System.IO;

/* ----------------------------------------------------------------------------- /
// File:        proxy.aspx.cs
// Purpose:     Intermediate page that enables one instance of MyKanban.Web to 
//              query data provided by another instance
// By:          Mark E. Gerow
// Date:        3/20/2015
// ----------------------------------------------------------------------------- /
// Mode:
// ----------------------------------------------------------------------------- /
    Copyright (c) 2015 Mark E. Gerow

    This file is part of MyKanban.

    MyKanban is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    MyKanban is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with MyKanban.  If not, see <http://www.gnu.org/licenses/>.
// -----------------------------------------------------------------------------*/

public partial class proxy : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string callback = Request.QueryString["callback"];

        string rootPath = Request.QueryString["rootPath"];

        string dbType = Request.QueryString["dbType"];
        string connectionString = Request.QueryString["connectionString"];

        if (string.IsNullOrEmpty(rootPath) || string.IsNullOrEmpty(dbType))
        {
            rootPath = "http://fw-8s50l02/MyKanban/";
            dbType = "MySql";
            connectionString = "Server=fw-8s50l02;Database=mykanban;Uid=mykanban;Pwd=megabase;";
        }

        string resp = "";
        WebRequest wReq = WebRequest.Create("http://dummy");    // So initialization won't fail needs to be some Url
        WebResponse wResp;
        StreamReader sr;
        string projectId = "";
        string assigneeId = "";
        string approverId = "";
        string taskId = "";
        string boardId = "";
        string userName = "";
        string pwd = "";
        string statusId = "";
        string assignees = "";
        string approvers = "";
        string defineDone = "";
        string estHours = "";
        string actHours = "";
        string startDate = "";
        string endDate = "";
        string name = "";
        string sprintId = "";
        string filterText = "";
        string t = Request.QueryString["timeStamp"];

        // Since token can contain special characters need to encode so they
        // don't get lost during web request
        string token = Request.QueryString.AllKeys.Contains("token") ? Server.UrlEncode(Request.QueryString["token"]) : "";

        // Call server-specific instances of various routines
        switch (Request.QueryString["mode"].ToLower())
        {

            case "deletetask":
                taskId = Request.QueryString["taskId"];
                wReq = WebRequest.Create(rootPath + "DeleteTask.aspx?taskId=" + taskId 
                    + "&dbType=" + dbType 
                    + "&connectionString=" + connectionString 
                    + "&callback=" + callback 
                    + "&t=" + t
                    + "&token=" + token);
                break;

            case "getboarddata":
                boardId = Request.QueryString["boardId"];
                sprintId = Request.QueryString["sprintId"];
                projectId = Request.QueryString["projectId"];
                assigneeId = Request.QueryString["assigneeId"];
                approverId = Request.QueryString["approverId"];
                filterText = Request.QueryString["filterText"];
                wReq = WebRequest.Create(rootPath + "GetBoardData.aspx?boardId=" + boardId 
                    + "&sprintId=" + sprintId 
                    + "&projectId=" + projectId 
                    + "&assigneeId=" + assigneeId 
                    + "&approverId=" + approverId 
                    + "&filterText=" + filterText 
                    + "&dbType=" + dbType 
                    + "&connectionString=" 
                    + connectionString 
                    + "&callback=" + callback 
                    + "&t=" + t
                    + "&token=" + token);
                break;

            case "gettaskdata":
                taskId = Request.QueryString["taskId"];
                wReq = WebRequest.Create(rootPath + "GetTaskData.aspx?taskId=" + taskId 
                    + "&dbType=" + dbType 
                    + "&connectionString=" + connectionString 
                    + "&callback=" + callback 
                    + "&t=" + t
                    + "&token=" + token);
                break;

            case "getuserdata":
                userName = Request.QueryString["userName"];
                pwd = Request.QueryString["pwd"];
                wReq = WebRequest.Create(rootPath + "GetUserData.aspx?userName=" + userName 
                    + "&pwd=" 
                    + pwd + "&dbType=" + dbType 
                    + "&connectionString=" + connectionString 
                    + "&callback=" + callback 
                    + "&t=" + t);
                break;

            case "updatetask":
                taskId = Request.QueryString["taskId"];
                if (taskId != "0")
                {
                    projectId = "0";
                }
                else
                {
                    projectId = Request.QueryString["projectId"];
                }
                defineDone = Request.QueryString["defineDone"];
                name = Request.QueryString["name"];
                assignees = Request.QueryString["assignees"];
                approvers = Request.QueryString["approvers"];
                startDate = Request.QueryString["startDate"];
                endDate = Request.QueryString["endDate"];
                estHours = Request.QueryString["estHours"];
                actHours = Request.QueryString["actHours"];
                statusId = Request.QueryString["statusId"];

                string url = "UpdateTask.aspx?taskId=" + taskId
                                    + "&projectId=" + projectId
                                    + "&name=" + name
                                    + "&defineDone=" + defineDone
                                    + "&assignees=" + assignees
                                    + "&approvers=" + approvers
                                    + "&startDate=" + startDate
                                    + "&endDate=" + endDate
                                    + "&estHours=" + estHours
                                    + "&actHours=" + actHours
                                    + "&statusId=" + statusId
                                    + "&token=" + token;

                wReq = WebRequest.Create(rootPath + url + "&dbType=" + dbType + "&connectionString=" + connectionString + "&callback=" + callback + "&t=" + t);
                break;

            case "updatetasksequence":
                boardId = Request.QueryString["boardId"];
                string serIdsInOrder = Request.QueryString["taskIds"];
                wReq = WebRequest.Create(rootPath + "UpdateTaskSequence.aspx?boardId=" + boardId 
                    + "&taskIds=" + serIdsInOrder 
                    + "&dbType=" + dbType 
                    + "&connectionString=" + connectionString 
                    + "&callback=" + callback 
                    + "&t=" + t
                    + "&token=" + token);
                break;

            case "updatetaskstatus":
                taskId = Request.QueryString["taskId"];
                statusId = Request.QueryString["statusId"];
                wReq = WebRequest.Create(rootPath + "UpdateTaskStatus.aspx?taskId=" + taskId 
                    + "&statusId=" + statusId 
                    + "&dbType=" + dbType 
                    + "&connectionString=" + connectionString 
                    + "&callback=" + callback 
                    + "&t=" + t
                    + "&token=" + token);
                break;

            default:
                break;
        }

        // Call the page and get the resulting JSON data
        wReq.Credentials = System.Net.CredentialCache.DefaultCredentials;
        wResp = wReq.GetResponse();
        sr = new StreamReader(wResp.GetResponseStream());
        resp = sr.ReadToEnd();
        sr.Close();

        // Return the result
        Response.Write(resp);
    }
}