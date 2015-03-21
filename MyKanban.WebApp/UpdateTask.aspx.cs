using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using MyKanban;

/* ----------------------------------------------------------------------------- /
// File:        UpdateTask.aspx.cs
// Purpose:     Update task data in MyKanban database
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

public partial class UpdateTask : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string callback = Request.QueryString["callback"];
            long taskId = long.Parse(Request.QueryString["taskId"]);
            long projectId = long.Parse(Request.QueryString["projectId"]);
            string defineDone = MyKanbanWeb.SpecialDecoding(Request.QueryString["defineDone"]);
            string name = MyKanbanWeb.SpecialDecoding(Request.QueryString["name"]);
            string[] assignees = Request.QueryString["assignees"].Split(';');
            string[] approvers = Request.QueryString["approvers"].Split(';');
            string dbType = Request.QueryString["dbType"];
            string connectionString = Request.QueryString["connectionString"];
            string token = Request.QueryString.AllKeys.Contains("token") ? Request.QueryString["token"] : "";

            MyKanbanWeb.SetDbConnection(dbType, connectionString);

            DateTime startDate;
            try
            {
                startDate = DateTime.Parse(Request.QueryString["startDate"]);
            }
            catch
            {
                startDate = DateTime.Now;
            }
            DateTime endDate;
            try
            {
                endDate = DateTime.Parse(Request.QueryString["endDate"]);
            } 
            catch
            {
                endDate = DateTime.Now;
            }
            double estHours = double.Parse(Request.QueryString["estHours"]);
            double actHours = double.Parse(Request.QueryString["actHours"]);
            long statusId = 0;
            long.TryParse(Request.QueryString["statusId"], out statusId);

            // If task ID# passed in, we're updating an existing task,
            // otherwise, this is a new task, and so will need to add it
            // to a project as well.
            Project project = null;
            Task task = null;
            if (taskId > 0)
            {
                task = new Task(taskId, new Credential(token));
            }
            else
            {
                project = new Project(projectId, new Credential(token));
                task = new Task(project.Credential);

                // Default to first status code in list
                task.Status = project.StatusCodes[0].Id;
            }

            // Set the rest of the task properties
            task.DefineDone = defineDone;
            task.Name = name;
            task.StartDate = startDate;
            task.EndDate = endDate;
            task.EstHours = estHours;
            task.ActHours = actHours;
            task.Status = statusId;

            // Update task assignees
            task.Assignees.Clear(true);
            for (int i = 0; i < assignees.Length; i++)
            {
                if (!string.IsNullOrEmpty(assignees[i]))
                {
                    Person assignee = new Person(long.Parse(assignees[i]), task.Credential);
                    task.Assignees.Add(assignee);
                }
            }

            // Update task approvers
            task.Approvers.Clear(true);
            for (int i = 0; i < approvers.Length; i++)
            {
                if (!string.IsNullOrEmpty(approvers[i]))
                {
                    Approver approver = new Approver(task.Credential);
                    approver.PersonId = long.Parse(approvers[i]);
                    approver.Update(true);
                    task.Approvers.Add(approver);
                    task.Approvers.Update(true);
                }
            }

            // Save updates (if task # is 0, will be added)
            task.Update();

            // If a project # passed in, this means that this is a new
            // task, so add it to the parent project's task collection
            if (projectId > 0)
            {
                project.Tasks.Add(task);
                project.Tasks.Update();
            }

            Response.Write(callback + "(" + task.JSON() + ");");
        }
        catch (Exception ex)
        {
            Response.Write("{'Error': '" + ex.Message + "'}");
        }
    }
}