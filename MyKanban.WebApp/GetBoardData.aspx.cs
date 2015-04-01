using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;

using MyKanban;
using System.Data;
using Newtonsoft.Json;

/* ----------------------------------------------------------------------------- /
// File:        GetBoardData.aspx.cs
// Purpose:     Return JSON for a given board
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

public partial class GetBoardData : System.Web.UI.Page
{
    public static Hashtable htBoards = new Hashtable();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string json = "";
            bool refresh = false;
            string callback = Request.QueryString["callback"];
            long boardId = long.Parse(Request.QueryString["boardId"]);
            
            long sprintId = 0;
            long.TryParse(Request.QueryString["sprintId"], out sprintId);
            
            long projectId = 0;
            long.TryParse(Request.QueryString["projectId"], out projectId);

            long assigneeId = 0;
            long.TryParse(Request.QueryString["assigneeId"], out assigneeId);

            long approverId = 0;
            long.TryParse(Request.QueryString["approverId"], out approverId);

            string filterText = Request.QueryString["filterText"];

            bool.TryParse(Request.QueryString["refresh"], out refresh);
            string dbType = Request.QueryString["dbType"];
            string connectionString = Request.QueryString["connectionString"];

            string token = Request.QueryString.AllKeys.Contains("token") ? Request.QueryString["token"] : "";

            MyKanbanWeb.SetDbConnection(dbType, connectionString);

            Board board = new Board(new Credential(token));

            DataSet dsBoard = Data.GetAllBoardData(
                boardId,
                sprintId,
                projectId,
                assigneeId,
                approverId,
                filterText,
                new Credential(token).Id);

            Response.Write(callback + "(" + Data.GetJson(dsBoard) + ");");
            return;

        }
        catch { }
    }

    // Get a web-optimized subset of JSON for all board projects
    private string GetProjectsJson(List<Project> projects)
    {
        List<SimpleProject> simpleProjects = new List<SimpleProject>();

        foreach (Project project in projects)
        {
            SimpleProject sProject = new SimpleProject();
            sProject.Id = project.Id;
            sProject.Name = project.Name;
            simpleProjects.Add(sProject);
        }

        return Data.GetJson(simpleProjects);
    }

    private class SimpleProject
    {
        public long Id;
        public string Name;
    }

    // Get web-optimized subset of JSON for all board tasks
    private string GetTasksJson(List<Task> tasks, List<StatusCode> statusCodes)
    {
        List<SimpleTask> simpleTasks = new List<SimpleTask>();

        foreach (MyKanban.Task task in tasks)
        {
            SimpleTask sTask = new SimpleTask();
            sTask.Name = task.Name;
            sTask.ProjectName = task.ProjectName;
            sTask.StartDate = task.StartDate;
            sTask.EndDate = task.EndDate;
            sTask.EstHours = task.EstHours;
            sTask.AssignedTo = task.AssignedTo;
            sTask.ApprovedBy = task.ApprovedBy;
            sTask.Status = task.Status;
            sTask.Id = task.Id;
            sTask.ProjectId = task.ProjectId;
            sTask.DefineDone = task.DefineDone;

            StatusCode statusCode = (from scode in statusCodes
                     where scode.Id == task.Status
                     select scode).ToList<StatusCode>()[0];

            sTask.ForeColor = statusCode.ForeColor;
            sTask.BackColor = statusCode.BackColor;

            simpleTasks.Add(sTask);
        }

        return MyKanban.Data.GetJson(simpleTasks);
    }

    private class SimpleTask
    {
        public long Id;
        public long ProjectId;
        public string Name;
        public string ProjectName;
        public DateTime StartDate;
        public DateTime EndDate;
        public string DefineDone;
        public long Status;
        public string AssignedTo;
        public string ApprovedBy;
        public double EstHours;
        public string ForeColor = "black";
        public string BackColor = "silver";
    }
}