using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Net;
using System.IO;

namespace MyKanban.WebApp
{
    /// <summary>
    /// Summary description for proxy
    /// </summary>
    public class proxy : IHttpHandler
    {
        private string rootPath = "http://gerow1.azurewebsites.net/";

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string callback = context.Request.QueryString["callback"];
            string resp = "";
            WebRequest wReq = WebRequest.Create("http://dummy");    // So initialization won't fail needs to be some Url
            WebResponse wResp;
            StreamReader sr;
            string projectId = "";
            string taskId = "";
            string boardId = "";
            string userName = "";
            string pwd = "";
            string statusId = "";
            string assignees = "";
            string defineDone = "";
            string estHours = "";
            string actHours = "";
            string startDate = "";
            string endDate = "";
            string name = "";

            switch (context.Request.QueryString["mode"].ToLower())
            {
                case "deletetask":
                    taskId = context.Request.QueryString["taskId"];
                    wReq = WebRequest.Create(rootPath + "DeleteTask.aspx?taskId=" + taskId + "&callback=" + callback);
                    break;

                case "getboarddata":
                    boardId = context.Request.QueryString["boardId"];
                    wReq = WebRequest.Create(rootPath + "GetBoardData.aspx?boardId=" + boardId + "&callback=" + callback);
                    break;

                case "gettaskdata":
                    taskId = context.Request.QueryString["taskId"];
                    wReq = WebRequest.Create(rootPath + "GetTaskData.aspx?taskId=" + taskId + "&callback=" + callback);
                    break;

                case "getuserdata":
                    userName = context.Request.QueryString["userName"];
                    pwd = context.Request.QueryString["pwd"];
                    wReq = WebRequest.Create(rootPath + "GetUserData.aspx?userName=" + userName + "&pwd=" + pwd + "&callback=" + callback);
                    break;

                case "updatetask":
                    taskId = context.Request.QueryString["taskId"];
                    if (taskId != "0")
                    {
                        projectId = "0";
                    }
                    else
                    {
                        projectId = context.Request.QueryString["projectId"];
                    }
                    defineDone = context.Request.QueryString["defineDone"];
                    name = context.Request.QueryString["name"];
                    assignees = context.Request.QueryString["assignees"];
                    startDate = context.Request.QueryString["startDate"];
                    endDate = context.Request.QueryString["endDate"];
                    estHours = context.Request.QueryString["estHours"];
                    actHours = context.Request.QueryString["actHours"];
                    statusId = context.Request.QueryString["statusId"];

                    string url = "UpdateTask.aspx?taskId=" + taskId
                                        + "&projectId=" + projectId
                                        + "&name=" + name
                                        + "&defineDone=" + defineDone
                                        + "&assignees=" + assignees
                                        + "&startDate=" + startDate
                                        + "&endDate=" + endDate
                                        + "&estHours=" + estHours
                                        + "&actHours=" + actHours
                                        + "&status=" + statusId;

                    wReq = WebRequest.Create(rootPath + url + "&callback=" + callback);
                    break;

                case "updatetaskstatus":
                    taskId = context.Request.QueryString["taskId"];
                    statusId = context.Request.QueryString["statusId"];
                    wReq = WebRequest.Create(rootPath + "UpdateTaskStatus.aspx?taskId=" + taskId + "&statusId=" + statusId + "&callback=" + callback);
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
            context.Response.Write(resp);

        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}