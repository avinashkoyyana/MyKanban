using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;

using MyKanban;

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

            Board board;
            board = new Board(boardId, new Credential(token));

            List<Task> tasks = board.Tasks.Items;

            // Sprint filter
            if (sprintId != 0)
            {
                Sprint sprint = new Sprint(sprintId, board.Credential);
                tasks = (from task in tasks
                    where task.StartDate <= sprint.EndDate && task.EndDate >= sprint.StartDate
                    orderby task.Sequence
                        select task).ToList<Task>();
            }

            // Project filter
            if (projectId != 0)
            {
                tasks = (from task in tasks
                         where task.ProjectId == projectId
                         orderby task.Sequence
                         select task).ToList<Task>();
            }

            // Assignee filter
            if (assigneeId != 0)
            {
                for (int i = tasks.Count - 1; i >= 0; i--)
                {
                    bool hasAssignee = false;
                    for (int j = 0; j < tasks[i].Assignees.Count; j++)
                    {
                        hasAssignee = (tasks[i].Assignees[j].Id == assigneeId);
                        if (hasAssignee) break;
                    }
                    if (!hasAssignee) tasks.Remove(tasks[i]);
                }
            }

            // Approver filter
            if (approverId != 0)
            {
                for (int i = tasks.Count - 1; i >= 0; i--)
                {
                    bool hasApprover = false;
                    for (int j = 0; j < tasks[i].Approvers.Count; j++)
                    {
                        hasApprover = (tasks[i].Approvers[j].PersonId == approverId);
                        if (hasApprover) break;
                    }
                    if (!hasApprover) tasks.Remove(tasks[i]);
                }
            }

            // Text filter
            if (!string.IsNullOrEmpty(filterText))
            {
                tasks = (from task in tasks
                         where task.Name.ToLower().Contains(filterText.ToLower()) || task.DefineDone.ToLower().Contains(filterText.ToLower())
                         orderby task.Sequence
                         select task).ToList<Task>();
            }

            BoardSet boardSet = new BoardSet(board.BoardSetId, board.Credential);

            json = "[";

            json += boardSet.StatusCodes.JSON();
            
            json += ", " + GetTasksJson(tasks, boardSet.StatusCodes.Items);
            
            json += ", " + board.Users.JSON();

            json += ", " + GetProjectsJson(board.Projects.Items);

            json += ", " + board.Sprints.JSON();

            json += ", {";
            json += "\"CanAdd\" : " + board.CanAdd.ToString().ToLower();
            json += ", \"CanDelete\" : " + board.CanDelete.ToString().ToLower();
            json += ", \"CanEdit\" : " + board.CanEdit.ToString().ToLower();
            json += ", \"CanRead\" : " + board.CanRead.ToString().ToLower();
            json += "}";
                
            json += "]";

            Response.Write(callback + "(" + json + ");");
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